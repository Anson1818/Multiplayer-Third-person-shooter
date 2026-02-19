using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraSetup : MonoBehaviourPun
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public Transform target;
    public void SetupCamera(CinemachineCamera freeLookCamera)
    {
        if (!photonView.IsMine)
            return;

        freeLookCamera.Follow = transform;
        freeLookCamera.LookAt = target;
    }
}
