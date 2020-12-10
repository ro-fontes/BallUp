using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public struct GhostTransform
{
    public Vector3 Position;
    public Quaternion Rotation;

    public GhostTransform(Transform transform)
    {
        this.Position = transform.position;
        this.Rotation = transform.rotation;
    }
}

public class GhostManager : MonoBehaviour
{
    public Transform Ball;
    //Transform teste;
    public Transform ghostBall;
    public bool recording;
    public bool playing;
    public int savedListCount;

    List<GhostTransform> recordedGhostTranforms = new List<GhostTransform>();
    GhostTransform LastRecordedGhostTransform;

    private void Start()
    {
        Ball = GameObject.Find("Player").transform;
    }
    private void Update()
    {

        if(recording == true)
        {
            if (Ball.position != LastRecordedGhostTransform.Position || Ball.rotation != LastRecordedGhostTransform.Rotation)
            {
                var newGhostTransform = new GhostTransform(Ball);
                recordedGhostTranforms.Add(newGhostTransform);
                LastRecordedGhostTransform = newGhostTransform;
            }
        }
        if(playing == true)
        {
            Play();
        }
    }


    public void SaveList()
    {
        print("Salvo");
        for (int i = 0; i < recordedGhostTranforms.Count; i++)
        {
            PlayerPrefs.SetFloat("TransformsX" + i, recordedGhostTranforms[i].Position.x);
            PlayerPrefs.SetFloat("TransformsY" + i, recordedGhostTranforms[i].Position.y);
            PlayerPrefs.SetFloat("TransformsZ" + i, recordedGhostTranforms[i].Position.z);

            PlayerPrefs.SetFloat("RotationX" + i, recordedGhostTranforms[i].Rotation.x);
            PlayerPrefs.SetFloat("RotationY" + i, recordedGhostTranforms[i].Rotation.y);
            PlayerPrefs.SetFloat("RotationZ" + i, recordedGhostTranforms[i].Rotation.z);
        }
        PlayerPrefs.SetInt("Count", recordedGhostTranforms.Count);
        
    }

    public void LoadList()
    {
        recordedGhostTranforms.Clear();
        savedListCount = PlayerPrefs.GetInt("Count");
        for (int i = 0; i < savedListCount; i++)
        {
            float playerX = PlayerPrefs.GetFloat("TransformsX" + i);
            float playerY = PlayerPrefs.GetFloat("TransformsY" + i);
            float playerZ = PlayerPrefs.GetFloat("TransformsZ" + i);

            float playerRotateX = PlayerPrefs.GetFloat("TransformsX" + i);
            float playerRotatey = PlayerPrefs.GetFloat("TransformsY" + i);
            float playerRotatez = PlayerPrefs.GetFloat("TransformsZ" + i);

            //teste.position = new Vector3(playerX, playerY, playerZ);
            //teste.rotation = new Quaternion(playerRotateX, playerRotatey, playerRotatez, 1f);

            //var newGhostTransformSaved = new GhostTransform(teste);

            //recordedGhostTranforms.Add(newGhostTransformSaved);
        }
        
    }
    void Play()
    {
        ghostBall.gameObject.SetActive(true);
        StartCoroutine(StartGhost());
    }
    IEnumerator StartGhost()
    {
        for(int i = 0; i < recordedGhostTranforms.Count; i++)
        {
            ghostBall.position = recordedGhostTranforms[i].Position;
            ghostBall.rotation = recordedGhostTranforms[i].Rotation;

            yield return new WaitForFixedUpdate();
        }
    }
}
