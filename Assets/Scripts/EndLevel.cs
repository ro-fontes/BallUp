using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    float secondsCount;
    int starsCouter;

    void Start()
    {
        starsCouter = 3;
    }
    void Update()
    {
        secondsCount += Time.deltaTime;
        if(secondsCount <= 13)
        {
            starsCouter = 3;
        }
        else if(secondsCount <= 19)
        {
            starsCouter = 2;
        }
        else
        {
            starsCouter = 1;
        }
    }

    void OnTriggerEnter()
    {
        GameManager.Instance.AddStars(starsCouter);
        GameManager.Instance.CompleteLevel();
    }
}
