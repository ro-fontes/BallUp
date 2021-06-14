using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    bool EnterEnd;
    private void Start()
    {
        EnterEnd = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !EnterEnd)
        {
            GameManager.Instance.AddStars();
            GameManager.Instance.CompleteLevel();
            EnterEnd = true;
        }
    }
}
