using UnityEngine;
using Photon.Pun;

public class particleManager : MonoBehaviourPunCallbacks
{
    public GameObject Spawn;

    private void Start()
    {
        Spawn = GameObject.FindWithTag("Player");
        this.gameObject.name = "BallParticle";   
    }

    void FixedUpdate()
    {
        if (!Spawn)
        {
            Spawn = GameObject.FindWithTag("Player");
        }

        if (GetComponent<PhotonView>().IsMine && !Spawn.GetComponent<Player>().isSinglePlayer)
        {
            transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
        }
        else
        {

            transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
        }
    }

    [PunRPC]
    public void RPC_ChangeParticleName(int playerID)
    {
        this.gameObject.name = "BallParticle" + playerID;
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
