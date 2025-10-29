using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ManualUIPress : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    void Start()
    {
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        Debug.Log("ManualUIPress ready on " + gameObject.name);
    }

    void Update()
    {
        // Sprawdz trigger kontrolera
        if (rayInteractor != null)
        {
            bool triggerPressed = false;

            // Metoda 1: Klawisz Space dla testow
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                triggerPressed = true;
                Debug.Log("Space pressed");
            }

            // Metoda 2: Lewy przycisk myszy
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                triggerPressed = true;
                Debug.Log("Mouse clicked");
            }

            // Metoda 3: XR Controller trigger
            var devices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevices(devices);

            foreach (var device in devices)
            {
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerValue))
                {
                    if (triggerValue)
                    {
                        triggerPressed = true;
                        Debug.Log("XR Trigger pressed on " + device.name);
                        break;
                    }
                }
            }

            // Jesli trigger nacisniety, sprawdz ray
            if (triggerPressed)
            {
                Debug.Log("Trigger detected, checking ray...");

                if (rayInteractor.TryGetCurrentUIRaycastResult(out var raycastResult))
                {
                    Debug.Log("Ray hitting UI: " + raycastResult.gameObject.name);

                    var button = raycastResult.gameObject.GetComponent<UnityEngine.UI.Button>();
                    if (button != null && button.interactable)
                    {
                        Debug.Log("Invoking button onClick");
                        button.onClick.Invoke();
                    }
                    else
                    {
                        Debug.Log("No Button component found or not interactable");
                    }
                }
                else
                {
                    Debug.Log("Ray NOT hitting any UI");
                }
            }
        }
    }
}