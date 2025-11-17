using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Disappearing Settings")]
    [SerializeField] private float _visibleTime = 3f;
    [SerializeField] private float _hiddenTime = 2f;
    [SerializeField] private bool _startVisible = true;

    private Renderer _renderer;
    private Collider _collider;
    private bool _isVisible;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _isVisible = _startVisible;

        SetVisibility(_startVisible);
    }

    private void Start()
    {
        if (_startVisible)
        {
            Invoke(nameof(Hide), _visibleTime);
        }
        else
        {
            Invoke(nameof(Show), _hiddenTime);
        }
    }

    private void Hide()
    {
        SetVisibility(false);
        Invoke(nameof(Show), _hiddenTime);
    }

    private void Show()
    {
        SetVisibility(true);
        Invoke(nameof(Hide), _visibleTime);
    }

    private void SetVisibility(bool visible)
    {
        _isVisible = visible;

        if (_renderer != null)
        {
            _renderer.enabled = visible;
        }

        if (_collider != null)
        {
            _collider.enabled = visible;
        }
    }
}