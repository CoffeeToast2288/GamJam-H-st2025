using UnityEngine;

public class Gun_Script : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject bullet;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, spawnPos.position, spawnPos.rotation);
        }
    }
}
