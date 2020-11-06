using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour
{
    [Tooltip("PS Images")]
    public Image X_PS, Cube_PS, Triangle_PS, Circle_PS, R1, R2, L1, L2;
    [Tooltip("XBX Images")]
    public Image Lb, RB, LT, RT, X_XBX, A_XBX, B_XBX, Y_XBX;
    int Xbox_One_Controller = 0;
    int PS4_Controller = 0;
    int Mouse_Controller = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Mouse_Controller = 1;
            Lb.enabled = RB.enabled = LT.enabled = RT.enabled = X_XBX.enabled = A_XBX.enabled = B_XBX.enabled = Y_XBX.enabled = false;
            X_PS.enabled = Cube_PS.enabled = Triangle_PS.enabled = Circle_PS.enabled = R1.enabled = R2.enabled = L1.enabled = L2.enabled = false;
        }
        if (Input.anyKey)
        {
            print("Qualquer teclad");
        }

        if (Xbox_One_Controller == 1 )
        {
            Lb.enabled = RB.enabled = LT.enabled = RT.enabled = X_XBX.enabled = A_XBX.enabled = B_XBX.enabled = Y_XBX.enabled = true;
        }
        else if (PS4_Controller == 1)
        {
            X_PS.enabled = Cube_PS.enabled = Triangle_PS.enabled = Circle_PS.enabled = R1.enabled = R2.enabled = L1.enabled = L2.enabled = true;
        }
        else
        {

        }
    }
}

