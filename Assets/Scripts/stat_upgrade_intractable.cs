using UnityEngine;

public class stat_upgrade_intractable : MonoBehaviour
{
    public GameObject upgrade_station_ui;
    public bool has_entered, is_open;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgrade_station_ui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (has_entered && Input.GetKeyDown(KeyCode.F)&& !is_open)
        {
            upgrade_station_ui.SetActive(true);
            is_open = true;

        }
        else if (has_entered && Input.GetKeyDown(KeyCode.F)&& is_open)
        {
            is_open = false;
            upgrade_station_ui.SetActive(true);

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            has_entered = true;

        }



    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            has_entered = false;

        }



    }

}
