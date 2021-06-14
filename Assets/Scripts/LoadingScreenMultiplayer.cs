using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadingScreenMultiplayer : MonoBehaviour
{
    public Animator loadAnimator;
    GameObject stars, fragments;

    private void Start()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        fragments = GameObject.FindGameObjectWithTag("Fragments");
        stars = GameObject.FindGameObjectWithTag("Stars");
    }

    public void loadingScreen(string sceneNo)
    {
        StartCoroutine(Loading(sceneNo));
    }
    
    IEnumerator Loading(string sceneNo)
    {
        fragments.SetActive(false);
        stars.SetActive(false);
        loadAnimator.SetTrigger("BallAnim");
        yield return new WaitForSeconds(4);
        PhotonNetwork.LoadLevel(sceneNo);
    }
}
