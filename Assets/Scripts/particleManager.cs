using UnityEngine;

<<<<<<< HEAD
public class particleManager : MonoBehaviour
=======
public class ParticleManager : MonoBehaviour
>>>>>>> parent of 246c85b (O resto)
{
    public GameObject Spawn;

    void FixedUpdate()
    {
<<<<<<< HEAD
        Spawn = GameObject.Find("Player"+PlayerPrefs.GetInt("Skin"));
=======
        Spawn = GameObject.FindGameObjectWithTag("Player");
>>>>>>> parent of 246c85b (O resto)

        transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
    }
}
