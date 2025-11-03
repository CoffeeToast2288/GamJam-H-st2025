using UnityEngine;

public class Enemy_hitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

               
        }
    }
}
