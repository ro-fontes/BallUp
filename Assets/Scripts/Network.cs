using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class Network : MonoBehaviourPunCallbacks
{
    [Header("Camera Prefab")]
    public GameObject Cam;
    [Header("Spawn Players")]
    public Vector3[] SpawnPlayer;

    [Header("UI")]

    public GameObject waitingPlayerPanel;

    private int x = 0;
    private int i = 0;
    private float savedColorR, savedColorG, savedColorB;
    private float R, G, B;
    private int savedSkin;
    public GameObject player, particle;

    private void Start()
    {
        //Pegando as cores do player
        savedSkin = PlayerPrefs.GetInt("Skin");
        savedColorR = PlayerPrefs.GetFloat("Color");
        savedColorG = PlayerPrefs.GetFloat("Color1");
        savedColorB = PlayerPrefs.GetFloat("Color2");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        x++;
        Spawn();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        x--;
    }
    private void Update()
    {



        if(PhotonNetwork.CurrentRoom.PlayerCount != i && !player)
        {
            Spawn();
            i++;
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount < i)
        {
            i--;
        }
        if(x > 0)
        {
            player.GetComponent<Player>().enabled = true;

            for (int i = 0; i > 4; i++)
            {
                GameObject.Find("Player" + i).GetComponent<Player>().enabled = true;

            }
            
        }
        else
        {
            player.GetComponent<Player>().enabled = false;
        }
    }

    void Spawn()
    {
        if(x != 0)
        {
            Instantiate(Cam, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            player = PhotonNetwork.Instantiate("Player" + savedSkin, SpawnPlayer[x], new Quaternion(0, 0, 0, 0));
            waitingPlayerPanel.SetActive(false);
        }
        else
        {
            Instantiate(Cam, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            player = PhotonNetwork.Instantiate("Player" + savedSkin, SpawnPlayer[x], new Quaternion(0, 0, 0, 0));
            player.GetComponent<Player>().enabled = false;
            waitingPlayerPanel.SetActive(true);
        }












        player.name = "Player" + x;
        player.GetComponent<MeshRenderer>().material.color = new Color(savedColorR, savedColorG, savedColorB, 255f);

        R = player.GetComponent<MeshRenderer>().material.color.r;
        G = player.GetComponent<MeshRenderer>().material.color.g;
        B = player.GetComponent<MeshRenderer>().material.color.b;
        player.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, R, G, B);

        particle = PhotonNetwork.Instantiate("Fire", player.transform.position, player.transform.rotation);
        particle.name = "BallParticle";
    }
}

