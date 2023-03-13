using System;
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


}
