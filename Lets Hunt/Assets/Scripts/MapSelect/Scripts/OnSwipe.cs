using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GG.Infrastructure.Utils.Swipe;

public class OnSwipe : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private GameObject[] maps;
    [SerializeField] private Button select;

    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private Image mapImage;

    [SerializeField] private string selectedMapName;
    [SerializeField] private Sprite selectedMapSprite;

    private int currentMap;

    private void OnEnable()
    {
        swipeListener.OnSwipe.AddListener(OnSwipee);
    }
    private void Start()
    {
        currentMap = SaveManager.instance.currentMap;

        select.onClick.AddListener(() => SaveSelectedMap());

    }

    private void OnSwipee(string swipe)
    {
        if (swipe == "Left")
        {
            currentMap++;

            if (currentMap >= maps.Length)
            {
                currentMap = 0;
            }
        }
        else if (swipe == "Right")
        {
            currentMap--;

            if (currentMap < 0)
            {
                currentMap = maps.Length - 1;
            }
        }

        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(i == currentMap);
        }
    }

    void SaveSelectedMap()
    {
        SaveManager.instance.currentMap = currentMap;

        SaveManager.instance.selectedMapName = selectedMapName;

        byte[] spriteData = selectedMapSprite.texture.EncodeToPNG();
        string spriteDataString = System.Convert.ToBase64String(spriteData);
        SaveManager.instance.selectedMapSprite = spriteDataString;

        SaveManager.instance.Save();
    }

    public void OnMapSelect()
    {
        selectedMapName = maps[currentMap].GetComponentInChildren<TextMeshProUGUI>().text;
        selectedMapSprite = maps[currentMap].GetComponentInChildren<Image>().sprite;
        mapName.text = selectedMapName;
        mapImage.sprite = selectedMapSprite;
    }

    private void OnDisable()
    {
        swipeListener?.OnSwipe.RemoveListener(OnSwipee);
    }
}
