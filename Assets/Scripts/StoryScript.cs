using UnityEngine;
using UnityEngine.Video;
using SuperStarSdk;

public class StoryScript : MonoBehaviour
{
    public string url;


    public VideoPlayer videoPlayer;

    // public void SetURLAndPlayVideo()
    // {
    //     UiManager.instance.loadingScreen.SetActive(true);
    //     VideoController.instance.currentUrl = url;
    // }

    public void SetURLAndPlayVideo()
    {
        UiManager.instance.loadingScreen.SetActive(true);
        VideoController.instance.currentVideoUrl = url;

        // Going up the hierarchy to get CatSection(Clone)
        Transform catSection = transform.parent.parent.parent.parent; // Adjust levels if needed

        // Get the script attached to CatSection(Clone)
        CategorySection categoryScript = catSection.GetComponent<CategorySection>(); // Replace with actual script name

        if (categoryScript != null)
        {
            Debug.Log("Successfully got the CatSection script!");
            // You can now call methods or access variables in that script
            // e.g., catScript.DoSomething();
        }
        else
        {
            Debug.LogWarning("CatSectionScript not found on parent!");
        }

        VideoController.instance.currentIndex = categoryScript.id; // Your logic here
    }

}
