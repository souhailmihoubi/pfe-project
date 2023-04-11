using Photon.Pun;
using UnityEngine;

public class GrenadeScript : MonoBehaviourPun, IPunObservable
{
    public GameObject parentBone;
    public Rigidbody rigid;

    private bool isReleased = false;

    void Start()
    {
        if (photonView.IsMine)
        {
            transform.parent = parentBone.transform;
            rigid.useGravity = false;
        }
    }

    public void ReleaseMe()
    {
        transform.parent = null;
        rigid.useGravity = true;
        transform.rotation = parentBone.transform.rotation;
        rigid.AddForce(transform.forward * 2000);

        isReleased = true;

        // Notify other clients that the grenade has been released
        //photonView.RPC("OnGrenadeReleased", RpcTarget.Others);
    }

    [PunRPC]
    private void OnGrenadeReleased()
    {
        // Update the state of the grenade on other clients
        isReleased = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send the state of the grenade to other clients
            stream.SendNext(isReleased);
        }
        else
        {
            // Receive the state of the grenade from the owner client
            isReleased = (bool)stream.ReceiveNext();
        }
    }
}
