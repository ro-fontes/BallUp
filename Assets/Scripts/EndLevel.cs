using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    void OnTriggerEnter()
    {
        GameManager.Instance.AddStars();
        GameManager.Instance.CompleteLevel();
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
    }
}
