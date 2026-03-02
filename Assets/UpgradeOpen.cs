using UnityEngine;

public class UpgradeOpen : MonoBehaviour
{
    public GameObject upgrade;
    public void Open()
    {
        upgrade.SetActive(true);
        Time.timeScale = 0f;
    }
}
