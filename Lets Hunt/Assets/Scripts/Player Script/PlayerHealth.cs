using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public TextMeshProUGUI healthText;
    public Image healthFill;

    private bool isHealing = false;
    private Coroutine healingCoroutine = null;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(10);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }

        UpdateUI();
        photonView.RPC("UpdateHealth", RpcTarget.Others, currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heal"))
        {
            isHealing = true;

            if (healingCoroutine == null)
            {
                healingCoroutine = StartCoroutine(HealingCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Heal"))
        {
            isHealing = false;

            if (healingCoroutine != null)
            {
                StopCoroutine(healingCoroutine);
                healingCoroutine = null;
            }
        }
    }

    private IEnumerator HealingCoroutine()
    {
        while (isHealing)
        {
            currentHealth += 10f;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UpdateUI();

            yield return new WaitForSeconds(1f);
        }
    }

    private void Die()
    {
        // Kif ymout
        Debug.Log("Die");
    }

    private void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{Mathf.FloorToInt(currentHealth)}";
        }

        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
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
}
