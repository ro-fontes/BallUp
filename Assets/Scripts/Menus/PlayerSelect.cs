using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerSelect : MonoBehaviour
{
    #region Variables

    public static PlayerSelect Instance;
    public Vector3[] SpawnPlayer;
    [HideInInspector]
    public int Language;

    [SerializeField]
    private GameObject[] Players;
    [SerializeField]
    private GameObject[] Particles;
    [Tooltip("Put the buttons that will activate if the player passes the level")]
    [SerializeField]
    private Button[] levelButtonActivate;
    [SerializeField]
    private float RotateSpeed = 0.5f;
    [SerializeField]
    private GameObject Stars, Fragments;
    [Space(20)]
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
    GameObject player, particle;

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
    }
    private void FixedUpdate()
    {
        player.transform.Rotate(0, RotateSpeed, 0);
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.Find("SkinManager");
            player = Instantiate(Players[SaveSkin], player.transform.position, player.transform.rotation);
            player.name = "Player";
            player.GetComponent<Player>().enabled = false;
            player.transform.parent = GameObject.Find("SkinManager").transform;
            player.gameObject.transform.localScale = new Vector3(10, 10, 10);
        }
        if (!particle)
        {
            particle = Instantiate(Particles[SaveParticle], player.transform.position, player.transform.rotation);
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
                player.transform.parent = null;
                particle.transform.parent = null;

                DontDestroyOnLoad(player);
                DontDestroyOnLoad(particle);

                particle.name = "BallParticle";
                //player.name = "Player";

                player.AddComponent<Rigidbody>();
                player.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;


                player.GetComponent<Player>().enabled = true;
                player.GetComponent<ChangeColor>().enabled = false;

                //particle.name = "BallParticle";
                //player.name = "Player";

                player.gameObject.transform.localScale = new Vector3(1, 1, 1);
                particle.gameObject.transform.localScale = new Vector3(1, 1, 1);


                player.transform.position = SpawnPlayer[sceneNo - 2];
                player.SetActive(true);
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
