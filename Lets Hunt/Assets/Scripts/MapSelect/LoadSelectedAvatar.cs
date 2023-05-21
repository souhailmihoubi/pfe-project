using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSelectedAvatar : MonoBehaviour
{
    [SerializeField] private Image avatarImage;

    SaveManager saveManager;


    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("PlayFabManger").GetComponent<SaveManager>();

        ChangeAvatar();
    }

    private void Update()
    {

        if(saveManager.avatarChanged)
        {

            ChangeAvatar();

            saveManager.avatarChanged = false;
        }
    }

    void ChangeAvatar()
    {
        string spriteDataString = SaveManager.instance.selectedAvatarSprite;

        byte[] spriteData = System.Convert.FromBase64String(spriteDataString);

        Texture2D texture = new Texture2D(1, 1);

        texture.LoadImage(spriteData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        avatarImage.sprite = sprite;

        Debug.Log("Avatar Loaded");
    }
}
