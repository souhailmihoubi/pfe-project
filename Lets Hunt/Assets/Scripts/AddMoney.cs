using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoney : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveManager.instance.coins += 10;
            SaveManager.instance.Save();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveManager.instance.coins -= 100;
            SaveManager.instance.Save();
        }
    }
}
