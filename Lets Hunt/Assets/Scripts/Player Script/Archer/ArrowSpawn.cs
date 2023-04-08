using GG.Infrastructure.Utils.Swipe;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawn : MonoBehaviour
{
    public GameObject arrowPrefab;
    public bool triggered = false;
    public Transform arrowSpawnPosition;

    Vector3 _direction;

    public float arrowSpeed = 20f;


    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void SetTarget(Transform enemy)
    {
        Debug.Log("taget set");

         _direction = (enemy.position - transform.position).normalized;

        transform.LookAt(enemy);
    }

    public void SpawnArrow()
    {

            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPosition.position, arrowSpawnPosition.rotation);

            Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

            arrowRigidbody.AddForce(_direction * arrowSpeed, ForceMode.VelocityChange);


            //Destroy(gameObject, 4f);

           /* if (view.IsMine)
            {
                view.RPC("SpawnArrowRPC", RpcTarget.Others, arrowSpawnPosition.position, arrowSpawnPosition.rotation, enemy.position);
            }*/
    }

    [PunRPC]
    private void SpawnArrowRPC(Vector3 position, Quaternion rotation, Vector3 enemyPosition)
    {
        if (arrowPrefab != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, rotation);
            arrow.transform.LookAt(enemyPosition);
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
