using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mines : MonoBehaviourPunCallbacks
{
    [Range(10, 10000)]
    public float explosionForce = 650;
    [Range(1, 360)]
    public float radius = 10;

    public GameObject particleExplosion;
    public AudioClip soundExplosion;

    private Vector3 explosionPosition;
    private AudioSource audioSource;
    private GameObject particle;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [PunRPC]
    private void Explode()
    {
        if (!particle)
        {
            particle = Instantiate(particleExplosion, transform.position, Quaternion.identity);
            particle.transform.parent = this.gameObject.transform;
            audioSource.PlayOneShot(soundExplosion);
        }

        GetComponentInChildren<MeshRenderer>().enabled = false;
        Destroy(this.gameObject, 2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            if (!collision.collider.gameObject.GetComponent<Player>().isSinglePlayer)
            {
                explosionPosition = transform.position;
                collision.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPosition, radius, 0);
                GetComponent<PhotonView>().RPC("Explode", RpcTarget.All);
            }
            else
            {
                explosionPosition = transform.position;
                collision.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPosition, radius, 0);
                Explode();
            }
        }
    }
}
