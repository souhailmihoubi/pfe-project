using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class ArrowLauncher : MonoBehaviourPunCallbacks
{
    public float damage;
    public float range;
    public EnemyHealth target;

    public bool targetSet;
    public string targetType;
    public float velocity = 5f;

    public bool stopProjectile;

    PlayerItem playerItem;

    public PlayerItem shooter;

    //---------- SFX

    [SerializeField] AudioSource bomberAudioSource;
    [SerializeField] AudioSource archerAudioSource;

    private void Start()
    {
        playerItem = GetComponent<PlayerItem>();
    }

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
                if (Vector3.Distance(transform.position, target.transform.position) <= 0.3f)
                {

                    if(targetType == "Enemy")
                    {
                        if (target != null)
                        {
                            target.TakeDamage(damage, shooter);

                            if(target.enemyDead)
                            {
                                print("met!");
                                shooter.GetKill();
                            }

                        }

                        if (archerAudioSource != null)
                        {
                            archerAudioSource.Play();
                        }

                        if(bomberAudioSource != null)
                        {
                            bomberAudioSource.Play();
                        }


                        stopProjectile = true;

                    }

                    stopProjectile = true;
                        Destroy(gameObject) ;
                    }

                }
            }
        }
    }


