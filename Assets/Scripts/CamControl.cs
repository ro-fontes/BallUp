using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class CamControl : MonoBehaviourPunCallbacks
{
    CinemachineFreeLook freeLook;
    public GameObject player;

    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }
}
