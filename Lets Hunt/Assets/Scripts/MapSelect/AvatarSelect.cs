using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelect : MonoBehaviour
{
    public void OnImageClick()
    {
        Sprite selectedSprite = GetComponent<Image>().sprite;

        Texture2D texture = selectedSprite.texture;
        Rect rect = selectedSprite.rect;
        Vector2 pivot = selectedSprite.pivot;
        Texture2D newTexture = new Texture2D((int)rect.width, (int)rect.height);
        Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        byte[] spriteData = newTexture.EncodeToPNG();

        string spriteDataString = System.Convert.ToBase64String(spriteData);

        SaveManager.instance.selectedAvatarSprite = spriteDataString;

        SaveManager.instance.Save();

        Debug.Log("Avatar changed!");
    }


}
