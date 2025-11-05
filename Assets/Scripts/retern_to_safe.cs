using UnityEngine;

public class retern_to_safe : MonoBehaviour
{
    [Header("Safe Zone Settings")]
    public Vector2 safe_zone;
    public Transform self;

    [HideInInspector] public bool isInSafeZone = false;

    // Teleports to safe zone and activates safe mode
    public void EnterSafeZone()
    {
        if (self != null)
        {
            self.position = safe_zone;
            isInSafeZone = true;
            Debug.Log($"{self.name} entered safe zone!");
        }
    }

    // Call this when player decides to continue (e.g., pressing Enter)
    public void ExitSafeZone()
    {
        isInSafeZone = false;
        Debug.Log($"{self.name} exited safe zone!");
    }
}