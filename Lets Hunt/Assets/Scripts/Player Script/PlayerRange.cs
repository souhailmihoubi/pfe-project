using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRange : MonoBehaviour
{
    PhotonView photonView;

    GameObject playerRangeIndicator;

    public Vector3 currentRange;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if( photonView != null  && photonView.IsMine)
        {
            playerRangeIndicator = GameObject.Find("rangeIndicator");

            currentRange = playerRangeIndicator.transform.localScale;
        }

    }

    public void ChangeRange(float x , float y)
    {
        
        Vector3 newRange  = currentRange + new Vector3(x, y, 0);
    
        currentRange = newRange;

        UpdateUI();

        photonView.RPC("UpdateRange", RpcTarget.OthersBuffered, currentRange);

    }

    public void UpdateUI()
    {
        playerRangeIndicator.transform.localScale = currentRange;
    }

    [PunRPC]
    private void UpdateRange(Vector3 newRange)
    {
        currentRange = newRange;

        UpdateUI();

    }
}
