using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WelcomeWindow : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Text welcomeText;

    [Header("Settings")]
    [SerializeField] private bool showOnStart = true;
    [SerializeField] private float autoCloseTime = 5f; // Czas w sekundach

    private void Start()
    {
        // Ustaw tekst powitalny
        if (welcomeText != null)
        {
            welcomeText.text = "Witaj w �wiecie VR!\n\nTo okno zamknie si� automatycznie za 30 sekund.";
        }

        // Pod��cz funkcj� zamykania do przycisku (opcjonalnie)
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseWindow);
            Debug.Log("Przycisk zamykania pod��czony");
        }

        // Poka� okno i uruchom timer
        if (showOnStart)
        {
            ShowWindow();
            StartCoroutine(AutoCloseTimer());
        }
        else
        {
            HideWindow();
        }
    }

    private IEnumerator AutoCloseTimer()
    {
        Debug.Log($"Timer uruchomiony - okno zamknie si� za {autoCloseTime} sekund");

        yield return new WaitForSeconds(autoCloseTime);

        Debug.Log("Czas min�� - automatyczne zamykanie okna");
        CloseWindow();
    }

    public void ShowWindow()
    {
        if (windowPanel != null)
        {
            windowPanel.SetActive(true);
            Debug.Log("Okno pokazane");
        }
        else
        {
            Debug.LogError("Nie przypisano windowPanel!");
        }
    }

    public void CloseWindow()
    {
        Debug.Log("ZAMYKANIE OKNA");

        if (windowPanel != null)
        {
            windowPanel.SetActive(false);
            Debug.Log("Okno zamkni�te");
        }
        else
        {
            Debug.LogError("windowPanel jest null!");
        }

        // Opcjonalnie: zniszcz ca�y obiekt
        // Destroy(gameObject, 0.5f);
    }

    public void HideWindow()
    {
        if (windowPanel != null)
        {
            windowPanel.SetActive(false);
        }
    }
}