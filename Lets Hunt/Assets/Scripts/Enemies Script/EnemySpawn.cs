using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class EnemySpawn : MonoBehaviourPun
{
    public GameObject[] level1Enemies;
    public GameObject[] level2Enemies;
    public GameObject[] level3Enemies;
    public GameObject warningPrefab;

    public float[] topRight;
    public float[] topLeft;
    public float[] botRight;
    public float[] botLeft;

    public float warningDuration = 3f;

    private float minX, minZ, maxX, maxZ;

    Vector3 playerPosition;

    private bool playerEnteredRange = false;


    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        

        foreach (GameObject player in players)
        {

             playerPosition = player.transform.position;

             photonView.RPC("UpdatePosition", RpcTarget.AllBuffered, playerPosition);

             // print (playerPosition);

             if (CheckIfPlayerInRange(playerPosition, topRight) || CheckIfPlayerInRange(playerPosition, topLeft) ||
                 CheckIfPlayerInRange(playerPosition, botRight) || CheckIfPlayerInRange(playerPosition, botLeft))
             {
                 if (!playerEnteredRange)
                 {
                     playerEnteredRange = true;
                     StartCoroutine(SpawnEnemiesWithDelay());
                     Debug.Log("aw d5al");
                 }
             }
             else
             {
                 if (playerEnteredRange)
                 {
                     playerEnteredRange = false;
                     StopCoroutine(SpawnEnemiesWithDelay());
                     Debug.Log("aw 5raj");
                 }
             }
        }
    }

    IEnumerator SpawnEnemiesWithDelay()
    {
        while (playerEnteredRange)
        {
            yield return new WaitForSeconds(5);

            SpawnRandomEnemy();
        }
    }

    void SpawnRandomEnemy()
    {
        GameObject warning = null;

        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(0, level1Enemies.Length);

            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

            warning = PhotonNetwork.Instantiate(warningPrefab.name, randomPosition, Quaternion.identity);

            StartCoroutine(DestroyWarningPrefab(warning, 3f, level1Enemies[randomIndex], randomPosition));
        }
    }

    bool CheckIfPlayerInRange(Vector3 playerPosition, float[] spotName)
    {
        minX = spotName[0];
        maxX = spotName[1];
        minZ = spotName[2];
        maxZ = spotName[3];

        if (playerPosition.x >= minX && playerPosition.x <= maxX && playerPosition.z >= minZ && playerPosition.z <= maxZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator DestroyWarningPrefab(GameObject warningPrefab, float delay, GameObject enemyPrefab, Vector3 randomPosition)
    {
        yield return new WaitForSeconds(delay);

        PhotonNetwork.Instantiate(enemyPrefab.name, randomPosition, Quaternion.identity);
        PhotonNetwork.Destroy(warningPrefab);
    }

    [PunRPC]
    void UpdatePosition(Vector3 newPosition)
    {
        playerPosition = newPosition;
    }
}