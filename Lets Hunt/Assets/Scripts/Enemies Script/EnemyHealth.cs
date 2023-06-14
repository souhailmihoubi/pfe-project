using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviourPunCallbacks, IPunObservable
{
    public float startingHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthFill;

    [SerializeField] private float updateSpeedSeconds = 0.5f;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject xpPrefab;

    public float currentHealth;
    public bool enemyDead = false;
    private Animator coinAnimator;
    private Animator xpAnimator;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        coinAnimator = GetComponent<Animator>();
        xpAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
        UpdateUI();
    }

    public void TakeDamage(float damageAmount, PlayerItem attacker)
    {
        if (enemyDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        if (currentHealth <= 0f)
        {
            enemyDead = true;

            photonView.RPC("DestroyEnemy", RpcTarget.AllBuffered);
            photonView.RPC("SpawnCoinsXP", RpcTarget.MasterClient);
        }

        photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, currentHealth);
    }

    [PunRPC]
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    private void SpawnCoinsXP()
    {
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * 1f;
        Vector3 coinPos = transform.position + new Vector3(randomPos.x, 0.3f, randomPos.z);
        Vector3 xpPos = new Vector3(transform.position.x, 0.3f, transform.position.z);

        PhotonNetwork.Instantiate(coinPrefab.name, coinPos, Quaternion.identity);
        PhotonNetwork.Instantiate(xpPrefab.name, xpPos, Quaternion.identity);
    }

    private void UpdateUI()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        if (healthText != null)
        {
            healthText.text = Mathf.FloorToInt(currentHealth).ToString();
        }

        if (healthFill != null)
        {
            StartCoroutine(ChangeToPct(currentHealth / startingHealth));
        }
    }

    private IEnumerator ChangeToPct(float pct)
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
}
