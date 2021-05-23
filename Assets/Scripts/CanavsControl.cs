using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanavsControl : MonoBehaviourPunCallbacks
{
    public GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = null;
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Destroy(gameObject);
    }
    void LateUpdate()
    {
        this.transform.position = spawn.transform.position + new Vector3(0, 1, 0);
    }
}
