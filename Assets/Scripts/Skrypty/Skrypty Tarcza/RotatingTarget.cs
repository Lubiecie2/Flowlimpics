using UnityEngine;

public class RotatingTarget : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 _rotationSpeed = new Vector3(0, 90, 0);
    [SerializeField] private bool _randomDirection = false;

    private void Start()
    {
        if (_randomDirection)
        {
            _rotationSpeed *= Random.Range(-1f, 1f) > 0 ? 1 : -1;
        }
    }

    private void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime);
    }
}