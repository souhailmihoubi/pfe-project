using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.IO;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] level1Enemies;
    public GameObject[] level2Enemies;
    public GameObject[] level3Enemies;

    public float spawnInterval = 3f;
    public float level1Duration = 60.0f;
    public float level2Duration = 60.0f;
    public float level3Duration = 60.0f;

    private float spawnTimer = 0.0f;
    private float levelTimer = 0.0f;
    private int level = 0;

    public float minX, minZ, maxX, maxZ;

    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        levelTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0.0f;

            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

            GameObject enemyPrefab = null;
            switch (level)
            {
                case 0:
                    enemyPrefab = level1Enemies[Random.Range(0, level1Enemies.Length)];
                    PhotonNetwork.Instantiate(Path.Combine("Enemies1", enemyPrefab.name), randomPosition, Quaternion.identity);

                    break;
                case 1:
                    enemyPrefab = level2Enemies[Random.Range(0, level2Enemies.Length)];
                    PhotonNetwork.Instantiate(Path.Combine("Enemies2", enemyPrefab.name), randomPosition, Quaternion.identity);

                    break;
                case 2:
                    enemyPrefab = level3Enemies[Random.Range(0, level3Enemies.Length)];
                    PhotonNetwork.Instantiate(Path.Combine("Enemies3", enemyPrefab.name), randomPosition, Quaternion.identity);

                    break;
            }


        }

        if (levelTimer >= level1Duration && level == 0)
        {
            levelTimer = 0.0f;
            level = 1;
            photonView.RPC("ChangeLevel", RpcTarget.All, level);
        }
        else if (levelTimer >= level2Duration && level == 1)
        {
            levelTimer = 0.0f;
            level = 2;

            photonView.RPC("ChangeLevel", RpcTarget.All, level);
        }
        else if (levelTimer >= level3Duration && level == 2)
        {
            levelTimer = 0.0f;

            photonView.RPC("DisableScript", RpcTarget.All);// disable this script on all players

            enabled = false;  // disable this script on the local client
        }
    }

    [PunRPC]
    void ChangeLevel(int newLevel)
    {
        level = newLevel;
    }

    [PunRPC]
    void DisableScript()
    {
        enabled = false;
    }


    /*  void Update()
      {
          // update timers
          spawnTimer += Time.deltaTime;
          levelTimer += Time.deltaTime;

          // check if it's time to spawn an enemy
          if (spawnTimer >= spawnInterval)
          {
              spawnTimer = 0.0f;

              GameObject enemyPrefab = null;
              switch (level)
              {
                  case 0:
                      enemyPrefab = level1Enemies[Random.Range(0, level1Enemies.Length)];
                      break;
                  case 1:
                      enemyPrefab = level2Enemies[Random.Range(0, level2Enemies.Length)];
                      break;
                  case 2:
                      enemyPrefab = level3Enemies[Random.Range(0, level3Enemies.Length)];
                      break;
              }

              Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 11.486f, Random.Range(minZ, maxZ));

              Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
          }

          if (levelTimer >= level1Duration && level == 0)
          {
              levelTimer = 0.0f;
              level = 1;
          }
          else if (levelTimer >= level2Duration && level == 1)
          {
              levelTimer = 0.0f;
              level = 2;
          }
          else if (levelTimer >= level3Duration && level == 2)
          {
              levelTimer = 0.0f;

              enabled = false;  // disable this script
          }
      }*/
}
