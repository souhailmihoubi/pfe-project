using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class PlayerHealth : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxHealth = 100f;

    public float currentHealth;

    public TextMeshProUGUI healthText;
    public Image healthFill;

    private bool isHealing = false;
    private Coroutine healingCoroutine = null;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    public bool playerDead = false;

    [SerializeField] AudioSource healingAudioSource;


    private void Start()
    {

        currentHealth = maxHealth;
        UpdateUI();
        photonView.RPC("UpdateHealth", RpcTarget.Others, currentHealth);
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
            playerDead = true;
            Die();
        }

        UpdateUI();

        photonView.RPC("UpdateHealth", RpcTarget.Others, currentHealth);

    
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
            StartCoroutine(ChangeToPct(currentHealth / maxHealth)); 
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

    //----------HEALING SPOT--------------

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
                isHealing = false;
            }

            if (photonView.IsMine)
            {
                healingAudioSource.Play();
            }

            UpdateUI();

            yield return new WaitForSeconds(2f);
        }
    }



    //--------- After Death ---------------

    private void Die()
    {

        Player player = PhotonNetwork.PlayerList[FindLocalPlayer()];

        photonView.RPC("PlayerFinish", RpcTarget.AllBuffered, player);

        StartCoroutine(OnEndPanel());

    }

    [PunRPC]
    void PlayerFinish(Player player)
    {
        Scoreboard scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();

        scoreboard.PlayerDies(player);
    }


    IEnumerator OnEndPanel()
    {
        PlayerScore playerScore = GetComponent<PlayerScore>();

        playerScore.WinLosePanel();

        yield return new WaitForSeconds(3f);

        PhotonNetwork.Destroy(gameObject);


        playerScore.SetScoreDeadPlayer();
    }


    private int FindLocalPlayer()
    {
        int localPlayerIndex = -1;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                localPlayerIndex = i;
                break;
            }
        }

        return localPlayerIndex;

    }

}
