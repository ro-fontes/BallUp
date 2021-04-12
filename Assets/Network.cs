using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{
    public GameObject Cam;
    public Vector3[] SpawnPlayer;
    int i = 0;
    int SaveSkin;
    int SaveParticle;
    GameObject player;

    private void Start()
    {
        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != i && !player)
        {
            Spawn();
            i++;
        }
    }

    [PunRPC]
    public void Spawn()
    {    
        Instantiate(Cam, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        player = PhotonNetwork.Instantiate("Player" + SaveSkin, SpawnPlayer[0], new Quaternion(0,0,0,0));
        player.name = "Player" + SaveSkin;
    }
}

