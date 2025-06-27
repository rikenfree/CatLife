using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        APIManager.instance.SetGridPanel(id);
    }
}
