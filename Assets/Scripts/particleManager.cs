using UnityEngine;
using Photon.Pun;

public class particleManager : MonoBehaviourPunCallbacks
{
    public GameObject Spawn;
    public int myActorNumber;

    private void Start()
    {
        if (!gameObject.GetComponent<PhotonView>().IsMine && !Spawn.GetComponent<Player>().isSinglePlayer)
        {
            gameObject.tag = "OtherParticle";
            //GetComponent<PhotonView>().RPC("RPC_ChangeParticleName", RpcTarget.AllBufferedViaServer); 
        }
        
        Spawn = GameObject.FindWithTag("Player");
        //GetComponent<PhotonView>().RPC("RPC_ChangeParticleName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.ActorNumber); 
        this.gameObject.name = "BallParticle";
    }
    
    void FixedUpdate()
    {
        if (!Spawn)
        {
            Spawn = GameObject.FindWithTag("Player");
        }

        if (!Spawn.GetComponent<Player>().isSinglePlayer)
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
            }
            else
            {
                gameObject.tag = "OtherParticle";
                //GetComponent<PhotonView>().RPC("RPC_ChangeParticleName", RpcTarget.AllBufferedViaServer); 
            }
        }
        else
        {
            transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
        }
    }

    [PunRPC]
    public void RPC_ChangeParticleName()
    {
        //this.gameObject.name = "BallParticle" + playerID;
    }

    [PunRPC]
    public void PlayParticle()
    {
        GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    public void StopParticle()
    {
        GetComponent<ParticleSystem>().Stop();
    }
}
