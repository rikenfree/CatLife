using UnityEngine;
using UnityEngine.Video;
using SuperStarSdk;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoDisplay;
    public GameObject panelToActivate;
    public GameObject downloadOptions;
    public GameObject allOptionObj;

    public static VideoController instance;
    public string currentUrl;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        // Make sure panel is disabled at start
        if (panelToActivate != null)
            panelToActivate.SetActive(false);
    }

    public void PlayVideo(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
        videoDisplay.SetActive(true);
        APIManager.instance.favouriteVideoUrl = url;
        UiManager.instance.loadingScreen.SetActive(false);
        if (videoPlayer != null)
        {
            // Register for video end event
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (panelToActivate != null)
            panelToActivate.SetActive(true); // Show panel when video ends
    }

    public void OpenDownloadOption(bool isActive)
    {
        downloadOptions.SetActive(isActive);
    }

    public void PlayURLVideo()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                PlayVideo(currentUrl);
            }, 3);
        }
        else
        {
            PlayVideo(currentUrl);
        }
    }

}