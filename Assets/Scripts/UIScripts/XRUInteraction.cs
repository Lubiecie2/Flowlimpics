using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // <-- To jest potrzebne dla GraphicRaycaster
using UnityEngine.XR.Interaction.Toolkit.UI;

[RequireComponent(typeof(Canvas))]
public class XRUISetup : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        // Usuñ standardowy Graphic Raycaster jeœli istnieje
        GraphicRaycaster standardRaycaster = GetComponent<GraphicRaycaster>();
        if (standardRaycaster != null)
        {
            Destroy(standardRaycaster);
        }

        // Dodaj Tracked Device Graphic Raycaster
        if (GetComponent<TrackedDeviceGraphicRaycaster>() == null)
        {
            gameObject.AddComponent<TrackedDeviceGraphicRaycaster>();
        }

        // SprawdŸ EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<XRUIInputModule>();
            Debug.Log("Utworzono EventSystem z XRUIInputModule");
        }
    }
}