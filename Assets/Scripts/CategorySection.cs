using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SuperStarSdk;

public class CategorySection : MonoBehaviour
{
    public Transform container;

    public TextMeshProUGUI titleTxt;

    public int id;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenGridPanel()
    {
        if(SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                APIManager.instance.SetGridPanel(id);
            }, 3);
        }
        else
        {
            APIManager.instance.SetGridPanel(id);
        }
    }
}
