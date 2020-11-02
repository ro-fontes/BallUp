using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
    [Space(20)]
	public Text _txtStars;
	public Text _txtFragments;
	public Text _txtTime;
    [Space(20)]
    public int Stars = 0;
	public int Fragments = 0;
    [Space(20)]
    public GhostManager ghost;
    public GameObject Blur;
    public GameObject completeLevelUI;
    public GameObject UIActive;

    float secondsCount;

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
        //_txtStars.text = Stars.ToString();
        //ghost.recording = true;
        _txtFragments.text = Fragments.ToString();

        if (SceneManager.sceneCount == 1)
		{
			secondsCount = 0;
		}
	}
    void Update()
    {
        UpdateTimerUI();
        ChangeCursorLock();
        _txtFragments.text = Fragments.ToString();
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
        if(completeLevelUI.activeSelf || UIActive.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
    }

    void UpdateTimerUI()
	{
		secondsCount += Time.deltaTime;
		_txtTime.text = FormatTime(secondsCount);
	}

	public void AddStars(int value)
	{
		Stars += value;
		//_txtStars.text = Stars.ToString();
		PlayerPrefs.SetInt("Stars", Stars);
	}
	public void AddFragments(int value)
	{
		Fragments += value;
		_txtFragments.text = Fragments.ToString();
		PlayerPrefs.SetInt("Fragments", Fragments);
	}
	public void FinishLevel()
	{
		ghost.recording = false;
		//ghost.SaveList();
	}
}
