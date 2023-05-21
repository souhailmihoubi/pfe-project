using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Security.Policy;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    public int currentHunter;

    public int archer = 0;
    public int kaboom = 0;
    public int swordToad = 0;
    public int matchPlayed = 0;

    public int ranked = 0;

    public int owned = 1;

    public int currentMap;
    public string selectedMapName;
    public string selectedMapSprite;
    public string selectedAvatarSprite;
    public string displayName;
    public int coins;
    public int gems;
    public int thunders;

    public bool changed = false;
    public bool avatarChanged = false;
    public bool[] huntersUnlocked = new bool[3] { true, true, false };



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
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);

            currentHunter = data.currentHunter;
            currentMap = data.currentMap;
            selectedMapName = data.selectedMapName;
            selectedMapSprite = data.selectedMapSprite;
            selectedAvatarSprite = data.selectedAvatarSprite;
            coins = data.coins;
            gems = data.gems;
            thunders = data.thunders;
            huntersUnlocked = data.huntersUnlocked;
            displayName = data.displayName;
            archer = data.archer;
            kaboom = data.kaboom;
            swordToad = data.swordToad;
            matchPlayed = data.matchPlayed;
            ranked = data.ranked;
            owned = data.owned;

            if (data.huntersUnlocked == null)
            {
                huntersUnlocked = new bool[3] { true, true, false };
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
        data.selectedAvatarSprite = selectedAvatarSprite;
        data.coins = coins;
        data.gems = gems;
        data.thunders = thunders;
        data.huntersUnlocked = huntersUnlocked;
        data.displayName = displayName;
        data.archer = archer;
        data.kaboom = kaboom;
        data.owned = owned;
        data.ranked = ranked;
        data.matchPlayed = matchPlayed;
        data.swordToad = swordToad;


        changed = true;

        avatarChanged = true;

        UnityEngine.Debug.Log(changed + " Saved!");

        bf.Serialize(file, data);
        file.Close();


    }
    public String MostPlayed()
    {
        int[] favourite = { kaboom, swordToad, archer };



        if (favourite[0] == favourite.Max())
        {
            return "Kaboom";
        }
        else if (favourite[1] == favourite.Max())
        {
            return "SwordToad";
        }
        else if (favourite[2] == favourite.Max())
        {
            return "Archer";
        }
        else
        {
            return "Unknown";
        }
    }





}

[Serializable]
class PlayerData_Storage
{
    public int currentHunter;
    public int currentMap;
    public string selectedMapName;
    public string selectedMapSprite;
    public string selectedAvatarSprite;
    public string displayName;
    public int coins;
    public int gems;
    public int thunders;
    public bool[] huntersUnlocked;
    public int archer = 0;
    public int kaboom = 0;
    public int swordToad = 0;
    public int matchPlayed = 0;

    public int ranked = 0;

    public int owned = 1;




}