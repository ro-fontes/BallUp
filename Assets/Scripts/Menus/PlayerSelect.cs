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
    private GameObject[] playersFBX;
    [SerializeField]
    private GameObject[] Particles;
    private GameObject playerFbxMenu, player, particle;
    private int SaveSkin, SaveParticle;
    float savedColorR, savedColorG, savedColorB;

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
    public Animator loadAnimator;
    #endregion

    [Header("LoadingScreen")]

    [SerializeField]
    private GameObject LoadGameobjct;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    [Range(0, 1f)] private float vignetteEfectVolue;

    AsyncOperation async;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
    }

    void Update()
    {
        savedColorR = PlayerPrefs.GetFloat("Color");
        savedColorG = PlayerPrefs.GetFloat("Color1");
        savedColorB = PlayerPrefs.GetFloat("Color2");

        if (!playerFbxMenu)
        {
            playerFbxMenu = Instantiate(playersFBX[SaveSkin], spawn.transform.position, spawn.transform.rotation);
            playerFbxMenu.transform.parent = spawn.transform;
            playerFbxMenu.gameObject.transform.localScale = new Vector3(10, 10, 10);
            playerFbxMenu.name = "Player";
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
        Destroy(playerFbxMenu);
    }

    public void SelectParticle(int index)
    {
        SaveParticle = index;
        PlayerPrefs.SetInt("Particle", SaveParticle);
        Destroy(particle);
    }

    public void loadingScreen(int sceneNo)
    {
        LoadGameobjct.gameObject.SetActive(true);
        StartCoroutine(Loading(sceneNo));
    }

    IEnumerator Loading(int sceneNo)
    {
        Stars.SetActive(false);
        Fragments.SetActive(false);
        loadAnimator.SetTrigger("BallAnim");
        yield return new WaitForSeconds(4);

        async = SceneManager.LoadSceneAsync(sceneNo);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            if (async.progress >= 0.9f && !loadAnimator.IsInTransition(0))
            {
                //Player spawn
                if (!player)
                {
                    player = Instantiate(Players[SaveSkin], SpawnPlayer[sceneNo - 2], new Quaternion(0, 0, 0, 0));
                    player.transform.parent = null;
                    player.GetComponent<MeshRenderer>().material.color = new Color(savedColorR, savedColorG, savedColorB, 255f);
                    player.GetComponent<Player>().isSinglePlayer = true;
                }

                if (!particle)
                {
                    particle = Instantiate(Particles[SaveParticle], new Vector3(-1000, -1000, 0), new Quaternion(0, 0, 0, 0));
                    particle.transform.parent = null;
                }

                DontDestroyOnLoad(player);
                DontDestroyOnLoad(particle);

                player.name = "Player" + SaveSkin;
                particle.name = "BallParticle";

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
