using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Coin : MonoBehaviourPunCallbacks
{
    private int value = 1;

    private bool isCollected = false;

    public int coinsCollected;

    private TextMeshProUGUI coins;

    private PhotonView photonView;

    private bool isTransferringOwnership = false;

    private Renderer coinRenderer;

    private Player playerToTransfer;

    private void Start()
    {

        photonView = GetComponent<PhotonView>();

        coinRenderer = GetComponent<Renderer>();

        //photonView.OwnershipTransfer = OwnershipOption.Takeover;

    }

    private void Awake()
    {
        coins = GameObject.FindGameObjectWithTag("coinsCollected").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);

            Sound();
        }

        if (other.gameObject.CompareTag("Coin") && photonView.IsMine)
        {
            value = Random.Range(1, 2);

            coinsCollected += value;

            coins.text = coinsCollected.ToString();

            SaveManager.instance.coins += value;

            SaveManager.instance.Save();

        }
    }

    void Sound()
    {
        //hnee naamel sound effect
    }

    /*
        [PunRPC]
        private void CollectCoin()
        {
            isCollected = true;
            //hnee naamlou l animation w sound effect
        }

         private void OnTriggerEnter(Collider other)
         {
             if (!isCollected && other.CompareTag("Player") && PhotonNetwork.IsConnected)
             {

                 if (photonView.Owner.ActorNumber != other.gameObject.GetPhotonView().Owner.ActorNumber)
                 {

                     playerToTransfer = PhotonNetwork.CurrentRoom.GetPlayer(photonView.Owner.ActorNumber);

                     photonView.TransferOwnership(playerToTransfer);

                     isTransferringOwnership = true;

                     coinRenderer.enabled = false;

                     //print("hided");

                     StartCoroutine(DestroyAfterTransfer());
                 }
                 else
                 {
                     PhotonNetwork.Destroy(gameObject);

                     //print("eat it normal");

                     Sound();

                     photonView.RPC("CollectCoin", RpcTarget.AllBuffered);

                     SaveManager.instance.coins += value;

                     SaveManager.instance.Save();
                 }
             }
         }



        private IEnumerator DestroyAfterTransfer()
        {
            while (isTransferringOwnership)
            {
                yield return new WaitForSeconds(0.2f);
            }
            if (photonView.IsMine)
            {
                //print("transfered");

                PhotonNetwork.Destroy(gameObject);

                Sound();

                photonView.RPC("CollectCoin", RpcTarget.AllBuffered);

                SaveManager.instance.coins += value;

                print(SaveManager.instance.coins);

                SaveManager.instance.Save();
            }
        }


        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {

        }
        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
                //Debug.Log("Ownership of Coin transferred");

                isTransferringOwnership = false;
                print(photonView.Owner.ActorNumber);
        }


        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            //Debug.Log("Ownership transfer of Coin failed");
        }*/


}
