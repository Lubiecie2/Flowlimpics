using UnityEngine;
using UnityEngine.UI;

public class CanvasDebugInfo : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        Debug.Log("===== CANVAS INFO =====");
        Debug.Log("Canvas name: " + gameObject.name);
        Debug.Log("Canvas position: " + transform.position);
        Debug.Log("Canvas rotation: " + transform.rotation.eulerAngles);
        Debug.Log("Canvas LOCAL scale: " + transform.localScale);
        Debug.Log("Canvas LOSSY scale: " + transform.lossyScale);
        Debug.Log("Canvas active: " + gameObject.activeInHierarchy);
        Debug.Log("Canvas layer: " + LayerMask.LayerToName(gameObject.layer));

        if (canvas != null)
        {
            Debug.Log("Render Mode: " + canvas.renderMode);
            Debug.Log("World Camera: " + (canvas.worldCamera != null ? canvas.worldCamera.name : "NULL"));
            Debug.Log("Plane Distance: " + canvas.planeDistance);
        }

        Camera cam = Camera.main;
        if (cam != null)
        {
            float distance = Vector3.Distance(transform.position, cam.transform.position);
            Debug.Log("Distance from Main Camera: " + distance);
        }

        var raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster != null)
        {
            Debug.Log("Has GraphicRaycaster: YES");
            Debug.Log("Raycaster enabled: " + raycaster.enabled);
        }
        else
        {
            Debug.LogError("Has GraphicRaycaster: NO - THIS IS PROBLEM!");
        }

        // Check parent scale
        if (transform.parent != null)
        {
            Debug.Log("PARENT " + transform.parent.name + " scale: " + transform.parent.localScale);
        }
    }
}