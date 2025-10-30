using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRNotificationPanel : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private float displayDelay = 1f;

    [Header("Optional: Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip notificationSound;

    private bool hasBeenShown = false;

    private void Start()
    {
        if (notificationPanel != null)
        {
            // Upewnij si�, �e panel jest ukryty na pocz�tku
            notificationPanel.SetActive(false);

            // Poka� powiadomienie po op�nieniu
            Invoke(nameof(ShowNotification), displayDelay);
        }
        else
        {
            Debug.LogError("Notification Panel nie zosta� przypisany!");
        }
    }

    private void ShowNotification()
    {
        if (!hasBeenShown && notificationPanel != null)
        {
            notificationPanel.SetActive(true);
            hasBeenShown = true;

            // Opcjonalnie odtw�rz d�wi�k
            if (audioSource != null && notificationSound != null)
            {
                audioSource.PlayOneShot(notificationSound);
            }
        }
    }

    public void CloseNotification()
    {
        if (notificationPanel != null)
        {
            // Animacja fadeout (opcjonalnie)
            StartCoroutine(FadeOutAndClose());
        }
    }

    private System.Collections.IEnumerator FadeOutAndClose()
    {
        // Opcjonalna animacja
        yield return new WaitForSeconds(0.2f);
        notificationPanel.SetActive(false);
    }
}