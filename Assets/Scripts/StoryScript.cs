using UnityEngine;
using UnityEngine.Video;
using SuperStarSdk;

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
        UiManager.instance.loadingScreen.SetActive(true);

        // if (SuperStarAd.Instance.NoAds == 0)
        // {
        //     SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
        //     {
        //         VideoController.instance.PlayVideo(url);
        //     }, 3);
        // }
        // else
        // {
        //     VideoController.instance.PlayVideo(url);
        // }
    }
}
