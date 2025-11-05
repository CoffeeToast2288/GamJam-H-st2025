using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public Animator death_animation;
    public string animation_name;
    private int layer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

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
