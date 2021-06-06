using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    public GameObject BallParticle;

    private void Start()
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            //gameObject.tag = "OtherPlayer";
        }

        BallParticle = GameObject.FindWithTag("Particle");
    }

    void Update()
    {
        if (!BallParticle)
        {
            BallParticle = GameObject.FindWithTag("Particle");
        }

        if (!GetComponent<Player>().isSinglePlayer)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                if (GetComponent<Rigidbody>().velocity.magnitude >= 2.5f && GetComponent<Player>().WaterInScene == null && GetComponent<Player>().isFloor)
                {
                    BallParticle.GetComponent<PhotonView>().RPC("PlayParticle", RpcTarget.All);    
                }
                else
                {
                    BallParticle.GetComponent<PhotonView>().RPC("StopParticle", RpcTarget.All);
                }
            }
            else
            {
                //gameObject.tag = "OtherPlayer";
            }
        }
        else
        {
            if (GetComponent<Rigidbody>().velocity.magnitude >= 5f && GetComponent<Player>().WaterInScene == null && GetComponent<Player>().isFloor == true)
            {
                BallParticle.GetComponent<particleManager>().PlayParticle();
            }
            else
            {
                BallParticle.GetComponent<particleManager>().StopParticle();
            }
        }
    }
}
