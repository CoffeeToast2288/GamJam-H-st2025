using UnityEngine;

public class UpgradeOpen : MonoBehaviour
{
    public bool hasOpened;
    public GameObject upgrade;
    public void Open()
    {
        if (!hasOpened)
        {
            hasOpened = true;
            upgrade.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
