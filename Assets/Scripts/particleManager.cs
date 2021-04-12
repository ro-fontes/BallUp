using UnityEngine;

public class particleManager : MonoBehaviour
{
    public GameObject Spawn;

    void Update()
    {
        Spawn = GameObject.Find("Player"+PlayerPrefs.GetInt("Skin"));

        transform.position = Spawn.transform.position - new Vector3(0, 0.55f, 0);
    }
}
