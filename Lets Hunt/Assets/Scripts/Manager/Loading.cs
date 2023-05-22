using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject loadingPanel;
    public float delay = 3f;

    private void Start()
    {
        Invoke("CloseLoadingPanel", delay);
    }

    private void CloseLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}