using UnityEngine;


public class RayDebug : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    void Start()
    {
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
    }

    void Update()
    {
        if (rayInteractor != null)
        {
            // Sprawdz czy ray w ogole dziala
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Debug.Log("Ray hitting 3D object: " + hit.collider.name + " at distance: " + hit.distance);
            }

            // Sprawdz UI raycast
            if (rayInteractor.TryGetCurrentUIRaycastResult(out var uiHit))
            {
                Debug.Log("Ray hitting UI: " + uiHit.gameObject.name);
            }
            else
            {
                // Ray nie trafia w UI - sprawdz dlaczego
                Debug.Log("Ray origin: " + rayInteractor.rayOriginTransform.position);
                Debug.Log("Ray direction: " + rayInteractor.rayOriginTransform.forward);
            }
        }
    }
}