using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
public class VideoDownloder : MonoBehaviour
{
    [Header("Set this to your video URL")]
    public string videoUrl = "https://velocitytechnosoft.com/SocialVideoManagement/uploads/videos/6855294308378_1750411587.mp4";
    [Header("Local file name")]
    public string videoFileName = "downloadedVideo.mp4";
    private string savePath;
    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, videoFileName);
        Debug.Log("Save path: " + savePath);
    }
    // Call this to start download
    public void StartDownload()
    {
        StartCoroutine(DownloadVideo());
    }
    private IEnumerator DownloadVideo()
    {
        Debug.Log("Starting download from: " + videoUrl);
        using (UnityWebRequest request = UnityWebRequest.Get(videoUrl))
        {
            request.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
            while (!request.isDone)
            {
                Debug.Log($"Downloading: {Mathf.RoundToInt(request.downloadProgress * 100)}%");
                yield return null;
            }
#else
            yield return request.SendWebRequest();
#endif
            if (request.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllBytes(savePath, request.downloadHandler.data);
                Debug.Log(":white_check_mark: Video downloaded successfully!");
                Debug.Log("Video saved at: " + savePath);
                // Optional: Call a function or play the video after download
                OnDownloadComplete();
            }
            else
            {
                Debug.LogError(":x: Download failed: " + request.error);
            }
        }
    }
    private void OnDownloadComplete()
    {
        // Optional action after download
        Debug.Log("Download completed! Ready to play.");
    }
    // This returns the full path to access the video later (for playback)
    public string GetVideoPath()
    {
        return savePath;
    }
}