using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using GoogleARCore;



public class AugmentedImageVisualizer : MonoBehaviour {
    [SerializeField] private VideoClip[] _videoClips;//array of video clips
    public AugmentedImage Image;
    private VideoPlayer _videoPlayer;

    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += OnStop;//Stops video

    }

    private void OnStop(VideoPlayer source)
    {
        gameObject.SetActive(false);//makes video disappear
    }

    // Update is called once per frame
    void Update()
    {
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            return;//making sure the image and tracking is there to even run
        }

        if (!_videoPlayer.isPlaying)
        {
            _videoPlayer.clip = _videoClips[Image.DatabaseIndex];
            _videoPlayer.Play();
            //if our video isnt playing we will play the correct one in our array
        }
        transform.localScale = new Vector3(Image.ExtentX, Image.ExtentZ, 1); //needs to change the dimesions of the image every frame
    }
}
