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
<<<<<<< HEAD
            player = GameObject.Find("Player" + PlayerPrefs.GetInt("Skin"));
=======
            player = GameObject.FindGameObjectWithTag("Player");
>>>>>>> parent of 246c85b (O resto)
        }
        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }
}
