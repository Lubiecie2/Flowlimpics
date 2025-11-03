using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class VRTeleportToScene : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string targetSceneName = "MiniGameScene";

    [Header("Teleport Settings")]
    [SerializeField] private float autoTeleportDelay = 1.5f;

    [Header("Visual Feedback")]
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private GameObject teleportEffect;

    private Material originalMaterial;
    private Renderer objectRenderer;
    private bool playerOnPlatform = false;
    private Coroutine teleportCoroutine;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }

        if (teleportEffect != null)
        {
            teleportEffect.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<XROrigin>() != null)
        {
            playerOnPlatform = true;

            if (highlightMaterial != null && objectRenderer != null)
            {
                objectRenderer.material = highlightMaterial;
            }

            if (teleportEffect != null)
            {
                teleportEffect.SetActive(true);
            }

            Debug.Log("Gracz wszed³ na platformê teleportacji");

            teleportCoroutine = StartCoroutine(AutoTeleport());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<XROrigin>() != null)
        {
            playerOnPlatform = false;

            if (teleportCoroutine != null)
            {
                StopCoroutine(teleportCoroutine);
            }

            if (originalMaterial != null && objectRenderer != null)
            {
                objectRenderer.material = originalMaterial;
            }

            if (teleportEffect != null)
            {
                teleportEffect.SetActive(false);
            }

            Debug.Log("Gracz zszed³ z platformy");
        }
    }

    private System.Collections.IEnumerator AutoTeleport()
    {
        yield return new WaitForSeconds(autoTeleportDelay);

        if (playerOnPlatform)
        {
            TeleportToScene();
        }
    }

    public void TeleportToScene()
    {
        Debug.Log($"Teleportacja do sceny: {targetSceneName}");
        SceneManager.LoadScene(targetSceneName);
    }
}