using UnityEngine;

public class UpgradeOpen : MonoBehaviour
{
    public bool hasOpened;
    public GameObject upgrade;
    [SerializeField] upgrades what;
    public void Open()
    {
        if (!hasOpened)
        {
            what.Runit();
            hasOpened = true;
            upgrade.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
