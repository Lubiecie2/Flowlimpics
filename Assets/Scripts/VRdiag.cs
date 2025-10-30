using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class FullDiagnostics : MonoBehaviour
{
    void Start()
    {
        Debug.Log("========== FULL DIAGNOSTICS ==========");

        // EventSystem check
        if (EventSystem.current != null)
        {
            Debug.Log(" EventSystem EXISTS");
            Debug.Log($"   Enabled: {EventSystem.current.enabled}");
            Debug.Log($"   InputModule: {EventSystem.current.currentInputModule?.GetType().Name}");
        }
        else
        {
            Debug.LogError(" NO EventSystem!");
        }

        // Camera check
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Debug.Log($" Main Camera: {mainCam.name}");
        }
        else
        {
            Debug.LogError(" NO Main Camera!");
        }

        // Canvas check
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Debug.Log($" Canvas: {canvas.name}");
            Debug.Log($"   RenderMode: {canvas.renderMode}");
            Debug.Log($"   WorldCamera: {(canvas.worldCamera != null ? canvas.worldCamera.name : "NULL!")}");

            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                Debug.Log($" GraphicRaycaster exists");
                Debug.Log($"   BlockingObjects: {raycaster.blockingObjects}");
            }
        }
    }

    void Update()
    {
        // Raycast test on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("====== MOUSE CLICK ======");

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log($"Mouse pos: {Input.mousePosition}");
            Debug.Log($"Found {results.Count} objects:");

            foreach (RaycastResult result in results)
            {
                Debug.Log($"  -> {result.gameObject.name}");
            }
        }
    }
}