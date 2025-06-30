using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using MoreMountains.NiceVibrations;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LoadingPanel, ScrollPanel, NextButton, PlayButton, MainBottomPanel;
    [SerializeField] ProceduralImage StoriesImage, FavoriteImage, ScrollImage, BasicImage, StandardImage, PremiumImage;
    [SerializeField] TextMeshProUGUI Loadingtext;
    float loadingtime;
    bool isLoading = true,isHaptic;
    [SerializeField] Slider LoadingSlider,HapticSlider;
    [SerializeField] SimpleScrollSnap scrollSnap;

    [SerializeField] Sprite[] panelSprites, SubscriptionSprites;

    private void Awake()
    {
        scrollSnap.OnPanelCentered.AddListener(OnPanelCentered);
    }

    private void OnDestroy()
    {
        scrollSnap.OnPanelCentered.RemoveListener(OnPanelCentered);
    }

    private void OnPanelCentered(int centeredPanel, int previousPanel)
    {
        int lastPanelIndex = scrollSnap.NumberOfPanels - 1;

        // Change the sprite according to the centered panel
        if (panelSprites != null && panelSprites.Length > centeredPanel)
        {
            ScrollImage.sprite = panelSprites[centeredPanel];
        }

        if (centeredPanel == lastPanelIndex)
        {
            NextButton.SetActive(false);
            PlayButton.SetActive(true);
        }
        else
        {
            NextButton.SetActive(true);
            PlayButton.SetActive(false);
        }
    }
    private void Start()
    {
        if (HapticSlider.value>0)
        {
            isHaptic = true;
        }
        else
        {
            isHaptic = false;
        }
    }

    private void Update()
    {
        if (isLoading)
        {
            loadingtime += Time.deltaTime * 20;
            Loadingtext.text = loadingtime.ToString("0") + "%";
            LoadingSlider.value = loadingtime;
            if (loadingtime >= 100f)
            {
                isLoading = false;
                LoadingPanel.SetActive(false);
                ScrollPanel.SetActive(true);
            }
        }
        if (HapticSlider.value > 0)
        {
            isHaptic = true;
        }
        else
        {
            isHaptic = false;
        }
    }

    public void OnStoriesButtonClick()
    {
        Color Storiescolor = StoriesImage.color;
        Storiescolor.a = 1;
        StoriesImage.color = Storiescolor;

        Color FavoriteColor = FavoriteImage.color;
        FavoriteColor.a = 0;
        FavoriteImage.color = FavoriteColor;
    }

    public void OnFavoriteButtonClick()
    {
        Color Storiescolor = StoriesImage.color;
        Storiescolor.a = 0;
        StoriesImage.color = Storiescolor;

        Color FavoriteColor = FavoriteImage.color;
        FavoriteColor.a = 1;
        FavoriteImage.color = FavoriteColor;
    }
    public void OnBasicButtonClick()
    {
        BasicImage.sprite = SubscriptionSprites[1];
        StandardImage.sprite = SubscriptionSprites[0];
        PremiumImage.sprite = SubscriptionSprites[0];
    }
    public void OnStandardButtonClick()
    {
        BasicImage.sprite = SubscriptionSprites[0];
        StandardImage.sprite = SubscriptionSprites[1];
        PremiumImage.sprite = SubscriptionSprites[0];
    }
    public void OnPremiumButtonClick()
    {
        BasicImage.sprite = SubscriptionSprites[0];
        StandardImage.sprite = SubscriptionSprites[0];
        PremiumImage.sprite = SubscriptionSprites[1];
    }

    public void BottomPanel()
    {
        if (MainBottomPanel.activeSelf)
        {
            MainBottomPanel.SetActive(false);
        }
        else
        {
            MainBottomPanel.SetActive(true);
        }
    }
    public void Vibration()
    {
        if (isHaptic)
            MMVibrationManager.Haptic(HapticTypes.RigidImpact);
    }
}
