using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawn : MonoBehaviour
{
    public GameObject arrowPrefab;
    public bool triggered = false; 
    public Transform arrowSpawnPosition;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void SpawnArrow(Transform enemy)
    {
        if (arrowPrefab != null && arrowSpawnPosition != null && enemy != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPosition.position, arrowSpawnPosition.rotation);

            arrow.transform.LookAt(enemy);

            Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

            if (arrowRigidbody != null)
            {
                arrowRigidbody.AddForce(arrow.transform.forward * 500f);


            }
            if (view.IsMine)
            {
                view.RPC("SpawnArrowRPC", RpcTarget.Others, arrowSpawnPosition.position, arrowSpawnPosition.rotation, enemy.position);
            }
           
        }
    }

    [PunRPC]
    private void SpawnArrowRPC(Vector3 position, Quaternion rotation, Vector3 enemyPosition)
    {
        if (arrowPrefab != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, rotation);
            arrow.transform.LookAt(enemyPosition);
            Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
            if (arrowRigidbody != null)
            {
                arrowRigidbody.AddForce(arrow.transform.forward * 500f);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            triggered = true;
            //Destroy(gameObject);
        }
    }
}
