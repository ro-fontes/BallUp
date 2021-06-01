using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(SphereCollider))]

public class Player : MonoBehaviour
{
    #region variables

    [Header("Sounds Config")]

    [Tooltip("Set Audio FX")]
    public AudioClip InWater;
    public AudioClip Outwater;

    [Header("Player Config")]

    public int myActorNumber;
    [SerializeField]
    [Tooltip("Set Player speed")]
    [Range(1, 20)]
    private float speed = 9;
    public bool isSinglePlayer;
    public bool isDead = false;
    public PlayerParticle particlesSystem;
    [SerializeField]
    [Tooltip("Set Jump Force")]
    [Range(100, 1000)]
    private float jumpFloat = 300;

    [SerializeField]
    [Tooltip("Set Fluctuation Multiplier")]
    [Range(1, 10)]
    private float fluctuationMultiplier = 3;
    public GameObject MyCamera;
    public GameObject Network;
    [Range(0.1f, 10)]
    public float dissolvepeed = 1f;

    private CinemachineFreeLook FreeLookCam;
    private PhotonView MyPhotonView;
    private Rigidbody rb;
    private Vector3 force;
    private MeshRenderer meshRenderer;
    private GameObject BallParticle;
    private AudioSource AudioPlayer;
    private float WaterDepth;
    private Material[] mats;
    private MeshRenderer[] matsChildren;

    [HideInInspector]
    public GameObject WaterInScene;
    [HideInInspector]
    public bool isFloor;

    public float timeDissolve = 1f;
    #endregion



    void Start()
    {
        timeDissolve = 1f;
        if (!isSinglePlayer)
        {
            MyCamera.transform.parent = null;
            MyPhotonView = GetComponent<PhotonView>();
            if (!MyPhotonView.IsMine)
            {
                MyCamera.gameObject.SetActive(false);
            }
            else
            {
                MyPhotonView.RPC("RPC_ChangeName", RpcTarget.AllBufferedViaServer, myActorNumber);
            }
        }
        else
        {
            PhotonNetwork.OfflineMode = true;
        }
        meshRenderer = this.GetComponent<MeshRenderer>();
        AudioPlayer = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();

        mats = meshRenderer.materials;
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
        if (!Network)
        {
            Network = GameObject.Find("NetworkManager");
        }

        if (!FreeLookCam)
        {
            FreeLookCam = FindObjectOfType<CinemachineFreeLook>();
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

        mats[0].SetFloat("_Cutoff", Mathf.Clamp01(timeDissolve * dissolvepeed));
        matsChildren = GetComponentsInChildren<MeshRenderer>();
        if (gameObject.transform.childCount > 1)
        {
            foreach (MeshRenderer mesh in matsChildren)
            {
                mesh.material.SetFloat("_Cutoff", Mathf.Clamp01(timeDissolve * dissolvepeed));
            }
        }
        if (isDead && timeDissolve < 1f)
        {
            timeDissolve += Time.deltaTime;
        }
        if (!isDead && timeDissolve > 0f)
        {
            timeDissolve -= Time.deltaTime;
        }

        meshRenderer.materials = mats;    
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

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.7f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.5f);

            if (hit.collider)
            {
                isFloor = true;
                if (!isSinglePlayer)
                {
                    if (MyPhotonView.IsMine)
                    {
                        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
                        {
                            rb.AddForce(Vector3.up * jumpFloat);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Void"))
        {
            StartCoroutine(PlayerDied());
        }
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
    IEnumerator PlayerDied()
    {
        isDead = true;
        yield return new WaitForSeconds(1);
        transform.position = Network.GetComponent<Network>().SpawnPlayer[myActorNumber - 1];
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(1);
        isDead = false;
    }

    [PunRPC]
    public void SetColor(float R, float G, float B)
    {
        GetComponent<MeshRenderer>().material.color = new Color(R, G, B, 255f);
    }

    [PunRPC]
    public void RPC_ChangeName(int myActorID)
    {
        this.gameObject.name = "Player" + myActorID;
    }

    [PunRPC]
    public void DisableWaitingPlayers(GameObject PanelWait)
    {
        PanelWait.SetActive(false);
    }  
}
