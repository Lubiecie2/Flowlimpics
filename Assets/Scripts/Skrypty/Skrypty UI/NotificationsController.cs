using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NotificationController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject notificationPanel;
    public Button closeButton;
    public Canvas canvas;

    [Header("Audio")]
    public AudioClip openSound;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Header("Timing")]
    public float delayBeforeShow = 3f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    void Start()
    {
        AutoConfigureVR();

        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }

        Invoke("ShowNotificationWithSound", delayBeforeShow);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            closeButton.interactable = true;

            var buttonImage = closeButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.raycastTarget = true;
            }
        }
    }

    void AutoConfigureVR()
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;

            if (canvas.worldCamera == null)
            {
                canvas.worldCamera = Camera.main;
            }
        }
    }

    void ShowNotificationWithSound()
    {
        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, Camera.main.transform.position, volume);
        }

        ShowNotification();
    }

    void OnCloseButtonClicked()
    {
        CloseNotification();
    }

    public void ShowNotification()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(true);
            if (showDebugLogs)
                Debug.Log("Notification shown");
        }
    }

    public void CloseNotification()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
            if (showDebugLogs)
                Debug.Log("Notification closed");
        }
    }
}