using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviourPunCallbacks
{
    public float startingHealth;
    public float currentHealth;
    public Animator animator;

    // The coin prefab to spawn
    public GameObject coinPrefab;

    // The number of coins to spawn
    public int numCoinsToSpawn = 3;

    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = startingHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(currentHealth.ToString());
        if (currentHealth <= 0f)
        {
            // Spawn coins at the position where the monster died
            for (int i = 0; i < numCoinsToSpawn; i++)
            {
                // Generate a random position within a sphere
                Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 0.5f;

                // Set the coin's position to the enemy's position plus the random position
                Vector3 coinPos = transform.position + randomPos;

                // Spawn the coin at the position
                GameObject coin = PhotonNetwork.Instantiate(coinPrefab.name, coinPos, Quaternion.identity);
                // You can add any additional functionality to the spawned coins here
                animator = coinPrefab.GetComponent<Animator>();

            }

            PhotonNetwork.Destroy(gameObject);
            //Play death animation
        }
    }
}




/*using Photon.Pun;
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

    void Start()
    {
        currentHealth = startingHealth;
        UpdateUI();
    }

    public void TakeDamage(float damageAmount)
    {
       
        currentHealth -= damageAmount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Destroy(gameObject);
            //Play death animation
        }

        UpdateUI();

        //photonView.RPC("UpdateHealth", RpcTarget.Others, currentHealth);

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
    private void UpdateHealth(float newHealth)
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
}*/
