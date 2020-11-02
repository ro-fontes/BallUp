using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnimation : MonoBehaviour
{

    public Transform target;
    public Vector3 Distance;
    private Vector3 velocity = new Vector3(7f, 7f, 7f);

    void FixedUpdate()
    {
        target = GameObject.Find("Player").transform;
        transform.position = Vector3.Lerp(transform.position, target.position, 5f);
    }
}