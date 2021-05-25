using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    #region Variables

    public static PlayerSelect Instance;

    [HideInInspector]
    public int Language;

    [Header("Player")]

    public Vector3[] SpawnPlayer;
    public GameObject spawn;
    [SerializeField]
    private GameObject[] Players;
    [SerializeField]
    private GameObject[] PlayersFBX;
    [SerializeField]
    private GameObject[] Particles;
    private GameObject player, playerPref, particle;

    [Header("Buttons")]

    [SerializeField]
    [Tooltip("Put the buttons that will activate if the player passes the level")]
    private Button[] levelButtonActivate;

    [Header("UI")]

    [SerializeField]
    private GameObject Stars;
    [SerializeField]
    private GameObject Fragments;

    [Header("Camera")]

    public GameObject cam;

    [Header("LoadingScreen")]

    [SerializeField]
    private GameObject LoadGameobjct;
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
    [Range(0, 1f)] private float vignetteEfectVolue;
    int SaveSkin, SaveParticle;
    AsyncOperation async;

    #endregion

    public void loadingScreen(int sceneNo)
    {
        LoadGameobjct.gameObject.SetActive(true);
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

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
    }

    void Start()
    {
        if (backGroundImageAndLoop)
        {
            StartCoroutine(transitionImage());
        }
        player.SetActive(true);
    }

    void Update()
    {
        if (!player)
        {
            player = Instantiate(PlayersFBX[SaveSkin], spawn.transform.position, spawn.transform.rotation);
            player.transform.parent = spawn.transform;
            player.gameObject.transform.localScale = new Vector3(10, 10, 10);
            player.name = "Player";
        }

        for (int i = 1; i < 4; i++)
        {
            if (PlayerPrefs.HasKey(i + "Stars"))
            {
                levelButtonActivate[i].interactable = true;
            }
            else
            {
                levelButtonActivate[i].interactable = false;
            }
        }
    }

    public void SelectSkin(int index)
    {
        SaveSkin = index;
        PlayerPrefs.SetInt("Skin", SaveSkin);
        Destroy(player);
    }

    public void SelectParticle(int index)
    {
        SaveParticle = index;
        PlayerPrefs.SetInt("Particle", SaveParticle);
        Destroy(particle);
    }

    IEnumerator Loading(int sceneNo)
    {
        Stars.SetActive(false);
        Fragments.SetActive(false);
        player.SetActive(false);

        async = SceneManager.LoadSceneAsync(sceneNo);
        async.allowSceneActivation = false;

        // Continue until the installation is completed
        while (async.isDone == false)
        {
            bar.transform.localScale = new Vector3(async.progress, 0.9f, 1);

            if (loadingText != null)
                loadingText.text = "%" + (100 * bar.transform.localScale.x).ToString("####");

            if (async.progress == 0.9f)
            {
                bar.transform.localScale = new Vector3(1, 0.9f, 1);

                //Player spawn
                if (!playerPref)
                {
                    playerPref = Instantiate(Players[SaveSkin], SpawnPlayer[sceneNo - 2], new Quaternion(0, 0, 0, 0));
                }
                if (!particle)
                {
                    particle = Instantiate(Particles[SaveParticle], playerPref.transform.position, playerPref.transform.rotation);
                }

                DontDestroyOnLoad(playerPref);
                DontDestroyOnLoad(particle);

<<<<<<< HEAD

                playerPref.name = "Player" + SaveSkin;

=======
                player.name = "Player" + SaveSkin;
>>>>>>> parent of 246c85b (O resto)
                particle.name = "BallParticle";
                playerPref.transform.GetChild(0).gameObject.SetActive(true);


                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void ChangeLanguage(int value)
    {
        Language = value;
    }
}
