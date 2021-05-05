using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanavsControl : MonoBehaviour
{
    public GameObject spawn;
    int PlayerSkin;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSkin = PlayerPrefs.GetInt("Skin");
    }

    void LateUpdate()
    {
        if (!spawn)
        {
            spawn = GameObject.Find("Player" + PlayerSkin + "(Clone)");
        }
        gameObject.transform.parent = null;
        this.transform.position = spawn.transform.position + new Vector3(0, 1, 0);
    }
}
