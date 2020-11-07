using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEditor;


public class PlayerSelect : MonoBehaviour
{
    GameObject player;
    GameObject particle;
    int SaveSkin;
    int SaveParticle;

    public static PlayerSelect Instance;
    [Space(20)]
    public GameObject SpawnParticle;
    [Space(20)]
    public Vector3[] SpawnPlayer;
    public GameObject[] Players;
    public GameObject[] Particles;
    [Space(20)]
    public Button[] level;
    public float RotateSpeed;


    public GameObject Stars, Fragments, Ball;
    public GameObject LoadGameobjct;
    public GameObject bar;
    public Text loadingText;
    public bool backGroundImageAndLoop;
    public float LoopTime;
    public GameObject[] backgroundImages;
    [Range(0, 1f)] public float vignetteEfectVolue; // Must be a value between 0 and 1
    AsyncOperation async;
    Image vignetteEfect;


    public void loadingScreen(int sceneNo)
    {
        LoadGameobjct.gameObject.SetActive(true);
        StartCoroutine(Loading(sceneNo));


        //SceneManager.LoadScene(sceneNo, LoadSceneMode.Single);
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
    }

    void Start()
    {

        vignetteEfect = transform.Find("VignetteEfect").GetComponent<Image>();
        vignetteEfect.color = new Color(vignetteEfect.color.r, vignetteEfect.color.g, vignetteEfect.color.b, vignetteEfectVolue);

        if (backGroundImageAndLoop)
            StartCoroutine(transitionImage());





        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
    }
    private void FixedUpdate()
    {
        player.transform.Rotate(0, RotateSpeed, 0);
    }
    void Update()
    {
        //---------------Aparece no menu principal----------------
        if (!player)
        {
            player = GameObject.Find("SkinManager");
            player = Instantiate(Players[SaveSkin], player.transform.position, player.transform.rotation);
            player.GetComponent<Player>().enabled = false;
            player.transform.parent = GameObject.Find("SkinManager").transform;
            player.gameObject.transform.localScale = new Vector3(10, 10, 10);
            //----------------------------------FAZER-Particula-aparecer-no-menu-principal---------------------------
            //particle.transform.parent = SpawnParticle.transform;
            //particle.gameObject.transform.localScale = new Vector3(10, 10, 10);
        }
        if (!particle)
        {
            SpawnParticle = GameObject.Find("SpawnParticle");
            particle = Instantiate(Particles[SaveParticle], SpawnParticle.transform.position, SpawnParticle.transform.rotation);

        }

        //---------------Aparece no menu principal----------------
        for (int i = 1; i < 4; i++)
        {
            if (PlayerPrefs.HasKey(i + "Stars"))
            {
                level[i].interactable = true;
            }
            else
            {
                level[i].interactable = false;
            }
        }
    }

    public void SelectMap(int index)
    {
        SpawnParticle = GameObject.Find("SpawnParticle");
        particle = Instantiate(Particles[SaveParticle], SpawnParticle.transform.position, SpawnParticle.transform.rotation);

        player.transform.parent = null;
        particle.transform.parent = null;

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(particle);

        particle.name = "BallParticle";
        player.name = "Player";

        player.AddComponent<Rigidbody>();
        player.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;


        player.GetComponent<Player>().enabled = true;
        player.GetComponent<ChangeColor>().enabled = false;

        player.gameObject.transform.localScale = new Vector3(1, 1, 1);
        particle.gameObject.transform.localScale = new Vector3(1, 1, 1);


        player.transform.position = SpawnPlayer[index - 2];
    }

    public void SelectSkin(int index)
    {
        SaveSkin = index;
        player.transform.Rotate(0, RotateSpeed, 0);
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
                



                player.transform.parent = null;
                particle.transform.parent = null;

                DontDestroyOnLoad(player);
                DontDestroyOnLoad(particle);

                particle.name = "BallParticle";
                player.name = "Player";

                player.AddComponent<Rigidbody>();
                player.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;


                player.GetComponent<Player>().enabled = true;
                player.GetComponent<ChangeColor>().enabled = false;

                particle.name = "BallParticle";
                player.name = "Player";

                player.gameObject.transform.localScale = new Vector3(1, 1, 1);
                particle.gameObject.transform.localScale = new Vector3(1, 1, 1);


                player.transform.position = SpawnPlayer[sceneNo - 2];
                player.SetActive(true);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
