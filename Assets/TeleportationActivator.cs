using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportationActivator : MonoBehaviour
{

    public XRRayInteractor teleportInteractior;
    public InputActionProperty teleportActivatorAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teleportInteractior.gameObject.SetActive(false);

        teleportActivatorAction.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Teleport Triggered!");
        teleportInteractior.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportActivatorAction.action.WasReleasedThisFrame())
        {
            teleportInteractior.gameObject.SetActive(false);
        }
    }
  
}
