using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("PROPERTIES")]
    public float bulletSpeed = 1000f;
    public float bulletTimeLife = 3f;
    float bulletTimeCount = 0f;
    Rigidbody bulletRb;

    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * bulletSpeed);
    }

    void Update()
    {
        if (bulletTimeCount >= bulletTimeLife)
        {
            Destroy(this.gameObject);
        }

        bulletTimeCount += Time.deltaTime;
    }
}
