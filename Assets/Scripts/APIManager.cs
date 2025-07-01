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
    public string favouriteName;

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
                    BannerVidDisplay[j].transform.GetComponent<StoryScript>().iconUrl = appData.catogary[i].catogaryData[j].iconUrl;
                    BannerVidDisplay[j].transform.GetComponent<StoryScript>().name = appData.catogary[i].catogaryData[j].name;
                    StartCoroutine(DownloadProfilePic(appData.catogary[i].catogaryData[j].iconUrl, BannerVidDisplay[j].transform.GetComponent<ProceduralImage>()));
                }

                GameObject go1 = Instantiate(story, go.GetComponent<CategorySection>().container);
                go1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
                go1.transform.GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
                go1.transform.GetComponent<StoryScript>().iconUrl = appData.catogary[i].catogaryData[j].iconUrl;
                go1.transform.GetComponent<StoryScript>().name = appData.catogary[i].catogaryData[j].name;
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
    public GameObject rowParent;

    GameObject go;

    public GameObject gridPanel;
    public GameObject mainPanel;

    public void SetGridPanel(int i)
    {
        for (int j = gridConteiner.childCount - 1; j >= 0; j--)
        {
            Destroy(gridConteiner.GetChild(j).gameObject);
        }

        //for (int j = 0; j < appData.catogary[i].catogaryData.Count; j++)
        //{
        //    go = Instantiate(rowContainer, gridConteiner);
        //    Transform storyObj = go.transform.GetChild(0);
        //    storyObj.GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
        //    storyObj.GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
        //    storyObj.GetComponent<StoryScript>().name = appData.catogary[i].catogaryData[j].name;
        //    StartCoroutine(DownloadProfilePic(
        //        appData.catogary[i].catogaryData[j].iconUrl,
        //        storyObj.GetComponent<ProceduralImage>())
        //    );
        //}

        GameObject currentParent = null;
        int adInterval = 2;

        for (int j = 0; j < appData.catogary[i].catogaryData.Count; j++)
        {
            // Create new parent every 2 rowContainers (i.e., j % 2 == 0)
            if (j % 2 == 0)
            {
                currentParent = Instantiate(rowParent, gridConteiner);
                currentParent.transform.localScale = Vector3.one;
            }

            // Instantiate rowContainer inside the current parent
            GameObject go = Instantiate(rowContainer, currentParent.transform);

            // Fill data
            Transform storyObj = go.transform.GetChild(0);
            storyObj.GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
            storyObj.GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
            storyObj.GetComponent<StoryScript>().name = appData.catogary[i].catogaryData[j].name;

            StartCoroutine(DownloadProfilePic(
                appData.catogary[i].catogaryData[j].iconUrl,
                storyObj.GetComponent<ProceduralImage>())
            );

            if ((j + 1) % adInterval == 0)
            {
                GameObject adGo = Instantiate(nativeAdPrefab, gridConteiner);
            }
        }
        gridPanel.SetActive(true);
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
    public List<GameObject> favouriteSpawnedObjects = new List<GameObject>();

    public void AddFavouriteData()
    {
        FavouriteData data = new FavouriteData();
        data.videoUrl = favouriteVideoUrl;
        data.iconUrl = favouriteIconUrl;
        data.name = favouriteName;
        FavouriteData existingData = favouriteData.Find(x => x.videoUrl == data.videoUrl);
        if (existingData == null)
        {
            favouriteData.Add(data);
            ShowDataInFavouriteScreen(data.videoUrl, data.iconUrl, data.name);
            Debug.Log("Added to favourites: " + favouriteVideoUrl);
        }
        else
        {
            favouriteData.Remove(existingData);
            Debug.Log("Removed from favourites: " + favouriteVideoUrl);
            // Find the object in the spawned list that matches this video URL
            GameObject targetToRemove = favouriteSpawnedObjects.Find(obj =>
                obj != null && obj.GetComponent<StoryScript>().url == favouriteVideoUrl);
            if (targetToRemove != null)
            {
                Destroy(targetToRemove);
                favouriteSpawnedObjects.Remove(targetToRemove);
                Debug.Log("Destroyed UI GameObject for: " + favouriteVideoUrl);
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
                ShowDataInFavouriteScreen(favouriteData[i].videoUrl, favouriteData[i].iconUrl, favouriteData[i].name);
            }
        }
    }

    public void ShowDataInFavouriteScreen(string data, string iconUrl, string name)
    {
        GameObject videoObject = Instantiate(story, favouriteScreenContent.transform);
        videoObject.transform.GetComponent<StoryScript>().url = data;
        videoObject.transform.GetComponent<StoryScript>().iconUrl = iconUrl;
        videoObject.transform.GetComponent<StoryScript>().name = name;
        StartCoroutine(DownloadProfilePic(iconUrl, videoObject.GetComponent<ProceduralImage>()));
        videoObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        // Set a unique name so we can destroy it later
        favouriteSpawnedObjects.Add(videoObject);
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
    public string iconUrl;
    public string videoUrl;
    public string name;
}