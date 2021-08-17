using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour {

    [SerializeField] private VideoPlayer videoPlayer;
    private GameplayController gameplayController;

    // Start is called before the first frame update
    void Start() {
        gameplayController = GameObject.Find("GameplayController").GetComponent<GameplayController>();

        videoPlayer.loopPointReached += VideoEndReached;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void PlayVideo() {
        StopAllCoroutines();

        StopVideo();

        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "me.mp4");

        videoPlayer.Play();
    }

    public void StopVideo() {
        if (videoPlayer.isPlaying) {
            videoPlayer.Stop();
        }
    }

    public bool IsPlaying() {
        return videoPlayer.isPlaying;
    }

    private void VideoEndReached(VideoPlayer vp) {
        gameplayController.VideoClipEnded(false);
    }
}
