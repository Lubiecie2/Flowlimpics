using UnityEngine;
using TMPro;

public class TeleportLabel : MonoBehaviour
{
    [Header("Label Settings")]
    [SerializeField] private string labelText = "Arena Testowa";
    [SerializeField] private float heightAbovePlatform = 2f;
    [SerializeField] private Vector2 canvasSize = new Vector2(3f, 1f);

    [Header("Text Settings")]
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private int fontSize = 48;
    [SerializeField] private bool addGlow = true;

    private GameObject labelCanvas;

    private void Start()
    {
        CreateLabel();
    }

    private void CreateLabel()
    {
        // Utwórz Canvas
        labelCanvas = new GameObject("TeleportLabel");
        labelCanvas.transform.SetParent(transform);
        labelCanvas.transform.localPosition = Vector3.up * heightAbovePlatform;

        Canvas canvas = labelCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        // RectTransform dla Canvas
        RectTransform canvasRect = labelCanvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = canvasSize;
        canvasRect.localScale = Vector3.one * 0.01f;

        // Dodaj CanvasScaler dla lepszej jakoœci
        var scaler = labelCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10;

        // Utwórz TextMeshPro
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(labelCanvas.transform);

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = labelText;
        text.fontSize = fontSize;
        text.color = textColor;
        text.alignment = TextAlignmentOptions.Center;
        text.fontStyle = FontStyles.Bold;

        // Wype³nij ca³y canvas
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        // Dodaj poœwiatê (outline/glow)
        if (addGlow)
        {
            text.outlineWidth = 0.2f;
            text.outlineColor = new Color(0, 0, 0, 0.8f);
        }
    }

    private void Update()
    {
        // Obróæ canvas w stronê kamery gracza (zawsze widoczny)
        if (labelCanvas != null && Camera.main != null)
        {
            labelCanvas.transform.LookAt(Camera.main.transform);
            labelCanvas.transform.Rotate(0, 180, 0);
        }
    }
}