using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Varibles
    public static GameManager Instance;
    [Space(20)]
    [Tooltip("Set Fragments text")]
    public Text _txtFragments;
    [Tooltip("Set Timer seconds")]
    public Text _txtTime;
    [Space(20)]

    [SerializeField]
    private int Stars, Fragments;
    [Tooltip("Put how many seconds to pass the level")]
    public int LevelSecond;
    [Space(20)]
    public GhostManager ghost;
    [Tooltip("Set Gameobject EndLevelUI")]
    public GameObject completeLevelUI;
    [Tooltip("Set Gameobject Panel")]
    public GameObject UIActive;
    GameObject ControllerManager;

    int CurrentLevel;
    int levelIndex;
    int scoreAtualDoNivel;
    int scoreMaximoSalvo;
    float secondsCount;

    #endregion

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        Stars = PlayerPrefs.GetInt("Stars");
        Fragments = PlayerPrefs.GetInt("Fragments");
    }

    private void Start()
    {
        if (!ControllerManager)
        {
            ControllerManager = GameObject.Find("ControllerManager");
        }

        //ghost.recording = true;		
        _txtFragments.text = Fragments.ToString();
        CurrentLevel = SceneManager.GetActiveScene().buildIndex;

        levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (SceneManager.sceneCount == 1)
        {
            secondsCount = 0;
        }
    }

    void Update()
    {
        UpdateTimerUI();
        ChangeCursorLock();
        if(scoreMaximoSalvo != PlayerPrefs.GetInt(levelIndex + "Stars"))
        {
            scoreMaximoSalvo = PlayerPrefs.GetInt(levelIndex + "Stars");
        }

        _txtFragments.text = Fragments.ToString();

        if (completeLevelUI.activeSelf == true)
        {
            if (Input.GetButtonDown("A"))
            {

                print("A");
            }
            if (Input.GetButtonDown("X"))
            {
                print("x");

            }
            if (Input.GetButtonDown("B"))
            {
                print("B");
            }
            if (Input.GetButtonDown("Y"))
            {
                print("Y");
            }
        }
    }

    string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }

    public void ChangeCursorLock()
    {
        if (completeLevelUI.activeSelf || UIActive.activeSelf)
        {
            if (ControllerManager.GetComponent<ControllerManager>().Mouse_Controller == 1)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);

        if (!PlayerPrefs.HasKey(levelIndex + "LevelTime"))
        {
            PlayerPrefs.SetFloat(levelIndex + "LevelTime", secondsCount);
        }

        if (PlayerPrefs.GetFloat(levelIndex + "LevelTime") > secondsCount)
        {
            PlayerPrefs.SetFloat(levelIndex + "LevelTime", secondsCount);
        }
    }

    void UpdateTimerUI()
    {
        secondsCount += Time.deltaTime;
        _txtTime.text = FormatTime(secondsCount);
    }

    public void AddStars()
    {
        #region Check the time
        if (secondsCount <= LevelSecond)
        {
            scoreAtualDoNivel = 3;
            //print("3 estrelas");
        }
        else if (secondsCount <= LevelSecond + 10)
        {
            scoreAtualDoNivel = 2;
            //print("2 estrelas");
        }
        else
        {
            scoreAtualDoNivel = 1;
            //print("1 estrelas");
        }

        #endregion

        #region Check MaxStars
        if (PlayerPrefs.HasKey(levelIndex + "Stars"))
        {
            switch (scoreAtualDoNivel)
            {
                case 2:
                    if (scoreMaximoSalvo == 1)
                    {
                        Stars += 1;
                        scoreMaximoSalvo = 2;
                        PlayerPrefs.SetInt("Stars", Stars);
                        PlayerPrefs.SetInt(levelIndex + "Stars", scoreMaximoSalvo);
                    }
                    break;

                case 3:
                    if (scoreMaximoSalvo == 2)
                    {
                        Stars += 1;
                        scoreMaximoSalvo = 3;

                        PlayerPrefs.SetInt(levelIndex + "Stars", scoreMaximoSalvo);
                        PlayerPrefs.SetInt("Stars", Stars);
                    }
                    if (scoreMaximoSalvo == 1)
                    {
                        scoreMaximoSalvo = 3;
                        Stars += 2;
                        PlayerPrefs.SetInt(levelIndex + "Stars", scoreMaximoSalvo);
                        PlayerPrefs.SetInt("Stars", Stars);
                    }
                    break;
            }
            scoreMaximoSalvo = scoreAtualDoNivel;
        }
        else
        {
            scoreMaximoSalvo = scoreAtualDoNivel;
            Stars += scoreMaximoSalvo;
            PlayerPrefs.SetInt(levelIndex + "Stars", scoreMaximoSalvo);
            PlayerPrefs.SetInt("Stars", Stars);
        }
        #endregion
    }

    public void AddFragments(int value)
    {
        Fragments += value;
        _txtFragments.text = Fragments.ToString();
        PlayerPrefs.SetInt("Fragments", Fragments);
    }
}
