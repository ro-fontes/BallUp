using UnityEngine;
using Cinemachine;

public class CamControl : MonoBehaviour
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
            player = GameObject.FindGameObjectWithTag("Player");
        }
        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }
}
