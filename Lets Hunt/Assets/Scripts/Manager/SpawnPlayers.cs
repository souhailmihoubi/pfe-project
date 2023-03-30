using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    private GameObject playerPrefab;
    private Animator animator;

    public float minX, minY, maxX, maxY;

    Vector3 randomPosition;

    private int characterValue;

    private bool playerSpawned = false;

    public void Start()
    {
        animator = GetComponent<Animator>();

        randomPosition = new Vector3(Random.Range(minX, maxX), 11.486f, Random.Range(minY, maxY));

        characterValue = PlayerPrefs.GetInt("SelectedCharacterIndex");
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
        print("spawn");

        if (whichCharacter == 0)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "frog"), randomPosition, Quaternion.identity);
        }
        else if (whichCharacter == 1)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "bomber"), randomPosition, Quaternion.identity);
        }
        else if (whichCharacter == 2)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "archerGirl"), randomPosition, Quaternion.identity);
        }

        animator = playerPrefab.GetComponent<Animator>();
    }
}






/*using Photon.Pun;
using System.IO;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    private GameObject playerPrefab;
    private Animator animator;

    public float minX, minY, maxX, maxY;

    Vector3 randomPosition;

    private int characterValue;


    public void Start()
    {
        animator = GetComponent<Animator>();

        randomPosition = new Vector3(Random.Range(minX, maxX), 11.486f, Random.Range(minY, maxY));

        characterValue = PlayerPrefs.GetInt("SelectedCharacterIndex");

        AddCharacter(characterValue);

    }

    public void AddCharacter(int whichCharacter)
    {

        if (whichCharacter == 0)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "frog"), randomPosition, Quaternion.identity);
        }
        else if (whichCharacter == 1)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "bomber"), randomPosition, Quaternion.identity);
        }
        else if (whichCharacter == 2)
        {
            playerPrefab = PhotonNetwork.Instantiate(Path.Combine("", "archerGirl"), randomPosition, Quaternion.identity);
        }

        //playerPrefab.transform.parent = transform;

        animator = playerPrefab.GetComponent<Animator>();

    }
}*/
