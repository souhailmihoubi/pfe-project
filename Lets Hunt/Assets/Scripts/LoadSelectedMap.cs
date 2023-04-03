using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LoadSelectedMap : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private Image mapImage;

    private void Start()
    {
        mapName.text = SaveManager.instance.selectedMapName;

        string spriteDataString = SaveManager.instance.selectedMapSprite;
        byte[] spriteData = System.Convert.FromBase64String(spriteDataString);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(spriteData);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        mapImage.sprite = sprite;

    }
}
