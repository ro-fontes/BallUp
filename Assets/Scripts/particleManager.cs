using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject Spawn;

    void FixedUpdate()
    {
        Spawn = GameObject.FindGameObjectWithTag("Player");

        transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
    }
}
