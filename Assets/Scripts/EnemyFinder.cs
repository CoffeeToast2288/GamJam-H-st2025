using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to a UI arrow prefab (Image with arrow graphic).
/// Positions itself on the screen edge and rotates to point from
/// the player toward the tracked enemy.
/// </summary>
public class EnemyArrowIndicator : MonoBehaviour
{
    [Tooltip("The enemy this arrow is tracking")]
    public Transform target;

    [Tooltip("The player transform")]
    public Transform player;

    [Tooltip("How far from screen center the arrow sits (in pixels)")]
    public float screenEdgePadding = 80f;

    private RectTransform rectTransform;
    private Camera mainCam;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);
        screenPos.z = 0f; // Orthographic: z is irrelevant, zero it out to keep math clean

        bool isOffScreen =
            screenPos.x < 0 || screenPos.x > Screen.width ||
            screenPos.y < 0 || screenPos.y > Screen.height;

        gameObject.SetActive(isOffScreen);
        if (!isOffScreen) return;

        // --- Direction from screen center to enemy's screen position ---
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dir = (screenPos - screenCenter).normalized;

        // --- Clamp to screen edge ---
        float padding = screenEdgePadding;
        float halfW = Screen.width / 2f - padding;
        float halfH = Screen.height / 2f - padding;

        Vector3 arrowPos = screenCenter;
        if (Mathf.Abs(dir.x) * halfH > Mathf.Abs(dir.y) * halfW)
        {
            float scale = halfW / Mathf.Abs(dir.x);
            arrowPos += dir * scale;
        }
        else
        {
            float scale = halfH / Mathf.Abs(dir.y);
            arrowPos += dir * scale;
        }

        rectTransform.position = arrowPos;

        // --- Rotate to point toward enemy ---
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}