using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    private RectTransform rectComponent;
    private Image imageComp;

    public float speed = 200f;
    public Text text;
    public Text textNormal;

    // Use this for initialization
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        int a = 0;
        if (imageComp.fillAmount < 0.99f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            a = (int)(imageComp.fillAmount * 100);
            if (a > 0 && a <= 50)
            {
                textNormal.text = "Loading...";
            }
            else if (a > 50 && a <= 100)
            {
                textNormal.text = "Please wait...";
            }
            text.text = a + "%";
        }
        else
        {
            Debug.Log("Complete");
            VideoController.instance.PlayURLVideo();
            imageComp.fillAmount = 0.0f;
            text.text = "0%";
        }
    }
}