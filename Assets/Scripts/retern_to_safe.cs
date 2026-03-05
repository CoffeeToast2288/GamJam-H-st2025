using UnityEngine;

public class retern_to_safe : MonoBehaviour
{
    [Header("Safe Zone Settings")]
    public Vector2 safe_zone;
    public Vector2 returnPosition;
    public Transform self;
    public GameObject healthPack;
    public Transform healthStation;

    [HideInInspector] public bool isInSafeZone = false;

    // Teleports to safe zone and activates safe mode
    public void EnterSafeZone()
    {
        if (self != null)
        {
            self.position = safe_zone;
            isInSafeZone = true;
            // Spawn bullet toward player
            Instantiate(healthPack, healthStation.position, healthStation.rotation);
            Debug.Log($"{self.name} entered safe zone!");
        }
    }

    // Call this when player decides to continue (e.g., pressing Enter)
    public void ExitSafeZone()
    {
        isInSafeZone = false;
        ReturnToBattle();
        Debug.Log($"{self.name} exited safe zone!");
    }
    void ReturnToBattle()
    {
        isInSafeZone = false;

        // Move player back into arena
        self.position = returnPosition;

    }


}