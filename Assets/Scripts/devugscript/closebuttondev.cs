using UnityEngine;
using UnityEngine.UI;

public class ButtonDebugInfo : MonoBehaviour
{
    void Start()
    {
        Debug.Log("===== BUTTON INFO =====");
        Debug.Log("Button name: " + gameObject.name);
        Debug.Log("Button position: " + transform.position);
        Debug.Log("Button active: " + gameObject.activeInHierarchy);
        Debug.Log("Button layer: " + LayerMask.LayerToName(gameObject.layer));

        Image img = GetComponent<Image>();
        if (img != null)
        {
            Debug.Log("Has Image: YES, Raycast Target: " + img.raycastTarget);
        }

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            Debug.Log("Has Button: YES, Interactable: " + btn.interactable);
        }

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            Debug.Log("Parent Canvas: " + parentCanvas.name);
        }
        else
        {
            Debug.LogError("NO PARENT CANVAS - THIS IS PROBLEM!");
        }
    }
}