using UnityEngine;

public class stat_upgrade_intractable : MonoBehaviour
{
    public GameObject upgrade_station_ui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgrade_station_ui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            upgrade_station_ui.SetActive(true);

        }



    }

}
