using UnityEngine;
using Photon.Pun;

public class ArrowLauncher : MonoBehaviourPunCallbacks
{
    public Rigidbody arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10f;
    public float cooldown = 1f;
    PlayerAttack playerAttack;

    private bool canShoot = true;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerAttack.attackRange);
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = hitCollider.transform;
                    closestDistance = distance;
                }
            }
        }

        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.position - arrowSpawnPoint.position;
            Rigidbody arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.LookRotation(direction));
            arrow.AddForce(direction.normalized * arrowSpeed, ForceMode.Impulse);
            Destroy(arrow.gameObject, 3f);
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }
}
