using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowGrabDetector : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;
    private bool _hasStartedGame = false;

    private void Awake()
    {
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.AddListener(OnBowGrabbed);
        }
    }

    private void OnDisable()
    {
        if (_grabInteractable != null)
        {
            _grabInteractable.selectEntered.RemoveListener(OnBowGrabbed);
        }
    }

    private void OnBowGrabbed(SelectEnterEventArgs args)
    {
        if (_hasStartedGame) return;

        _hasStartedGame = true;

        Debug.Log("�uk chwycony - gra startuje!");

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.StartGame();
        }
    }
}