using UnityEngine;

public class TargetHit : MonoBehaviour
{
    [Header("Visual Feedback")]
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private float _flashDuration = 15.5f;

    private Renderer _renderer;
    private Color _originalColor;
    private bool _isFlashing = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
    }

    public void OnArrowHit(float damage)
    {
        Debug.Log("UDA£O CI SIÊ TRAFIÆ! Trafiono w tarczê!");

        FlashColor();
    }

    private void FlashColor()
    {
        if (_renderer == null || _isFlashing) return;

        _isFlashing = true;
        _renderer.material.color = _hitColor;

        Invoke(nameof(ResetColor), _flashDuration);
    }

    private void ResetColor()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
        _isFlashing = false;
    }
}