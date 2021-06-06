using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMenuManager : MonoBehaviour
{
    public GameObject playerMenu, playerParticle;
    public GameObject menuParticles;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerParticle)
        {
            playerParticle = GameObject.Find("BallParticle");
            playerParticle.SetActive(false);
        }
        if (!playerMenu)
        {
            playerMenu = GameObject.FindWithTag("Player");
        }

        if (menuParticles.activeSelf == true)
        {
            
            
            animator.SetBool("TriggerParticleAnim", true);
            playerParticle.transform.localScale = new Vector3(6, 6, 6);
            playerParticle.SetActive(true);
            playerParticle.transform.localPosition = new Vector3(0, -5, 0);
            playerMenu.GetComponent<Rotate>().axis = EnumAxis.X;
            playerMenu.GetComponent<Rotate>().speed = 5;
        }
        else
        {
            playerParticle.SetActive(false);
            playerMenu.GetComponent<Rotate>().axis = EnumAxis.Y;
            animator.SetBool("TriggerParticleAnim", false);
            playerMenu.GetComponent<Rotate>().speed = 0.5f;
        }

    }
}
