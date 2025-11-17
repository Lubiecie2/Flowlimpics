using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetZone : MonoBehaviour
{
    [Header("Reset Settings")]
    [SerializeField] private float _resetDelay = 0.5f;
    [SerializeField] private bool _showDebugMessage = true;

    private bool _isResetting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isResetting) return;

        if (other.CompareTag("Player") || other.name.Contains("XR Origin") || other.name.Contains("Camera"))
        {
            if (_showDebugMessage)
            {
                Debug.Log("Gracz wszed³ na platformê resetuj¹c¹! Prze³adowujê scenê...");
            }

            _isResetting = true;
            Invoke(nameof(ResetScene), _resetDelay);
        }
    }

    private void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}