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
    public string tempPlayerName;

    [Header("Lobby")]
    public GameObject lobbyPn;
    public GameObject noRoomPn;
    public InputField roomName;
    public Slider maxPlayers;
    public Button[] privacyButtons;
    public Button[] mapButtons;
    public Text maxPlayerCount;
    public bool roomVisible, roomOpened;
    public string tempRoomName;


    [Header("Player")]
    public string savedPlayerName;

    [Header("LoadMap")]
    [SerializeField]
    private string selectedMap = "0";
    private LoadingScreenMultiplayer loadScreen;

    public void loadingScreen(string sceneNo)
    {
        loadScreen.loadingScreen(sceneNo);
    }

    void Start()
    {
        privacyButtons[0].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
        mapButtons[0].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
        roomOpened = true;
        roomVisible = true;
        
        selectedMap = "0";
        loadScreen = GetComponent<LoadingScreenMultiplayer>();
        if (!PlayerPrefs.HasKey("namePlayerSaved"))
        {
            loginPn.gameObject.SetActive(true);
        }
        else
        {
            loginPn.gameObject.SetActive(false);
        }

        maxPlayers.value = 2;
        PhotonNetwork.AutomaticallySyncScene = true;
        tempPlayerName = "Player" + UnityEngine.Random.Range(8, 99);
        tempRoomName = "Room" + UnityEngine.Random.Range(8, 99);
        noRoomPn.SetActive(false);
        savedPlayerName = PlayerPrefs.GetString("namePlayerSaved");
    }

    private void Update()
    {
        maxPlayerCount.text = maxPlayers.value.ToString();
    }

    IEnumerator JoinLobby(float time)
    {
        PlayerPrefs.SetString("namePlayerSaved", savedPlayerName);
        PhotonNetwork.NickName = savedPlayerName;
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitForSeconds(time);
        PhotonNetwork.JoinLobby();
    }

    IEnumerator GenerateRoom(float time)
    {
        PlayerPrefs.SetString("namePlayerSaved", savedPlayerName);
        PhotonNetwork.NickName = savedPlayerName;
        PhotonNetwork.ConnectUsingSettings();
        loadingScreen(selectedMap);
        yield return new WaitForSeconds(time);

        RoomOptions roomOptions = new RoomOptions();

        roomOptions.IsVisible = roomVisible;
        roomOptions.IsOpen = roomOpened;

        roomOptions.MaxPlayers = Convert.ToByte(maxPlayers.value);

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

    public void SelectPrivateRoom(int number)
    {
        switch (number)
        {
            case 0:
                privacyButtons[0].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
                privacyButtons[1].GetComponentInChildren<Text>().color = Color.white;
                roomVisible = true;
                roomOpened = true;
                break;
            case 1:
                privacyButtons[1].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
                privacyButtons[0].GetComponentInChildren<Text>().color = Color.white;
                roomVisible = false;
                roomOpened = false;
                break;
        }
    }

    public void SelectMap(string mapName)
    {
        switch (mapName)
        {
            case "0":
                mapButtons[0].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
                mapButtons[1].GetComponentInChildren<Text>().color = Color.white;
                //mapButtons[2].GetComponentInChildren<Text>().color = Color.white;
                selectedMap = mapName;
                break;
            case "1":
                mapButtons[0].GetComponentInChildren<Text>().color = Color.white;
                mapButtons[1].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
                //mapButtons[2].GetComponentInChildren<Text>().color = Color.white;
                selectedMap = mapName;
                break;
            case "2":
                mapButtons[0].GetComponentInChildren<Text>().color = Color.white;
                mapButtons[1].GetComponentInChildren<Text>().color = Color.white;
                //mapButtons[2].GetComponentInChildren<Text>().color = new Color(1.0f, 0.64f, 0.0f);
                selectedMap = mapName;
                break;
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.LogWarning("OnJoinedRoom");
        Debug.LogWarning("Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);
        Debug.LogWarning("Quantidade maxima de players da sala: " + PhotonNetwork.CurrentRoom.MaxPlayers);
        Debug.LogWarning("Nome do Player: " + PhotonNetwork.NickName);
        Debug.LogWarning("Players Conectados: " + PhotonNetwork.CurrentRoom.PlayerCount);
        loginPn.gameObject.SetActive(false);
    }
}
