using UnityEngine;
using UnityEngine.Video;

public class StoryScript : MonoBehaviour
{
    public string url;

    public VideoPlayer videoPlayer;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetURLAndPlayVideo()
    {
        VideoController.instance.PlayVideo(url);
    }
}
