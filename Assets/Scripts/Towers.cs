using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Towers : MonoBehaviour
{
    [Header("PlayerTarget")]
    [SerializeField]
    private GameObject playerTarget;
    public float distance;

    [Header("Tower Config")]

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    [Range(1, 300)]
    private float bulletSpeed;

    [Range(1, 20)]
    public float minDistance;
    [Range(1, 20)]
    public float maxDistance;
    

    void Start()
    {
       
    }

    void Update()
    {
        distance = Vector3.Distance(this.gameObject.transform.position, playerTarget.transform.position);

        if(distance < maxDistance)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player");
            gameObject.transform.rotation = Quaternion.LookRotation(playerTarget.transform.position);
            gameObject.GetComponent<PhotonView>().RPC("Shoot", RpcTarget.All);
        }
        else
        {
            playerTarget = null;
        }
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(bullet);
    }
}
