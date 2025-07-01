using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class APIManager : MonoBehaviour
{
    public AppData appData = new AppData();

    string appDataUrl = "https://velocitytechnosoft.com/SocialVideoManagement/fetch_data.php?page=1&limit=10";

    public GameObject NoInternet;

    public static APIManager instance;

    public List<FavouriteData> favouriteData = new List<FavouriteData>();
    public string favouriteIconUrl;
    public string favouriteVideoUrl;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        LoadFavouritesFromPrefs();
        GetAllData();
    }

    public void SetupJson()
    {
        string jsonData = JsonUtility.ToJson(appData);

        Debug.Log("JsonData : " + jsonData);
    }

    void Update()
    {

    }

    public void GetAllData()
    {
        StartCoroutine(IEGetAllData(appDataUrl));
    }

    IEnumerator IEGetAllData(string URL)
    {
        if (IsInternetReachable())
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.result);

                    Debug.Log(request.downloadHandler.text);

                    appData = JsonConvert.DeserializeObject<AppData>(request.downloadHandler.text);

                    SetUp();
                }
                else
                {
                    Debug.LogError(request.error);
                }
            }
        }
    }

    public GameObject catSection;
    public Transform catParent;

    public GameObject story;

    public GameObject[] BannerVidDisplay;

    public GameObject nativeAdPrefab;

    public void SetUp()
    {
        int adInterval = 1; // for example, after every 2 catSections
        for (int i = 0; i < appData.catogary.Count; i++)
        {
            GameObject go = Instantiate(catSection, catParent);

            go.GetComponent<CategorySection>().titleTxt.text = appData.catogary[i].name;
            go.GetComponent<CategorySection>().id = i;

            for (int j = 0; j < appData.catogary[i].catogaryData.Count; j++)
            {
                if (i == 0 && j < 5)
                {
                    BannerVidDisplay[j].transform.GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
                    StartCoroutine(DownloadProfilePic(appData.catogary[i].catogaryData[j].iconUrl, BannerVidDisplay[j].transform.GetComponent<ProceduralImage>()));
                }

                GameObject go1 = Instantiate(story, go.GetComponent<CategorySection>().container);
                go1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
                go1.transform.GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
                StartCoroutine(DownloadProfilePic(appData.catogary[i].catogaryData[j].iconUrl, go1.GetComponent<ProceduralImage>()));
            }

            if ((i + 1) % adInterval == 0)
            {
                GameObject adGo = Instantiate(nativeAdPrefab, catParent);
                // ... setup adGo ...
            }
        }
    }


    public Transform gridConteiner;
    public GameObject rowContainer;

    GameObject go;

    public GameObject gridPanel;
    public GameObject mainPanel;

    public void SetGridPanel(int i)
    {
        // Clear previous children
        for (int j = gridConteiner.childCount; j > 0; j--)
        {
            Destroy(gridConteiner.GetChild(0).gameObject);
        }

        GameObject go = null;
        int adInterval = 1; // 1 = after every rowContainer, 2 = after every 2 rowContainers, etc.
        int rowCount = 0;

        for (int j = 0; j < appData.catogary[i].catogaryData.Count; j++)
        {
            if (j % 2 == 0)
            {
                go = Instantiate(rowContainer, gridConteiner);
                rowCount++;

                // Insert native ad after every rowContainer (or every N rows)
                if (rowCount % adInterval == 0)
                {
                    GameObject adGo = Instantiate(nativeAdPrefab, gridConteiner);
                    // Optionally, set up adGo here
                }
            }

            go.transform.GetChild(j % 2).GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
            go.transform.GetChild(j % 2).GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
            StartCoroutine(DownloadProfilePic(appData.catogary[i].catogaryData[j].iconUrl, go.transform.GetChild(j % 2).GetComponent<ProceduralImage>()));

            gridPanel.SetActive(true);
            //mainPanel.SetActive(false);
        }
    }

    public bool IsInternetReachable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Check internet connection!");
            NoInternet.SetActive(true);
            return false;
        }
        else
        {
            return true;
        }
    }

    public IEnumerator DownloadProfilePic(string url, ProceduralImage image)
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.error == null)
        {
            Texture2D texture = new Texture2D(512, 512);
            www.LoadImageIntoTexture(texture);

            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            Debug.Log("Image Downloaded");

            www.Dispose();
            www = null;
        }
        else
        {
            Debug.Log("Unable to download profile pic");
            Debug.Log("Error : " + www.error);
        }
    }

    public GameObject favouriteScreenContent;

    //public void AddFavouriteData()
    //{
    //    FavouriteData data = new FavouriteData();
    //    data.videoUrl = favouriteVideoUrl;

    //    // Check if already in the list
    //    FavouriteData existingData = favouriteData.Find(x => x.videoUrl == data.videoUrl);

    //    if (existingData == null)
    //    {
    //        favouriteData.Add(data);
    //        ShowDataInFavouriteScreen(data.videoUrl);
    //        Debug.Log("Added to favourites: " + favouriteVideoUrl);
    //    }
    //    else
    //    {
    //        favouriteData.Remove(existingData);
    //        Debug.Log("Removed from favourites: " + favouriteVideoUrl);
    //    }

    //    SaveFavouritesToPrefs();
    //}

    public void AddFavouriteData()
    {
        FavouriteData data = new FavouriteData();
        data.videoUrl = favouriteVideoUrl;

        FavouriteData existingData = favouriteData.Find(x => x.videoUrl == data.videoUrl);

        if (existingData == null)
        {
            favouriteData.Add(data);
            ShowDataInFavouriteScreen(data.videoUrl);
            Debug.Log("Added to favourites: " + favouriteVideoUrl);
        }
        else
        {
            favouriteData.Remove(existingData);
            Debug.Log("Removed from favourites: " + favouriteVideoUrl);

            string objectName = "Fav_" + favouriteVideoUrl;
            Transform favItem = favouriteScreenContent.transform.Find(objectName);

            if (favItem != null)
            {
                Destroy(favItem.gameObject);
                Debug.Log("Destroyed UI object for: " + favouriteVideoUrl);
            }
            else
            {
                Debug.Log("Not Found: " + objectName);
                Debug.Log("Not Found: " + favItem.name);
            }
        }

        SaveFavouritesToPrefs();
    }


    public void SaveFavouritesToPrefs()
    {
        string json = JsonConvert.SerializeObject(favouriteData);
        PlayerPrefs.SetString("favourites", json);
        PlayerPrefs.Save();
        Debug.Log("Favourites saved: " + json);
    }

    public void LoadFavouritesFromPrefs()
    {
        if (PlayerPrefs.HasKey("favourites"))
        {
            string json = PlayerPrefs.GetString("favourites");
            favouriteData = JsonConvert.DeserializeObject<List<FavouriteData>>(json);
            Debug.Log("Favourites loaded: " + json);

            for (int i = 0; i < favouriteData.Count; i++)
            {
                ShowDataInFavouriteScreen(favouriteData[i].videoUrl);
            }
        }
    }

    public void ShowDataInFavouriteScreen(string data)
    {
        GameObject videoObject = Instantiate(story, favouriteScreenContent.transform);
        videoObject.transform.GetComponent<StoryScript>().url = data;

        // Set a unique name so we can destroy it later
        videoObject.name = "Fav_" + data;
    }

}

[System.Serializable]
public class AppData
{
    public string status;
    public int page;
    public int limit;
    public int total_categories;
    public int total_pages;
    public string catogaryName;
    public List<Catogary> catogary;
}

[System.Serializable]
public class Catogary
{
    public int id;
    public string name;
    public string iconUrl;
    public string videoUrl;
    public string created_at;
    public int video_count;
    public List<CatogaryDatum> catogaryData;
}

[System.Serializable]
public class CatogaryDatum
{
    public int id;
    public string title;
    public string description;
    public string iconUrl;
    public string videoUrl;
    public string name;
    public string created_at;
}

[System.Serializable]
public class FavouriteData
{
    // public string iconUrl;
    public string videoUrl;
}