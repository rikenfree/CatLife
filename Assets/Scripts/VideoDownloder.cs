using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VideoDownloader : MonoBehaviour
{
    [Header("Single video URL")]
    public string videoUrl = "https://velocitytechnosoft.com/SocialVideoManagement/uploads/videos/6855294308378_1750411587.mp4";

    private string savePath;

    void Start()
    {
        Debug.Log("🟢 Ready to download from: " + videoUrl);
    }

    public void OpenDownloadScreen()
    {
        // Open Download Option Screen..
        VideoController.instance.allOptionObj.SetActive(false);
        VideoController.instance.OpenDownloadOption(true);
    }

    public void OpenDownloadScreenFavoriteVideo()
    {
        // Open Download Option Screen..
        VideoController.instance.favoriteScreenAllOption.SetActive(false);
        VideoController.instance.favoriteScreenDownloadOption.SetActive(true);
    }

    // Call to download as HD
    public void DownloadHD()
    {
        string fileName = "MyAwesomeVideo_HD.mp4";
        savePath = Path.Combine(Application.persistentDataPath, fileName);
        StartCoroutine(DownloadVideo(savePath));
    }

    // Call to download as 4K
    public void Download4K()
    {
        string fileName = "MyAwesomeVideo_4K.mp4";
        savePath = Path.Combine(Application.persistentDataPath, fileName);
        StartCoroutine(DownloadVideo(savePath));
    }

    private IEnumerator DownloadVideo(string pathToSave)
    {
        Debug.Log("📥 Starting download from: " + videoUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(videoUrl))
        {
            request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            while (!request.isDone)
            {
                // Debug.Log($"⏳ Downloading... {Mathf.RoundToInt(request.downloadProgress * 100)}%");
                yield return null;
            }
#else
            yield return request.SendWebRequest();
#endif

            if (request.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllBytes(pathToSave, request.downloadHandler.data);
                Debug.Log("✅ Download complete!");
                Debug.Log("📁 Saved to: " + pathToSave);

                OnDownloadComplete(pathToSave);
            }
            else
            {
                Debug.LogError("❌ Download failed: " + request.error);
            }
        }
    }

    private void OnDownloadComplete(string finalPath)
    {
        Debug.Log("🎉 File saved successfully! Ready at: " + finalPath);
        VideoController.instance.OpenDownloadOption(false);
        // You can trigger playback or notification here
    }

    // Optional: Get last used video path
    public string GetLastSavedVideoPath()
    {
        return savePath;
    }
}
