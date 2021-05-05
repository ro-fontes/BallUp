using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login")]
    public GameObject loginPn;
    public InputField playerName;
    string tempPlayerName;

    [Header("Lobby")]
    public GameObject lobbyPn;
    public InputField roomName;
    string tempRoomName;

    [Header("Player")]
    public GameObject playerPun;
    public GameObject[] Players;
    public Vector3[] SpawnPlayer;
    public GameObject[] Particles;
    public GameObject particle;
    int SaveParticle;
    int SaveSkin;
    int i = 1;

    #region LoadingScreen

    [Header("LOADINGSCREEN")]
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
    AsyncOperation async;
    
    

    public void loadingScreen(int sceneNo)
    {
        loadGameobject.gameObject.SetActive(true);
        StartCoroutine(Loading(sceneNo));
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
        if (backGroundImageAndLoop)
        {
            StartCoroutine(transitionImage());
        }
        playerPun = Players[PlayerPrefs.GetInt("Skin")];
        loginPn.gameObject.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
        tempPlayerName = "Neiva" + Random.Range(8, 99);
        playerName.text = tempPlayerName;
        tempRoomName = "Pato" + Random.Range(8, 99);

        particle = GameObject.Find("BallParticle");
        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
    }

    public void Login()
    {
        PhotonNetwork.ConnectUsingSettings();

        if (playerName.text != "")
        {
            PhotonNetwork.NickName = playerName.text;
        }
        else
        {
            PhotonNetwork.NickName = tempPlayerName;
        }

        loginPn.gameObject.SetActive(false);
        roomName.text = tempRoomName;
    }

    public void Quicksearch()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
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
        PhotonNetwork.CreateRoom(tempRoomName);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedRoom()
    {
        Debug.LogWarning("OnJoinedRoom");
        Debug.LogWarning("Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);
        Debug.LogWarning("Nome do Player: " + PhotonNetwork.NickName);
        Debug.LogWarning("Players Conectados: " + PhotonNetwork.CurrentRoom.PlayerCount);

        loginPn.gameObject.SetActive(false);
        loadingScreen(2);
    }
    IEnumerator Loading(int sceneNo)
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
    }
}
