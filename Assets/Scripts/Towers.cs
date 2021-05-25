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
    GameObject blt;
    public GameObject spawnBullet;

    [SerializeField]
    [Range(1, 50)]
    private float bulletLife;

    [SerializeField]
    [Range(1, 300)]
    private float bulletSpeed = 200;

    [Range(1, 20)]
    public float minDistance = 1;
    [Range(1, 1000)]
    public float maxDistance = 100;
    

    void Update()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        distance = Vector3.Distance(this.gameObject.transform.position, playerTarget.transform.position);

        if (distance < maxDistance)
        {
            gameObject.transform.LookAt(playerTarget.transform.position);
            gameObject.GetComponent<PhotonView>().RPC("Shooting", RpcTarget.All);
        }
        else
        {
            playerTarget = null;
        }
    }

    
    IEnumerator Shoot(float time)
    {
        
        if (!blt)
        {
            blt = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation);
        }
        yield return new WaitForSeconds(time);

    }

    [PunRPC]
    void Shooting()
    {
        StartCoroutine(Shoot(bulletLife));
    }
}
