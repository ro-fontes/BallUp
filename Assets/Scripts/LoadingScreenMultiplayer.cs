using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoadingScreenMultiplayer : MonoBehaviour
{
    public Animator loadAnimator;
    GameObject stars, fragments;

    [Header("LoadingScreen")]
    [SerializeField]
    private GameObject loadGameobject;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    [Range(0, 1f)] private float vignetteEffectValue;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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
        loadGameobject.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);

        PhotonNetwork.LoadLevel(sceneNo);
    }
}
