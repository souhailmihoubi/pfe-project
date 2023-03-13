using UnityEngine;
using Photon.Pun;
using System;
using Cinemachine;

public class CameraController : MonoBehaviourPun
{
    private PhotonView photonView;

    private void Start()
    {

        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
          
            Camera cam = Camera.main;
            CinemachineVirtualCamera vc = cam.GetComponent<CinemachineVirtualCamera>();
            vc.Follow = transform;
           

        }

    }
}
