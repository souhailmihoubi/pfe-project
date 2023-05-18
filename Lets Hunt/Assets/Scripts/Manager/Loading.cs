using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public GameObject loadingPanel;

    private void Start()
    {
        StartCoroutine(close());
    }

    IEnumerator close()
    {
        yield return new WaitForSeconds(3f);
        loadingPanel.SetActive(false);
    }
}
