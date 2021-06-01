using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    GameObject BallParticle;


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
        }
        else
        {
            if (GetComponent<Rigidbody>().velocity.magnitude >= 5f && GetComponent<Player>().WaterInScene == null && GetComponent<Player>().isFloor == true)
            {
                //PlayParticle();
                BallParticle.GetComponent<particleManager>().PlayParticle();
            }
            else
            {
                //StopParticle();
                BallParticle.GetComponent<particleManager>().StopParticle();
            }
        }
    }
}
