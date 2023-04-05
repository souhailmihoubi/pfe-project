using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawn : MonoBehaviour
{
    public GameObject grenadePrefab;
    public bool triggered = false;
    public Transform grenadeSpawnPosition;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

 public void SpawnGrenade()
{
    if (grenadePrefab != null && grenadeSpawnPosition != null)
    {
            GameObject grenade = Instantiate(grenadePrefab, grenadeSpawnPosition.position, grenadeSpawnPosition.rotation);
            //grenade.transform.LookAt(enemy);

            Debug.Log("Throw");
            GrenadeScript grenadeScript = grenade.GetComponent<GrenadeScript>();
            grenadeScript.ReleaseMe();

            // Pass the velocity of the grenade to the SpawngrenadeRPC method
            if (view.IsMine)
            {
               
               view.RPC("SpawngrenadeRPC", RpcTarget.Others, grenadeSpawnPosition.position, grenadeSpawnPosition.rotation);
            }
        
    }
}


    [PunRPC]
    private void SpawngrenadeRPC(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        if (grenadePrefab != null && grenadeSpawnPosition != null)
        {
            GameObject grenade = Instantiate(grenadePrefab, position, rotation);
       //     grenade.transform.LookAt(enemyPosition);

    
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            triggered = true;
           // Destroy(gameObject);
        }
    }
}
