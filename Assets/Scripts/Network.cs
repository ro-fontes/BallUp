using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class Network : MonoBehaviourPunCallbacks
{
    public GameObject Cam;
    public Vector3[] SpawnPlayer;
    public GameObject[] Particles;
    int i = 0;
    int SaveSkin, SaveParticle;
    float savedColorR, savedColorG, savedColorB;
    float R, G, B;
    GameObject player, particle;

    private void Start()
    {
        //Pegando as cores do player
        savedColorR = PlayerPrefs.GetFloat("Color");
        savedColorG = PlayerPrefs.GetFloat("Color1");
        savedColorB = PlayerPrefs.GetFloat("Color2");
        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        print("entrou joinedRoom");
        //Spawn();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        
    }

    private void Update()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount != i && !player)
        {
            Spawn();
            i++;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount < i)
        {
            i--;
        }
    }

    void Spawn()
    {
        Instantiate(Cam, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

        player = PhotonNetwork.Instantiate("Player" + SaveSkin, SpawnPlayer[0], new Quaternion(0, 0, 0, 0));

        player.name = "Player" + SaveSkin;
        player.GetComponent<MeshRenderer>().material.color = new Color(savedColorR, savedColorG, savedColorB, 255f);

        R = player.GetComponent<MeshRenderer>().material.color.r;
        G = player.GetComponent<MeshRenderer>().material.color.g;
        B = player.GetComponent<MeshRenderer>().material.color.b;
        player.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, R, G, B);

        particle = Instantiate(Particles[SaveParticle], player.transform.position, player.transform.rotation);
        particle.name = "BallParticle";
    }
}

