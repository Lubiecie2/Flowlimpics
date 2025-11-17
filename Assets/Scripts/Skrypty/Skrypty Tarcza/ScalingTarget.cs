using UnityEngine;

public class ScalingTarget : MonoBehaviour
{
    [Header("Scaling Settings")]
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] private float _pulseSpeed = 2f;

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        float scale = Mathf.Lerp(_minScale, _maxScale, (Mathf.Sin(Time.time * _pulseSpeed) + 1f) / 2f);
        transform.localScale = _originalScale * scale;
    }
}