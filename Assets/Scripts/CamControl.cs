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
            player = GameObject.Find("Player" + PlayerPrefs.GetInt("Skin"));
        }
        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }
}
