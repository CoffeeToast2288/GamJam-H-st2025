using UnityEngine;
using UnityEngine.UI;

public class UpgradeBench : MonoBehaviour
{
    public GameObject UpgradeUi;
    public GameObject  activate;
    public retern_to_safe GoBack;

    private void Update()
    {
        if(GoBack.isInSafeZone == true)
        {
            activate.SetActive(true);


        }
        if (GoBack.isInSafeZone == false)
        {
            activate.SetActive(false);


        }
    }
    void showUi()
    {
        UpgradeUi.SetActive(true);


    }
}
