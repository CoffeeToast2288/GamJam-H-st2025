using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBench : MonoBehaviour
{
    public GameObject UpgradeUi;
    public Button  activate;
    public GameObject activefather;
    public retern_to_safe GoBack;


    public void Start()
    {
        activate.onClick.AddListener(showUi);
    }
    private void Update()
    {
        if(GoBack.isInSafeZone == true)
        {
            activefather.SetActive(true);


        }
        if (GoBack.isInSafeZone == false)
        {
            activefather.SetActive(false);


        }
    }
    void showUi()
    {
        UpgradeUi.SetActive(true);


    }
}
