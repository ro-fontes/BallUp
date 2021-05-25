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
    public GameObject playerFbx;
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

    #region LoadingScreen

    [Header("LoadingScreen")]
    [SerializeField]
    private GameObject loadGameobject;
    [SerializeField]
    private GameObject bar;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private bool backGroundImageAndLoop;
    [SerializeField]
    private float LoopTime;
    [SerializeField]
    private GameObject[] backgroundImages;
    [SerializeField]
    [Range(0, 1f)] private float vignetteEffectValue;

    public void loadingScreen(string sceneNo)
    {
        loadGameobject.gameObject.SetActive(true);
        playerFbx.SetActive(false);
        StartCoroutine(WaitATime(3, sceneNo));

    }

    IEnumerator transitionImage()
    {
        for (int i = 0; i < backgroundImages.Length; i++)
        {
            yield return new WaitForSeconds(LoopTime);
            for (int j = 0; j < backgroundImages.Length; j++)
                backgroundImages[j].SetActive(false);
            backgroundImages[i].SetActive(true);
        }
    }

    #endregion

    void Start()
    {
<<<<<<< HEAD
        if (backGroundImageAndLoop)
        {
            StartCoroutine(transitionImage());
        }
=======
        loadScreen = GetComponent<LoadingScreenMultiplayer>();
>>>>>>> parent of 246c85b (O resto)
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
        playerFbx = GameObject.Find("Player");
    }

    IEnumerator WaitATime(float time, string text)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Loading(text));
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

<<<<<<< HEAD
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = Convert.ToByte(maxPlayers.value), PlayerTtl = 20 };
=======
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = Convert.ToByte(maxPlayers.value)};
>>>>>>> parent of 246c85b (O resto)

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
<<<<<<< HEAD
        loadingScreen(selectedMap);
    }
    IEnumerator Loading(string sceneNo)
    {
        PhotonNetwork.LoadLevel(sceneNo);
        PhotonNetwork.AutomaticallySyncScene = true;
        

        // Continue until the installation is completed
        while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            bar.transform.localScale = new Vector3(PhotonNetwork.LevelLoadingProgress, 0.9f, 1);

            if (loadingText != null)
                loadingText.text = "%" + (100 * bar.transform.localScale.x).ToString("####");

            if (PhotonNetwork.LevelLoadingProgress > 0.8f)
            {
                bar.transform.localScale = new Vector3(1, 0.9f, 1);

                playerPun.transform.GetChild(0).gameObject.SetActive(true);
            }
            yield return null;
        }
=======
        playerPun.GetComponent<Player>().isSinglePlayer = false;
        loadingScreen(selectedMap);
>>>>>>> parent of 246c85b (O resto)
    }
}
