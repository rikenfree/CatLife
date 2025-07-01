using UnityEngine;
using UnityEngine.Video;
using SuperStarSdk;

public class StoryScript : MonoBehaviour
{
    public string url;

    public VideoPlayer videoPlayer;

    public void SetURLAndPlayVideo()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                SuperStarAd.Instance.HideBannerAd();
                UiManager.instance.loadingScreen.SetActive(true);
                VideoController.instance.currentUrl = url;
            }, 3);
        }
        else
        {
            SuperStarAd.Instance.HideBannerAd();
            UiManager.instance.loadingScreen.SetActive(true);
            VideoController.instance.currentUrl = url;
        }
    }
}
