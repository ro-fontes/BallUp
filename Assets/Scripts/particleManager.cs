using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleManager : MonoBehaviour
{
    public GameObject Spawn;

    void Update()
    {
        Spawn = GameObject.Find("Player");

        transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
    }
}
