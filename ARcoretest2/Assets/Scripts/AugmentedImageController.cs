using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class AugmentedImageController : MonoBehaviour
{
    //serialized meaning, can input this from unity, we will drag it in, and this object will be available to the controller
    [SerializeField] private AugmentedImageVisualizer _augmentedImageVisualizer;

    private readonly Dictionary<int, AugmentedImageVisualizer> _visualizers =
        new Dictionary<int, AugmentedImageVisualizer>(); 
    //keep an instantiated list of visualizers that we currently have so we dont have to keep creating new ones.
    private readonly List<AugmentedImage> _images = new List<AugmentedImage>();//list of images that we are tracking

private void Update()
{

    if (Session.Status != SessionStatus.Tracking)
    {
        return;
    }

    Session.GetTrackables(_images, TrackableQueryFilter.Updated);
    VisualizeTrackables();
    //when we find an imnage we will put something on top of that. track:get ahold of them and put the visualizers on top of them.
}
private void VisualizeTrackables()
{
    foreach (var image in _images)
    {
        var visualizer = GetVisualizer(image);
        if (image.TrackingState == TrackingState.Tracking && visualizer == null)
        {
            AddVisualizer(image);//if tracking add augment
        }
        else if (image.TrackingState == TrackingState.Stopped && visualizer != null)

        {
            RemoveVisualizer(image, visualizer);// if it has lost tracking remove te augment
           
        }
    }
}

    private void RemoveVisualizer(AugmentedImage image, AugmentedImageVisualizer visualizer)

{
    _visualizers.Remove(image.DatabaseIndex);
    Destroy(visualizer.gameObject);
}

private void AddVisualizer(AugmentedImage image)
{
    var anchor = image.CreateAnchor(image.CenterPose);
    var visualizer = Instantiate(_augmentedImageVisualizer, anchor.transform);
    _visualizers.Add(image.DatabaseIndex, visualizer);

}

private AugmentedImageVisualizer GetVisualizer(AugmentedImage image)
{
    AugmentedImageVisualizer visualizer;
    _visualizers.TryGetValue(image.DatabaseIndex, out visualizer);

    return visualizer;

}
    }




            