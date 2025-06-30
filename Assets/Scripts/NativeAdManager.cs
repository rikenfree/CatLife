using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using TMPro;
using System.Collections.Generic;

public class NativeAdManager : MonoBehaviour
{
    public bool test;
    public string NativeAdanceID;
    private NativeAd nativeAd;
    [SerializeField] RawImage adIcon;
    [SerializeField] RawImage BodyImage;
    [SerializeField] RawImage adChoices;
    [SerializeField] TextMeshProUGUI adHeadline;
    [SerializeField] TextMeshProUGUI adCallToAction;
    [SerializeField] TextMeshProUGUI adAdvertiser;
    private bool nativeAdLoaded;
    #region Instance
    public static NativeAdManager Instance;
    private readonly object bodyText;
    void Awake()
    {
        Instance = this;
    }
    #endregion
    #region CoreFunctions
    void Start()
    {
        if (test)
        {
            NativeAdanceID = "ca-app-pub-3940256099942544/2247696110";
        }
        Requestnative();
    }
    private void OnEnable()
    {
        //if (test)
        //{
        //    NativeAdanceID = "ca-app-pub-3940256099942544/2247696110";
        //}
        //Requestnative();
    }
    void Requestnative()
    {
        AdLoader adLoader = new AdLoader.Builder(NativeAdanceID)
        .ForNativeAd()
        .Build();
        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        adLoader.LoadAd(new AdRequest());
    }
    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        Debug.Log("Native ad loaded.");
        this.nativeAd = args.nativeAd;
        nativeAdLoaded = true;
    }
    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Debug.Log("Native ad failed to load: " + args.Message);
    }
    private void Update()
    {
        if (this.nativeAdLoaded)
        {
            this.nativeAdLoaded = false;
            DisplayAd(nativeAd);
        }
    }
    AdRequest AdRequestBuild()
    {
        return new AdRequest();
    }
    void DisplayAd(NativeAd ad)
    {
        Texture2D iconTexture = this.nativeAd.GetIconTexture();
        Texture2D iconAdChoices = this.nativeAd.GetAdChoicesLogoTexture();
        if (this.nativeAd.GetImageTextures().Count > 0)
        {
            List<Texture2D> goList = this.nativeAd.GetImageTextures();
            BodyImage.texture = goList[0];
            List<GameObject> list = new List<GameObject>();
            list.Add(BodyImage.gameObject);
            this.nativeAd.RegisterImageGameObjects(list);
        }
        string headline = this.nativeAd.GetHeadlineText();
        string cta = this.nativeAd.GetCallToActionText();
        string advertiser = this.nativeAd.GetAdvertiserText();
        adIcon.texture = iconTexture;
        adChoices.texture = iconAdChoices;
        adHeadline.text = headline;
        adAdvertiser.text = advertiser;
        adCallToAction.text = cta;
        //register gameobjects
        nativeAd.RegisterIconImageGameObject(adIcon.gameObject);
        nativeAd.RegisterAdChoicesLogoGameObject(adChoices.gameObject);
        nativeAd.RegisterHeadlineTextGameObject(adHeadline.gameObject);
        nativeAd.RegisterCallToActionGameObject(adCallToAction.gameObject);
        nativeAd.RegisterAdvertiserTextGameObject(adAdvertiser.gameObject);
    }
    void DestroyNative()
    {
        nativeAd.Destroy();
    }
    #endregion
    public void ShowNativeAd()
    {
        Requestnative();
    }
    public void HideNativeAd()
    {
        DestroyNative();
    }
}

