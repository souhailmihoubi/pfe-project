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


    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
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

    private void Update()
    {
        if(photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(10);
            }
        }
    }

    private void Die()
    {
        // handle player death here
    }

    private void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}";
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




/*using System;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public event Action<float> OnHealthPctChange = delegate { };

    SetupHealthBar text;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        text = GetComponent<SetupHealthBar>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;

        float currentHealthPct = (float) currentHealth / (float) maxHealth;

        OnHealthPctChange(currentHealthPct);
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ModifyHealth(-10);
            }
            //  Debug.Log(currentHealth);
            text.healthTxt.SetText(currentHealth.ToString());
        }
       
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else if(stream.IsReading)
        {
            currentHealth = (int) stream.ReceiveNext();
        }
    }


}*/
