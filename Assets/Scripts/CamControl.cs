using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class CamControl : MonoBehaviourPunCallbacks
{
    int x = 0;
    CinemachineFreeLook freeLook;
    public GameObject player;
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        print("entrou joinedRoom");
        x++;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        x--;
    }

    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.Find("Player" + x);
        }
        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }
}
