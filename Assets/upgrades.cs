using System;
using UnityEngine;

public class upgrades : MonoBehaviour
{
  
    public upgrade_node[] upgrade_nodes; // these contain the values for the upgrades 
    PlayerStats stats; // this refrances the player stats
    public int[] rolls; // these determin the chance of getting what bonus 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void roll_upgrades()
    {



    }


    public void OnButtonClicked(int id)
    {
        Debug.Log($"Button {id} clicked!");


    }



}
[System.Serializable]
public class upgrade_node
{
    public string text;
    public int id;
    public GameObject button;

}
