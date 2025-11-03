using UnityEngine;

public class retern_to_safe : MonoBehaviour
{
    public Vector2 safe_zone;
    public Transform self;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            retern_to_safezone();
        }
    }

    public void retern_to_safezone()
    {
        self.position = safe_zone;

    }
}
