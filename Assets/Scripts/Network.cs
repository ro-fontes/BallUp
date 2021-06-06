using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Network : MonoBehaviourPunCallbacks
{
    [Header("Map Config")]
    public ParticleSystem[] mapParticles;
    [Header("Camera Prefab")]
    public GameObject Cam;
    [Header("Spawn Players")]
    public Vector3[] SpawnPlayer;
    public GameObject[] playerParticles;

    private int myPlayerId;
    private int savedSkin;
    private int savedParticle;

    [Header("UI")]
    public Text playerJoinText;
    public GameObject waitingPlayerPanel;

    private int x = 0;
    private int i = 0;
    private float savedColorR, savedColorG, savedColorB;
    private float R, G, B;
    private GameObject player, particle;

    private void Start()
    {
        //Pegando as cores do player
        savedSkin = PlayerPrefs.GetInt("Skin");
        savedParticle = PlayerPrefs.GetInt("Particle");
        savedColorR = PlayerPrefs.GetFloat("Color");
        savedColorG = PlayerPrefs.GetFloat("Color1");
        savedColorB = PlayerPrefs.GetFloat("Color2");
        Spawn();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        playerJoinText.text = newPlayer.NickName + " Joined the Room";
        StartCoroutine(ActiveJoinText(5));
        x++;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        x--;
    }
    
    IEnumerator ActiveJoinText(float time)
    {
        playerJoinText.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        playerJoinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        playerJoinText.gameObject.SetActive(true);
        playerJoinText.text = "Be faster than your opponent";
        yield return new WaitForSeconds(time);
        playerJoinText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            GameObject.Find("Player" + myPlayerId).GetComponent<Player>().enabled = true;
            waitingPlayerPanel.SetActive(false);
        }
        else
        {
            GameObject.Find("Player" + myPlayerId).GetComponent<Player>().enabled = false;
            waitingPlayerPanel.SetActive(true);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount != i && !player)
        {
            i++;
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount < i)
        {
            i--;
        }
    }

    void Spawn()
    {
        myPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
        Instantiate(Cam);
        particle = PhotonNetwork.Instantiate(playerParticles[savedParticle].name, new Vector3(0, 0, 0), Quaternion.identity);
        player = PhotonNetwork.Instantiate("Player" + savedSkin, SpawnPlayer[myPlayerId - 1], Quaternion.identity);
        
        

        particle.GetComponent<particleManager>().myActorNumber = myPlayerId;
        player.GetComponent<Player>().myActorNumber = myPlayerId;
        player.GetComponent<Player>().isSinglePlayer = false;

        player.GetComponent<MeshRenderer>().material.color = new Color(savedColorR, savedColorG, savedColorB, 255f);

        R = player.GetComponent<MeshRenderer>().material.color.r;
        G = player.GetComponent<MeshRenderer>().material.color.g;
        B = player.GetComponent<MeshRenderer>().material.color.b;
        player.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBufferedViaServer, R, G, B);
        if(mapParticles.Length > 0)
        {
            mapParticles[myPlayerId - 1].Play();
        }  
    }
}

