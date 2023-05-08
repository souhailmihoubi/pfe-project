/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviourPunCallbacks
{
    public float startingHealth;
    public float currentHealth;
    public Animator coinAnimator;
    public Animator xpAnimator;

    // The coin prefab to spawn
    public GameObject coinPrefab;

    public GameObject xpPrefab;


    // The number of coins to spawn
    //public int numCoinsToSpawn = 3;

    void Start()
    {
        coinAnimator = GetComponent<Animator>();

        xpAnimator = GetComponent<Animator>();


        currentHealth = startingHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        Debug.Log(currentHealth.ToString());

        if (currentHealth <= 0f)
        {
                Vector3 randomPos0 = UnityEngine.Random.insideUnitSphere * 1f;

                Vector3 randomPos = new Vector3(randomPos0.x, 0, randomPos0.z);

                Vector3 coinPos = transform.position + randomPos;

                Vector3 xpPos = transform.position;


                GameObject coin = PhotonNetwork.Instantiate(coinPrefab.name, coinPos, Quaternion.identity);

                GameObject xp = PhotonNetwork.Instantiate(xpPrefab.name, xpPos, Quaternion.identity);

                coinAnimator = coinPrefab.GetComponent<Animator>();

                xpAnimator = xpPrefab.GetComponent<Animator>();

                PhotonNetwork.Destroy(gameObject);
        }
    }
}*/
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth;
    public float currentHealth;

    public TextMeshProUGUI healthText;
    public Image healthFill;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    public Animator coinAnimator;
    public Animator xpAnimator;

    // The coin prefab to spawn
    public GameObject coinPrefab;

    public GameObject xpPrefab;

    PhotonView photonView;

    public bool enemyDead = false;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        coinAnimator = GetComponent<Animator>();
        xpAnimator = GetComponent<Animator>();

        currentHealth = startingHealth;

        UpdateUI();
    }

    public void TakeDamage(float damageAmount)
    {
       
        currentHealth -= damageAmount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;

            enemyDead = true;

            photonView.RPC("DestroyEnemy", RpcTarget.AllBuffered);
            photonView.RPC("SpawnCoinsXP", RpcTarget.MasterClient);
        }

        UpdateUI();

        photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, currentHealth);

    }

   

    [PunRPC]
    private void DestroyEnemy()
    {
        Destroy(gameObject);
        //SpawnCoinsXP();
    }


    [PunRPC]
    public void SpawnCoinsXP()
    {
        if (enemyDead)
        {
            return;
        }

        enemyDead = true;

        Vector3 randomPos0 = UnityEngine.Random.insideUnitSphere * 1f;

        Vector3 randomPos = new Vector3(randomPos0.x, 0.3f, randomPos0.z);

        Vector3 coinPos = transform.position + randomPos;

        Vector3 xpPos = new Vector3(transform.position.x, 0.3f, transform.position.z);


        GameObject coin = PhotonNetwork.Instantiate(coinPrefab.name, coinPos, Quaternion.identity);

        GameObject xp = PhotonNetwork.Instantiate(xpPrefab.name, xpPos, Quaternion.identity);

        coinAnimator = coinPrefab.GetComponent<Animator>();

        xpAnimator = xpPrefab.GetComponent<Animator>();
    }


    public void UpdateUI()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        if (healthText != null)
        {
            healthText.text = $"{Mathf.FloorToInt(currentHealth)}";
        }

        if (healthFill != null)
        {
            StartCoroutine(ChangeToPct(currentHealth / startingHealth));
        }
    }

    //----------FOR SMOOTH ANIMATION-----------

    public IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = healthFill.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthFill.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        healthFill.fillAmount = pct;
    }

    [PunRPC]
    private void UpdateHealth(float newHealth, PhotonMessageInfo info)
    {
        currentHealth = newHealth;

        UpdateUI();

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            UpdateUI();
        }
    }
}
