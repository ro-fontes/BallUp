using UnityEngine;
using Photon.Pun;

public class ParticleManager : MonoBehaviourPunCallbacks
{
    public GameObject Spawn;
    int x = 0;

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        x++;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        x--;
    }

    void FixedUpdate()
    {
        Spawn = GameObject.Find("Player" + x);

        transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
    }
}
