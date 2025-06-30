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

    public GameObject[] nativeAdPrefabs;

    public void SetUp()
    {
        StartCoroutine(IESetUpWithNativeAds());

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
        }
    }

    IEnumerator IESetUpWithNativeAds()
    {
        for (int i = 0; i < appData.catogary.Count; i++)
        {
            // Create cat section
            GameObject go = Instantiate(catSection, catParent);
            go.GetComponent<CategorySection>().titleTxt.text = appData.catogary[i].name;
            go.GetComponent<CategorySection>().id = i;
            // Wait a frame to ensure the cat section is properly initialized
            yield return null;
            // Load stories for this category
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
            // Add delay between cat sections
            yield return new WaitForSeconds(0.1f);
            // Clone native ad prefab after each cat section (except the last one)
            if (i < appData.catogary.Count - 1)
            {
                GameObject selectedNativeAdPrefab = GetNativeAdPrefab();
                if (selectedNativeAdPrefab != null)
                {
                    GameObject nativeAd = Instantiate(selectedNativeAdPrefab, catParent);
                    // Wait for native ad to initialize
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }

    private GameObject GetNativeAdPrefab()
    {
        // If you have an array of prefabs, randomly select one
        if (nativeAdPrefabs != null && nativeAdPrefabs.Length > 0)
        {
            return nativeAdPrefabs[Random.Range(0, nativeAdPrefabs.Length)];
        }
        // Otherwise use the single prefab
        return nativeAdPrefab;
    }

    public Transform gridConteiner;
    public GameObject rowContainer;

    GameObject go;

    public GameObject gridPanel;
    public GameObject mainPanel;

    // public void SetGridPanel(int i)
    // {
    //     //go.GetComponent<CategorySection>().titleTxt.text = appData.catogary[i].name;

    //     for (int j = gridConteiner.childCount; j > 0; j--)
    //     {
    //         Destroy(gridConteiner.GetChild(0).gameObject);
    //     }

    //     for (int j = 0; j < appData.catogary[i].catogaryData.Count; j++)
    //     {
    //         // if (j % 2 == 0)
    //         // {
    //         //     go = Instantiate(rowContainer, gridConteiner);
    //         // }
    //         go = Instantiate(rowContainer, gridConteiner);
    //         go.transform.GetChild(j % 2).GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
    //         go.transform.GetChild(j % 2).GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
    //         StartCoroutine(DownloadProfilePic(appData.catogary[i].catogaryData[j].iconUrl, go.transform.GetChild(j % 2).GetComponent<ProceduralImage>()));

    //         gridPanel.SetActive(true);
    //         //mainPanel.SetActive(false);
    //     }
    // }

    public void SetGridPanel(int i)
    {
        // Clear old children
        for (int j = gridConteiner.childCount - 1; j >= 0; j--)
        {
            Destroy(gridConteiner.GetChild(j).gameObject);
        }

        // Loop through all stories in the selected category
        for (int j = 0; j < appData.catogary[i].catogaryData.Count; j++)
        {
            // Always instantiate a new rowContainer for each story
            go = Instantiate(rowContainer, gridConteiner);

            // Access the only child in this rowContainer (since now it only holds 1 story)
            Transform storyObj = go.transform.GetChild(0); // Assuming the story is the first (and only) child

            // Set text and data
            storyObj.GetChild(0).GetComponent<TextMeshProUGUI>().text = appData.catogary[i].catogaryData[j].name;
            storyObj.GetComponent<StoryScript>().url = appData.catogary[i].catogaryData[j].videoUrl;
            

            // Download and apply image
            StartCoroutine(DownloadProfilePic(
                appData.catogary[i].catogaryData[j].iconUrl,
                storyObj.GetComponent<ProceduralImage>())
            );
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
    private List<GameObject> favouriteSpawnedObjects = new List<GameObject>();

    public void AddFavouriteData()
    {
        FavouriteData data = new FavouriteData();
        data.videoUrl = favouriteVideoUrl;

        FavouriteData existingData = favouriteData.Find(x => x.videoUrl == data.videoUrl);

        if (existingData == null)
        {
            favouriteData.Add(data);
            ShowDataInFavouriteScreen(data.videoUrl, data.iconUrl);
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
                ShowDataInFavouriteScreen(favouriteData[i].videoUrl, favouriteData[i].iconUrl);
            }
        }
    }

    public void ShowDataInFavouriteScreen(string videoUrl, string iconUrl)
    {
        GameObject videoObject = Instantiate(story, favouriteScreenContent.transform);
        videoObject.transform.GetComponent<StoryScript>().url = videoUrl;

        // Add to spawned object list
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
}