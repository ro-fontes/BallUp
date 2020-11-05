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

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    void Start()
    {
        SaveSkin = PlayerPrefs.GetInt("Skin");
        SaveParticle = PlayerPrefs.GetInt("Particle");
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

        }
        player.transform.Rotate(0, RotateSpeed, 0);
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
        //---------------Aparece no menu principal----------------
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


        player.transform.position = SpawnPlayer[index - 1];








        SceneManager.LoadScene(index, LoadSceneMode.Single);
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
}
