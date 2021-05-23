using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    #region variables
    
    [Header("Sounds Config")]

    [Tooltip("Set Audio FX")]
    public AudioClip InWater;
    public AudioClip Outwater;

    [Header("Player Config")]

    [SerializeField]
    [Tooltip("Set Player speed")]
    [Range(1, 20)]
    private float speed = 9;
    public bool isSinglePlayer;

    [SerializeField]
    [Tooltip("Set Jump Force")]
    [Range(100, 1000)]
    private float jumpFloat = 300;

    [SerializeField]
    [Tooltip("Set Fluctuation Multiplier")]
    [Range(1, 10)]
    private float fluctuationMultiplier = 3;
    public GameObject MyCamera;

    CinemachineFreeLook FreeLookCam;
    PhotonView MyPhotonView;
    Rigidbody rb;
    Vector3 force;
    GameObject WaterInScene;
    GameObject BallParticle;
    AudioSource AudioPlayer;
    float WaterDepth;
    bool isFloor;

    #endregion

    void Start()
    {
        if (!isSinglePlayer)
        {
            MyCamera.transform.parent = null;
            MyPhotonView = GetComponent<PhotonView>();
            if (!MyPhotonView.IsMine)
            {
                MyCamera.gameObject.SetActive(false);
            }
        }
        
        AudioPlayer = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isSinglePlayer)
        {
            if (MyPhotonView.IsMine)
            {
                Move();
            }
        }
        else
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

        if (MyPhotonView.IsMine)
        {
            if (rb.velocity.magnitude >= 2.5f && WaterInScene == null && isFloor == true)
            {
                PlayParticle();
            }
            else
            {
                StopParticle();
                //GetComponent<PhotonView>().RPC("StopParticle", RpcTarget.AllBufferedViaServer);
            }
        }
        else
        {
            return;
        }


        if (WaterInScene != null)
        {
            if (transform.position.y <= WaterInScene.transform.position.y)
            {
                WaterDepth = transform.position.y - WaterInScene.transform.position.y;
                force.y = (Physics.gravity.y * WaterDepth - rb.velocity.y) * fluctuationMultiplier;
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
                    if (!isSinglePlayer)
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
                    else
                    {
                        rb.AddForce(Vector3.up * jumpFloat);
                    }
                }
            }
        }

        if (!hit.collider)
        {
            isFloor = false;
        }
    }

    [PunRPC]
    public void PlayParticle()
    {
         BallParticle.GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    public void StopParticle()
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
