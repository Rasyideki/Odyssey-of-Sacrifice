using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public float delayBeforePlay = 10f; 

    void Start()
    {
        Invoke("StartVideoPlayback", delayBeforePlay); 
    }

    void StartVideoPlayback()
    {
        videoPlayer.loopPointReached += OnVideoEnd; 
        videoPlayer.Play(); 
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
