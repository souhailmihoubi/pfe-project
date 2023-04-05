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

        if (whichCharacter == 0)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "frog"), spawnPoints[spawnPoint].position , Quaternion.identity); ; ;
        }
        else if (whichCharacter == 1)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "bomber"), spawnPoints[spawnPoint].position, Quaternion.identity);
        }
        else if (whichCharacter == 2)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "archerGirl"), spawnPoints[spawnPoint].position, Quaternion.identity);
        }

        animator = playerPrefab.GetComponent<Animator>();

        PlayerPrefs.SetInt("spawnPoints", spawnPoint++);


    }
}
