using UnityEngine;
using Photon.Pun;

public class ArrowLauncher : MonoBehaviourPunCallbacks
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10f;
    public float cooldown = 1f;

    private bool canShoot = true;

    void Update()
    {
        if (photonView.IsMine && canShoot && Input.GetKeyDown("space"))
        {
            photonView.RPC("SpawnArrow", RpcTarget.AllViaServer);
            canShoot = false;
            Invoke("ResetCooldown", cooldown);
        }
    }

    [PunRPC]
    void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        arrowRigidbody.AddForce(arrowSpawnPoint.forward * arrowSpeed, ForceMode.Impulse);
        Destroy(arrow, 3f);
    }

    void ResetCooldown()
    {
        canShoot = true;
    }
}
