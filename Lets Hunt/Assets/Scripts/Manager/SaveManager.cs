using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    public int currentHunter;
    public int currentMap;
    public string selectedMapName;
    public string selectedMapSprite;
    public int coins;
    public int gems;
    public int thunders;
    public int playerID;

    public bool[] huntersUnlocked = new bool[3] { true,true,false };
    


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);

            currentHunter = data.currentHunter;
            currentMap = data.currentMap;
            selectedMapName = data.selectedMapName;
            selectedMapSprite = data.selectedMapSprite;
            coins = data.coins;
            gems = data.gems;
            thunders = data.thunders;
            huntersUnlocked = data.huntersUnlocked;
            playerID = data.playerID;

            if(data.huntersUnlocked == null)
            {
                huntersUnlocked = new bool[3] {true,true,false };
            }

            file.Close();



        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData_Storage data = new PlayerData_Storage();

        data.currentHunter = currentHunter;
        data.currentMap = currentMap;
        data.selectedMapName = selectedMapName;
        data.selectedMapSprite = selectedMapSprite;
        data.coins = coins;
        data.gems = gems;
        data.thunders = thunders;
        data.huntersUnlocked = huntersUnlocked;
        data.playerID = playerID;

        bf.Serialize(file, data);
        file.Close();

        
    }



}

[Serializable]
class PlayerData_Storage
{
    public int currentHunter;
    public int currentMap;
    public string selectedMapName;
    public string selectedMapSprite;
    public int coins;
    public int gems;
    public int thunders;
    public bool[] huntersUnlocked;
    public int playerID;


}
