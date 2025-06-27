using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public GameObject videoDisplay;

    public static VideoController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        
    }

    public void PlayVideo(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
        videoDisplay.SetActive(true);
    }
}