using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelect : MonoBehaviour
{
    public Button Portuguese, English;
    public string[] Text;


    private void Start()
    {



    }
    private void Update()
    {
        if(PlayerSelect.Instance.Language == 1)
        {
            GetComponent<Text>().text = Text[1];
        }
        if(PlayerSelect.Instance.Language == 0)
        {
            GetComponent<Text>().text = Text[0];
        }
    }
    public void SetPortuguese()
    {
        Portuguese.GetComponent<Button>().interactable = true;
        English.gameObject.SetActive(false);
        //for (int i = 0; i > SetText.Length; i++)
        //{
        //    SetText[i].text = Text[1];
        //}
    }

    public void SetEnglish()
    {
        Portuguese.gameObject.SetActive(false);
        English.gameObject.SetActive(true);
        //for (int i = 0; i > SetText.Length; i++)
        //{
        //    SetText[i].text = Text[0];
        //}
    }
}

