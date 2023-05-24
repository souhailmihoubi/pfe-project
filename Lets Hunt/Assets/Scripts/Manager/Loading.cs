using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Header("Loading")]
    public float delay = 3f;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        StartCoroutine(LoadingPanel());

    }

    IEnumerator LoadingPanel()
    {
        gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);

    }
}