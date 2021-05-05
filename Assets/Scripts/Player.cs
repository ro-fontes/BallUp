using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    #region variables

    [Tooltip("Set Audio FX")]
    [SerializeField]
    private AudioClip InWater, Outwater;
    [Tooltip("Set Player Speed")]
    [SerializeField]
    private float speed = 9;
    [Tooltip("Set Jump Force")]
    [SerializeField]
    private float jumpFloat = 1;

    public GameObject MyCamera;
    CinemachineFreeLook FreeLookCam;

    PhotonView MyPhotonView;
    Rigidbody rb;
    Vector3 force;
    GameObject WaterInScene;
    GameObject BallParticle;
    AudioSource AudioPlayer;
    float WaterDepth;
    float multiplier = 3;
    bool isFloor;

    #endregion

    void Start()
    {
        MyPhotonView = GetComponent<PhotonView>();
        AudioPlayer = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        if (!MyPhotonView.IsMine)
        {
            MyCamera.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if(MyPhotonView.IsMine)
        {
            Move();
        }
    }

    void Update()
    {
        playerJump();

        if (!FreeLookCam)
        {
            FreeLookCam = FindObjectOfType<CinemachineFreeLook>();
        }

        if (!BallParticle)
        {
            BallParticle = GameObject.Find("BallParticle");
        }


        if (rb.velocity.magnitude >= 2.5f && WaterInScene == null && isFloor == true)
        {
            PlayParticle();
        }
        else
        {
            StopParticle();
        }

        if (WaterInScene != null)
        {
            if (transform.position.y <= WaterInScene.transform.position.y)
            {
                WaterDepth = transform.position.y - WaterInScene.transform.position.y;
                force.y = (Physics.gravity.y * WaterDepth - rb.velocity.y) * multiplier;
                rb.AddForce(force);
            }
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camDirection = (transform.position - FreeLookCam.transform.position).normalized;
        camDirection = new Vector3(camDirection.x, 0, camDirection.z);
        Vector3 right = Vector3.Cross(Vector3.up, camDirection);
        Vector3 movement = camDirection * z + right * x;

        rb.GetComponent<Rigidbody>().angularDrag = 0f;

        if (z == 0)
        {
            rb.GetComponent<Rigidbody>().angularDrag = 4.5f;
        }
        FreeLookCam.m_Lens.Dutch = x * 6;
        rb.AddForce(movement * speed);
    }

    void playerJump()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.7f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.5f);

            if(hit.collider)
            {
                isFloor = true;

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
                {
                    if (MyPhotonView.IsMine)
                    {
                        rb.AddForce(Vector3.up * jumpFloat);
                    }
                    else
                    {
                        return;
                    }
                    
                }
            }
        }

        if (!hit.collider)
        {
            isFloor = false;
        }
    }

    void PlayParticle()
    {
         BallParticle.GetComponent<ParticleSystem>().Play();
    }

    void StopParticle()
    {
        BallParticle.GetComponent<ParticleSystem>().Stop();
    }

    [PunRPC]
    public void SetColor(float R, float G, float B)
    {
        //this.GetComponent<MeshRenderer>().material.color = new Color(PlayerPrefs.GetFloat("Color"), PlayerPrefs.GetFloat("Color1"), PlayerPrefs.GetFloat("Color2"), 255f);
        GetComponent<MeshRenderer>().material.color = new Color(R, G, B, 255f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            AudioPlayer.PlayOneShot(InWater);
            WaterInScene = other.gameObject;
            rb.drag = 2f;
        }

        if (other.gameObject.CompareTag("Fragments"))
        {
            GameManager.Instance.AddFragments(1);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            rb.drag = 0;
            WaterInScene = null;
            AudioPlayer.PlayOneShot(Outwater);
        }
    }
}
