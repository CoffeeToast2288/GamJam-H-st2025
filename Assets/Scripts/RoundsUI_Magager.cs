using UnityEngine;
using TMPro;

/// <summary>
/// Handles all on-screen UI text for the wave system.
/// Attach this to a Canvas and link the text fields in the inspector.
/// </summary>
public class WaveUIController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI waveText;       // Shows "Wave 1", "Wave 2", etc.
    public TextMeshProUGUI messageText;    // Shows general messages like "Wave Cleared!"
    public TextMeshProUGUI continueText;   // Shows "Press ENTER to continue"

    /// <summary>
    /// Updates the wave counter UI.
    /// </summary>
    public void UpdateWaveText(int wave)
    {
        if (waveText != null)
            waveText.text = $"Wave {wave}";
    }

    /// <summary>
    /// Sets the message text (no fading).
    /// </summary>
    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            // Reset alpha to full in case it was previously faded
            Color c = messageText.color;
            c.a = 1f;
            messageText.color = c;
        }
    }

    /// <summary>
    /// Shows or hides the "Press ENTER to continue" text.
    /// </summary>
    public void ShowContinuePrompt(bool show)
    {
        if (continueText != null)
            continueText.gameObject.SetActive(show);
    }

    /// <summary>
    /// Fades out the message text smoothly over the given duration.
    /// </summary>
    public System.Collections.IEnumerator FadeOutMessage(float duration)
    {
        if (messageText == null) yield break;

        float elapsed = 0f;
        Color originalColor = messageText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure fully transparent and clear text (optional)
        messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}