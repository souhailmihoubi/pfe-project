using UnityEngine;
using Photon.Pun;

public class ArrowLauncher : MonoBehaviourPunCallbacks
{
    public float damage;
    public float range;
    public EnemyHealth target;

    public bool targetSet;
    public string targetType;
    public float velocity = 5f;

    public bool stopProjectile;

    public bool getKill = false;

    private void Update()
    {
        if (target)
        {
            if(target == null)
            {
                Destroy(gameObject);
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, velocity * Time.deltaTime);

            if(!stopProjectile)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= 0.5f)
                {
                    if(targetType == "Enemy")
                    {
                        target.GetComponent<EnemyHealth>().TakeDamage(damage);
                        stopProjectile = true;
                        Destroy(gameObject) ;
                    }

                }
            }
        }
    }
}

