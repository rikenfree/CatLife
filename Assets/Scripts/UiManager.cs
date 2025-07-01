using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using SuperStarSdk;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public GameObject LoadingPanel;
    public GameObject TutorialPanel;
    public GameObject Main_SelectionPanel;

    public Image loadingFillImage; // Assign your Image Fill 1 here

    public Button nextButton;
    public Button getStartedButton;

    // Reference to your ScrollSnap script
    public SimpleScrollSnap scrollSnap;

    public TextMeshProUGUI loadingText;

    public GameObject gridPanel;

    public GameObject downloadOption, loadingScreen;

    public GameObject favouriteVideoScreen;
    public GameObject favoriteScreenContent;
    public TextMeshProUGUI favoriteScreenText;

    public bool isFavoriteVideo;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initial state
        LoadingPanel.SetActive(true);
        TutorialPanel.SetActive(false);
        Main_SelectionPanel.SetActive(false);

        getStartedButton.gameObject.SetActive(false);

        // Subscribe to scroll event
        scrollSnap.OnPanelSelected.AddListener(OnTutorialPageChanged);

        getStartedButton.onClick.AddListener(OnGetStartedClicked);

        StartCoroutine(SimulateLoading());
    }

    void OnTutorialPageChanged(int pageIndex)
    {
        if (pageIndex == scrollSnap.NumberOfPanels - 2)
        {
            if (SuperStarAd.Instance.NoAds == 0)
            {
                nextButton.gameObject.SetActive(false);
                getStartedButton.gameObject.SetActive(true);

            }
            else
            {
                nextButton.gameObject.SetActive(false);
                getStartedButton.gameObject.SetActive(true);
            }
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            getStartedButton.gameObject.SetActive(false);
        }
    }

    void OnGetStartedClicked()
    {
        TutorialPanel.SetActive(false);
        Main_SelectionPanel.SetActive(true);
        // Show banner ad when tutorial is hidden
        SuperStarAd.Instance.ShowBannerAd();
    }

    // Call this when loading is done
    public void ShowTutorialPanel()
    {
        LoadingPanel.SetActive(false);
        TutorialPanel.SetActive(true);
        // Hide banner ad when tutorial is shown
        SuperStarAd.Instance.HideBannerAd();
    }

    public void SetLoadingProgress(float progress)
    {
        // progress should be between 0 and 1
        loadingFillImage.fillAmount = progress;
        int percent = Mathf.RoundToInt(progress * 100f);
        loadingText.text = $"Loading {percent}%";
    }

    IEnumerator LoadYourScene()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("YourSceneName");
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f); // Unity's async progress goes from 0 to 0.9
            SetLoadingProgress(progress);

            if (progress >= 1f)
            {
                asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator SimulateLoading()
    {
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * 0.2f; // Adjust speed as needed
            SetLoadingProgress(progress);
            yield return null;
        }
        SetLoadingProgress(1f); // Ensure it ends at 100%
        ShowTutorialPanel();    // Show the tutorial panel and hide loading panel
    }

    public void StopVideoPlayer()
    {
        VideoController.instance.videoPlayer.Stop();
        APIManager.instance.SetGridPanel(VideoController.instance.currentIndex);
        VideoController.instance.panelToActivate.SetActive(false);
        VideoController.instance.videoDisplay.SetActive(false);
        gridPanel.SetActive(true);
    }

    public void StopFavoriteVideoPlayer()
    {
        VideoController.instance.videoPlayer.Stop();
        // APIManager.instance.SetGridPanel(VideoController.instance.currentIndex);
        FavoriteScreenTextRemove();
        VideoController.instance.favoriteVideoEndScreen.SetActive(false);
        VideoController.instance.videoDisplay.SetActive(false);
        favouriteVideoScreen.SetActive(true);
    }

    public void HomeBtnClick()
    {
        VideoController.instance.videoPlayer.Stop();
        Main_SelectionPanel.SetActive(true);
        gridPanel.SetActive(false);
        VideoController.instance.panelToActivate.SetActive(false);
        VideoController.instance.favoriteVideoEndScreen.SetActive(false);
        VideoController.instance.videoDisplay.SetActive(false);
    }

    public void FavoriteScreenTextRemove()
    {
        if (favoriteScreenContent.transform.childCount > 0)
        {
            favoriteScreenText.transform.gameObject.SetActive(false);
        }
        else
        {
            favoriteScreenText.transform.gameObject.SetActive(true);
        }
    }

    public void FavouriteBtnClick()
    {
        isFavoriteVideo = true;
        favouriteVideoScreen.SetActive(true);
        FavoriteScreenTextRemove();
    }

    public void BackToFavouriteBtnClick()
    {
        isFavoriteVideo = false;
        favouriteVideoScreen.SetActive(false);
        FavoriteScreenTextRemove();
    }

    public void HideGridPanel()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                gridPanel.SetActive(false);
            }, 3);
        }
        else
        {
            gridPanel.SetActive(false);
        }
    }
}
