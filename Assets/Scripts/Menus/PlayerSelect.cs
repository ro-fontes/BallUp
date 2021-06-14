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
    private AsyncOperation async;

    [HideInInspector]
    public int Language;

    [Header("Player")]
    public Vector3[] SpawnPlayer;
    public GameObject spawn;
    [SerializeField]
    private GameObject[] Players;    
    [SerializeField]
    private GameObject[] particles;
    [SerializeField]
    private GameObject[] playersFBX;
    [SerializeField]
    private GameObject[] particlesFBX;
    private GameObject playerFbxMenu, particleFbxMenu, player, particle;
    private int SaveSkin, SaveParticle;
    private float savedColorR, savedColorG, savedColorB;

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
            playerFbxMenu = Instantiate(playersFBX[SaveSkin], spawn.transform.position, Quaternion.identity);
            playerFbxMenu.transform.parent = spawn.transform;
            playerFbxMenu.gameObject.transform.localScale = new Vector3(10, 10, 10);
            playerFbxMenu.name = "Player";
        }
        if (!particleFbxMenu)
        {
            particleFbxMenu = Instantiate(particlesFBX[SaveParticle], playerFbxMenu.transform.position, Quaternion.identity);
            particleFbxMenu.transform.parent = spawn.transform;
            particleFbxMenu.name = "BallParticle";
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
        Destroy(particleFbxMenu);
    }

    public void loadingScreen(int sceneNo)
    {
        //LoadGameobjct.gameObject.SetActive(true);
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
                    player = Instantiate(Players[SaveSkin], SpawnPlayer[sceneNo - 2], Quaternion.identity);
                    player.transform.parent = null;
                    player.GetComponent<MeshRenderer>().material.color = new Color(savedColorR, savedColorG, savedColorB, 255f);
                    player.GetComponent<Player>().isSinglePlayer = true;
                }
                if (!particle)
                {
                    particle = Instantiate(particles[SaveParticle], playerFbxMenu.transform.position, Quaternion.identity);
                    particle.transform.parent = null;
                }

                DontDestroyOnLoad(player);
                DontDestroyOnLoad(particle);

                player.name = "Player" + 0;
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
