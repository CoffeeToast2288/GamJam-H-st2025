using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightUpgrad : MonoBehaviour
{
    public GameObject flashlight;
    public bool unlocked = false;
    public bool unlockable = true;
    private void Update()
    {
        if(unlocked == true && unlockable == true)
        {
            unlockable = false;
            upgradelight();
        }
    }

    void upgradelight()
    {
        flashlight.transform.localScale =new Vector3 (2, 2, 0);

    }

}
