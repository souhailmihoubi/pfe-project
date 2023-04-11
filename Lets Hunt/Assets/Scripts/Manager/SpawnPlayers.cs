using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    private GameObject playerPrefab;
    private Animator animator;
    [SerializeField] private Transform[] spawnPoints;

    private int spawnPoint;

    private int characterValue;

    private bool playerSpawned = false;

    public void Start()
    {
        animator = GetComponent<Animator>();

        characterValue = SaveManager.instance.currentHunter;

        spawnPoint = PlayerPrefs.GetInt("spawnPoints",0);

        if(spawnPoint > 4)
        {
            PlayerPrefs.SetInt("spawnPoints", 0);

        }

    }

    public void Update()
    {
        if (PhotonNetwork.InRoom && !playerSpawned)
        {
            AddCharacter(characterValue);

            playerSpawned = true;
        }
    }

    public void AddCharacter(int whichCharacter)
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        if (whichCharacter == 0)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "frog"), spawnPoints[spawnIndex].position, Quaternion.identity); ; ;
        }
        else if (whichCharacter == 1)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "bomber"), spawnPoints[spawnIndex].position, Quaternion.identity);
        }
        else if (whichCharacter == 2)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "archerGirl"), spawnPoints[spawnIndex].position, Quaternion.identity);
        }

        animator = playerPrefab.GetComponent<Animator>();

        //PlayerPrefs.SetInt("spawnPoints", (spawnIndex + 1) % spawnPoints.Length);
    }

}
