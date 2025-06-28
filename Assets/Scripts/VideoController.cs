using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
   public VideoPlayer videoPlayer;
    public GameObject videoDisplay;
    public GameObject panelToActivate;
    public GameObject downloadOptions;
    public GameObject allOptionObj;

    public static VideoController instance;

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

}