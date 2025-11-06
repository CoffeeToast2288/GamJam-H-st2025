using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public Animator death_animation;
    public string animation_name;
    private int layer = 0;
    public float radius = 1f;
    public float damage = 5f;

    void Start()
    {
        // Damage enemies in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            Enemy_Script enemy = hit.GetComponent<Enemy_Script>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }


    }


    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = death_animation.GetCurrentAnimatorStateInfo(layer);

        if (info.IsName(animation_name))
        {
            // Check if animation has finished (normalizedTime > 1.0)
            if (info.normalizedTime >= 1.0f)
            {
                Destroy(transform.gameObject);
            }
        }

    }
}
