using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRCloseButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onButtonPressed;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private AudioClip clickSound;

    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }

        var interactable = gameObject.AddComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnButtonClick);
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void OnButtonClick(SelectEnterEventArgs args)
    {
        onButtonPressed?.Invoke();

        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (meshRenderer != null && highlightMaterial != null)
        {
            meshRenderer.material = highlightMaterial;
        }
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (meshRenderer != null && originalMaterial != null)
        {
            meshRenderer.material = originalMaterial;
        }
    }
}