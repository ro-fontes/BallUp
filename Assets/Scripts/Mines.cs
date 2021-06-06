using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mines : MonoBehaviourPunCallbacks
{
    [Range(1, 100)]
    public float explosionRange = 2;
    [Range(1, 1000)]
    public float explosionForce = 50;
    [Range(1, 360)]
    public float radius = 2;

    public GameObject particleExplosion;

    public AudioClip soundExplosion;
    public float distance;
    public Vector3 explosionPosition;

    public AudioSource audioSource;
    private GameObject player, particle;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);
        if (distance < explosionRange)
        {
            explosionPosition = transform.position;

            Explode();
            
            //GetComponent<PhotonView>().RPC("Explosion", RpcTarget.All);
        }
        else
        {
            player = null;
        }
    }
    [PunRPC]
    private void Explode()
    {
        if (!particle)
        {
            player.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPosition, radius, 0);
            particle = Instantiate(particleExplosion, transform.position, Quaternion.identity);
            particle.transform.parent = this.gameObject.transform;
            audioSource.PlayOneShot(soundExplosion);
            
        }
        GetComponentInChildren<MeshRenderer>().enabled = false;
        Destroy(this.gameObject, 3f);
    }

    [PunRPC]
    public void Explosion()
    {
        audioSource.PlayOneShot(soundExplosion);
        particleExplosion.GetComponent<ParticleSystem>().Play();
        Destroy(this.gameObject);
    }
}
