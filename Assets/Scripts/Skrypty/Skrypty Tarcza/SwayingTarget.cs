using UnityEngine;

public class SwayingTarget : MonoBehaviour
{
    [Header("Swaying Settings")]
    [SerializeField] private float _swayAmount = 2f;
    [SerializeField] private float _swaySpeed = 1f;
    [SerializeField] private Vector3 _swayAxis = Vector3.right;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * _swaySpeed) * _swayAmount;
        transform.position = _startPosition + (_swayAxis.normalized * offset);
    }
}