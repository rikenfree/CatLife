using UnityEngine;
using UnityEngine.Video;
using SuperStarSdk;

public class StoryScript : MonoBehaviour
{
    public string url;

    public VideoPlayer videoPlayer;

    public void SetURLAndPlayVideo()
    {
        UiManager.instance.loadingScreen.SetActive(true);
        VideoController.instance.currentUrl = url;
    }
}
