using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamControl : MonoBehaviour
{
    CinemachineFreeLook freeLook;
    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        freeLook.Follow = player.transform;
        freeLook.LookAt = player.transform;
    }
}
