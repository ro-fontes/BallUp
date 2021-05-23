using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public MenuManager menuManager;

    [Header("Login")]
    public GameObject loginPn;
    public InputField playerName;
    string tempPlayerName;

    [Header("Lobby")]
    public GameObject lobbyPn;
    public GameObject noRoomPn;
    public InputField roomName;
    public Slider maxPlayers;
    public Text maxPlayerCount;
    string tempRoomName;

    [Header("Player")]
    public GameObject playerPun;
    public GameObject[] Players;
    public Vector3[] SpawnPlayer;
    public GameObject[] Particles;
    public GameObject particle;
    int SaveSkin;
    int SaveParticle;
    string savedPlayerName;

    [Header("LoadMap")]
    [SerializeField]
    private string selectedMap = "Fase1A";
    public LoadingScreenMultiplayer loadScreen;

    public void loadingScreen(string sceneNo)
    {
        loadScreen.loadingScreen(sceneNo);
        playerPun.GetComponent<Player>().isSinglePlayer = false;
        if(PhotonNetwork.LevelLoadingProgress > 0.8f)
        {
            playerPun.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void Start()
    {
        loadScreen = GetComponent<LoadingScreenMultiplayer>();
        if (!PlayerPrefs.HasKey("namePlayerSaved"))
        {
            loginPn.gameObject.SetActive(true);
        }
        else
        {
            loginPn.gameObject.SetActive(false);
        }

        maxPlayers.value = 1;
        
        PhotonNetwork.AutomaticallySyncScene = true;
        tempPlayerName = "Player" + UnityEngine.Random.Range(8, 99);
        playerName.text = tempPlayerName;
        tempRoomName = "Room" + UnityEngine.Random.Range(8, 99);
        noRoomPn.SetActive(false);
        particle = GameObject.Find("BallParticle");
        savedPlayerName = PlayerPrefs.GetString("namePlayerSaved");
        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
        playerPun = Players[SaveSkin];
    }

    private void Update()
    {
        maxPlayerCount.text = maxPlayers.value + " Player";
    }

    IEnumerator JoinLobby(float time)
    {
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(time);
        PhotonNetwork.JoinLobby();
    }
    IEnumerator GenerateRoom(float time)
    {
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(time);

        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = Convert.ToByte(maxPlayers.value)};

        if (roomName.text == "")
        {
            PhotonNetwork.JoinOrCreateRoom(tempRoomName, roomOptions, TypedLobby.Default);
        }
        else
        {
            tempRoomName = roomName.text;
            PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
        }
    }

    IEnumerator FadeText(float time)
    {
        noRoomPn.SetActive(true);
        yield return new WaitForSeconds(time);
        noRoomPn.SetActive(false);
    }

    public void Login()
    {
        if (playerName.text != "")
        {
            PhotonNetwork.NickName = playerName.text;
            savedPlayerName = playerName.text;
            PlayerPrefs.SetString("namePlayerSaved", savedPlayerName);
        }
        else
        {
            PhotonNetwork.NickName = tempPlayerName;
            savedPlayerName = tempPlayerName;
            PlayerPrefs.SetString("namePlayerSaved", savedPlayerName);
        }

        loginPn.gameObject.SetActive(false);
        
    }

    public void Quicksearch()
    {
        StartCoroutine(JoinLobby(2));
        
    }

    public void CreateRoom()
    {
        StartCoroutine(GenerateRoom(2));
    }

    public override void OnConnected()
    {
        Debug.LogWarning("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogWarning("OnConnectedToMaster");
        Debug.LogWarning("Server: " + PhotonNetwork.CloudRegion);
        Debug.LogWarning("Ping: " + PhotonNetwork.GetPing());
    }

    public override void OnJoinedLobby()
    {
        Debug.LogWarning("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        StartCoroutine(FadeText(5));
        Disconnect();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedRoom()
    {
        Debug.LogWarning("OnJoinedRoom");
        Debug.LogWarning("Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);
        Debug.LogWarning("Quantidade maxima de players da sala: " + PhotonNetwork.CurrentRoom.MaxPlayers);
        Debug.LogWarning("Nome do Player: " + PhotonNetwork.NickName);
        Debug.LogWarning("Players Conectados: " + PhotonNetwork.CurrentRoom.PlayerCount);

        loginPn.gameObject.SetActive(false);
        playerPun.GetComponent<Player>().isSinglePlayer = false;
        loadingScreen(selectedMap);
    }
}
