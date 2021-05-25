using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class Network : MonoBehaviourPunCallbacks
{
    public GameObject Cam;
    public Vector3[] SpawnPlayer;
    public GameObject[] Particles;
<<<<<<< HEAD
    int i = 0;
    int SaveSkin, SaveParticle;
    float savedColorR, savedColorG, savedColorB;
    float R, G, B;
    GameObject player, particle;
=======

    private int i = 0;
    private int x = 0;
    private int SaveSkin, SaveParticle;
    private float savedColorR, savedColorG, savedColorB;
    private float R, G, B;
    private GameObject player, particle;
>>>>>>> parent of 246c85b (O resto)

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
<<<<<<< HEAD
        //Spawn();
=======
        x++;
>>>>>>> parent of 246c85b (O resto)
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        
    }
<<<<<<< HEAD

    private void Update()
    {

=======
    

    private void Update()
    {
>>>>>>> parent of 246c85b (O resto)
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

<<<<<<< HEAD
        player.name = "Player" + SaveSkin;
=======
        player.name = "Player" + x;
>>>>>>> parent of 246c85b (O resto)
        player.GetComponent<MeshRenderer>().material.color = new Color(savedColorR, savedColorG, savedColorB, 255f);

        R = player.GetComponent<MeshRenderer>().material.color.r;
        G = player.GetComponent<MeshRenderer>().material.color.g;
        B = player.GetComponent<MeshRenderer>().material.color.b;
        player.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, R, G, B);

        particle = Instantiate(Particles[SaveParticle], player.transform.position, player.transform.rotation);
        particle.name = "BallParticle";
    }
}

