using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.IO;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] level1Enemies;
    public GameObject[] level2Enemies;
    public GameObject[] level3Enemies;

    public GameObject warningPrefab;

    private int currentEnemyLevel = 1;

    public float warningDuration = 3f;

    private float spawnTimer = 0.0f;

    public float minX, minZ, maxX, maxZ;

    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 60f)
        {
            currentEnemyLevel++;

            spawnTimer = 0f;

            if (currentEnemyLevel == 3)
                currentEnemyLevel = 1;
        }

        SpawnEnemies();
    }


    private int enemyCounter = 0;

    void SpawnEnemies()
    {
        if (enemyCounter < 12 && spawnTimer >= 10f)
        {
            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

            if (PhotonNetwork.IsMasterClient)
            {
                GameObject warning = null;
                GameObject enemyPrefab = null;

                switch (currentEnemyLevel)
                {
                    case 1:
                        enemyPrefab = level1Enemies[Random.Range(0, level1Enemies.Length)];

                        warning = PhotonNetwork.Instantiate(warningPrefab.name, randomPosition, Quaternion.identity);

                        StartCoroutine(DestroyWarningPrefab(warning, warningDuration, enemyPrefab, randomPosition));

                        break;
                    case 2:
                        enemyPrefab = level2Enemies[Random.Range(0, level2Enemies.Length)];

                        warning = PhotonNetwork.Instantiate(warningPrefab.name, randomPosition, Quaternion.identity);

                        StartCoroutine(DestroyWarningPrefab(warning, warningDuration, enemyPrefab, randomPosition));

                        break;
                    case 3:
                        enemyPrefab = level3Enemies[Random.Range(0, level3Enemies.Length)];

                        warning = PhotonNetwork.Instantiate(warningPrefab.name, randomPosition, Quaternion.identity);

                        StartCoroutine(DestroyWarningPrefab(warning, warningDuration, enemyPrefab, randomPosition));

                        break;
                }

                enemyCounter++;
                spawnTimer = 0f;
            }
        }
    }

    IEnumerator DestroyWarningPrefab(GameObject warningPrefab, float delay, GameObject enemyPrefab, Vector3 randomPosition)
    {

        yield return new WaitForSeconds(delay);
        PhotonNetwork.Instantiate(enemyPrefab.name, randomPosition, Quaternion.identity);
        PhotonNetwork.Destroy(warningPrefab);


    }


}