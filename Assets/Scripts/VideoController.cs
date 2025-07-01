using UnityEngine;
using UnityEngine.Video;
using SuperStarSdk;

public class VideoController : MonoBehaviour
{
    [Header("Video Player")]
    public VideoPlayer videoPlayer;
    public GameObject videoDisplay;
    public GameObject panelToActivate;

    [Header("Download Options")]
    public GameObject downloadOptions;
    public GameObject allOptionObj;

    [Header("Favorite Video End Screen")]
    public GameObject favoriteVideoEndScreen;
    public GameObject favoriteScreenDownloadOption;
    public GameObject favoriteScreenAllOption;

    public static VideoController instance;
    public string currentVideoUrl;
    public string currentIconUrl;
    public string currentName;
    public int currentIndex;

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

    public void PlayVideo(string url, string iconUrl, string name)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
        videoDisplay.SetActive(true);
        APIManager.instance.favouriteVideoUrl = url;
        APIManager.instance.favouriteIconUrl = iconUrl;
        APIManager.instance.favouriteName = name;
        SuperStarAd.Instance.ShowBannerAd();
        UiManager.instance.loadingScreen.SetActive(false);
        if (videoPlayer != null)
        {
            // Register for video end event
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (UiManager.instance.isFavoriteVideo)
        {
            favoriteVideoEndScreen.SetActive(true);
        }
        else
        {
            panelToActivate.SetActive(true); // Show panel when video ends
        }
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
                PlayVideo(currentVideoUrl, currentIconUrl, currentName);
            }, 3);
        }
        else
        {
            PlayVideo(currentVideoUrl, currentIconUrl, currentName);
        }
    }

}