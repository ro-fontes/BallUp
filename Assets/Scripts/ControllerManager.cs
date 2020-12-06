using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour
{
    [Tooltip("PS Images")]
    public GameObject X_PS, Cube_PS, Triangle_PS, Circle_PS, R1, R2, L1, L2;
    [Tooltip("XBX Images")]
    public GameObject  LT, RT, X_XBX, A_XBX, B_XBX, Y_XBX, Start;
    public GameObject[] LB, RB;

    [HideInInspector]
    public int Xbox_One_Controller = 0;
    [HideInInspector]
    public int PS4_Controller = 0;
    [HideInInspector]
    public int Mouse_Controller = 0;


    // Update is called once per frame
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19 && Mouse_Controller == 0)
            {
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
                Mouse_Controller = 0;
            }
            if (names[x].Length == 33 && Mouse_Controller == 0)
            {
                PS4_Controller = 0;
                Xbox_One_Controller = 1;
                Mouse_Controller = 0;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Mouse_Controller = 1;
            PS4_Controller = 0;
            Xbox_One_Controller = 0;
        }

        if (Input.GetButton("A"))
        {
            Xbox_One_Controller = 1;
            Mouse_Controller = 0;
            PS4_Controller = 0;
        }

        if (Xbox_One_Controller == 1 && Mouse_Controller == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                Cursor.visible = false;
                X_XBX.SetActive(true);
                Y_XBX.SetActive(true);
                for(int i = 0; i < RB.Length; i++)
                {
                    RB[i].SetActive(true);
                }
                for (int i = 0; i < LB.Length; i++)
                {
                    LB[i].SetActive(true);
                }
                Start.SetActive(true);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                for (int i = 0; i < RB.Length; i++)
                {
                    RB[i].SetActive(true);
                }
                for (int i = 0; i < LB.Length; i++)
                {
                    LB[i].SetActive(true);
                }

            }
        }
        else if (PS4_Controller == 1 && Mouse_Controller == 0)
        {
            Cursor.visible = false;
            //X_PS.SetActive(true);
            //Cube_PS.SetActive(true);
            //Triangle_PS.SetActive(true);
            //Circle_PS.SetActive(true);
            //R1.SetActive(true);
            //R2.SetActive(true);
            //L1.SetActive(true);
            //L2.SetActive(true);
        }
        else if(Mouse_Controller == 1 && Xbox_One_Controller == 0)
        {
            //Xbox Buttons
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                Cursor.visible = true;
                X_XBX.SetActive(false);
                Y_XBX.SetActive(false);
                for (int i = 0; i < RB.Length; i++)
                {
                    RB[i].SetActive(false);
                }

                for (int i = 0; i < LB.Length; i++)
                {
                    LB[i].SetActive(false);
                }
                Start.SetActive(false);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                for (int i = 0; i < RB.Length; i++)
                {
                    RB[i].SetActive(false);
                }
                for (int i = 0; i < LB.Length; i++)
                {
                    LB[i].SetActive(false);
                }
            }

            //Playstation Buttons
            //X_PS.SetActive(false);
            //Cube_PS.SetActive(false);
            //Triangle_PS.SetActive(false);
            //Circle_PS.SetActive(false);
            //R1.SetActive(false);
            //R2.SetActive(false);
            //L1.SetActive(false);
            //L2.SetActive(false);
        }
    }
}

