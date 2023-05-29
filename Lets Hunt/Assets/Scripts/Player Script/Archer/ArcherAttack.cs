    using Photon.Pun;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    public class ArcherAttack : MonoBehaviour
    {
        public float attackRange = 2.0f;
        public float attackDamage = 10.0f;
        public LayerMask enemyLayer;

        [SerializeField] private AnimatorController _animatorController;
        protected FixedJoystick joystick;
        public float attackCooldown = 2f;
        private float lastAttackTime = 0f;

        PlayerMove playerMove;

        //------Rotation Variables------
        public float speed = 1f;
        private Coroutine LookCoroutine;

        public bool isAttacking = false;

        PhotonView view;
        PlayerItem playerItem;


        [Header("Arrow Attack Variabels")]
        public bool preformArrowAttack = true;
        public GameObject arrowPrefab;
        public Transform arrowSpawnPoint;
        EnemyHealth closestEnemy;





        private void Start()
        {
            view = GetComponent<PhotonView>();
            playerItem = GetComponent<PlayerItem>();
        }
        private void Awake()
        {
            playerMove = GetComponent<PlayerMove>();
            joystick = FindObjectOfType<FixedJoystick>();
        }

        void Update()
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

                float closestDistance = float.MaxValue;

                closestEnemy = null;

                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        if (playerMove.isMoving == false)
                        {
                            Vector3 direction = (hitCollider.transform.position - transform.position).normalized;

                            StartRotating();

                             //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                            if (distance < closestDistance)
                            {
                                closestEnemy = hitCollider.GetComponent<EnemyHealth>();

                                closestDistance = distance;
                            }
                        }

                    }
                }

                // Attack the closest enemy

                if (closestEnemy != null)
                {
                    if (Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
                    {
                        if (preformArrowAttack)
                        {
                            StartCoroutine(RangedAttackInterval());
                        }
                    }

               
                }

                // Reset attack timer
                lastAttackTime = Time.time;
            }
        }

        IEnumerator RangedAttackInterval()
        {
            preformArrowAttack = false;

            if (closestEnemy != null && !closestEnemy.enemyDead)
            {
                _animatorController.PlayAttack();

                yield return new WaitForSeconds(attackCooldown / ((100 + attackCooldown) * 0.01f));

                if (closestEnemy.enemyDead)
                {
                    playerItem.GetKill();
                }
            }

            _animatorController.StopAttack();
            preformArrowAttack = true;
        }

        public void RangedAttack()
        {
            if (closestEnemy != null)
            {
                Debug.Log("Attack!");
                SpawnRangedProj("Enemy", closestEnemy);
            }

            preformArrowAttack = true;
        }

    void SpawnRangedProj(string typeOfEnemy, EnemyHealth targetedEnemyObj)
    {

        if (typeOfEnemy == "Enemy" && targetedEnemyObj != null)
        {
            ArrowLauncher arrowLauncher = arrowPrefab.GetComponent<ArrowLauncher>();

            arrowLauncher.damage = attackDamage;

            arrowLauncher.targetType = typeOfEnemy;

            arrowLauncher.target = targetedEnemyObj;

            arrowLauncher.targetSet = true;

            Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, Quaternion.identity);
        }
    }
        public void StartRotating()
        {
            if (LookCoroutine != null)
            {
                StopCoroutine(LookCoroutine);
            }

            LookCoroutine = StartCoroutine(look());
        }

        private IEnumerator look()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
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
                Vector3 direction = closestEnemy.position - transform.position;
                direction.y = 0f; // zero out the Y-component to only rotate on the Y-axis
                transform.LookAt(transform.position + direction);

                yield return null;
            }
        }

        void OnDrawGizmosSelected()
        {
            // Draw attack range gizmo in scene view
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        public void IncreaseDamage(float amount)
        {
            attackDamage += amount;

            if (view.IsMine)
            {
                view.RPC("UpdateDamage", RpcTarget.AllBuffered, attackDamage);
            }
        }

        [PunRPC]
        private void UpdateDamage(float newDamage)
        {
            attackDamage = newDamage;
        }

        public void IncreaseAtkSpeed()
        {
            attackCooldown = attackCooldown * 0.75f;
            if (view.IsMine)
            {
                view.RPC("UpdateAtkSpeed", RpcTarget.AllBuffered, attackCooldown);
            }
        }

        [PunRPC]
        private void UpdateAtkSpeed(float newSpeed)
        {
            attackCooldown = newSpeed;
        }


    }