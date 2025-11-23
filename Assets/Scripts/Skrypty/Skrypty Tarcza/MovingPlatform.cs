using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveDistance = 2f; 
    [SerializeField] private float _moveSpeed = 1f; 
    [SerializeField] private bool _startMovingUp = true; 

    [Header("Movement Type")]
    [SerializeField] private MovementType _movementType = MovementType.PingPong;

    private Vector3 _startPosition;
    private float _time;

    public enum MovementType
    {
        PingPong,    
        Smooth,      
        EaseInOut    
    }

    private void Start()
    {
        _startPosition = transform.position;
        _time = _startMovingUp ? 0f : Mathf.PI;
    }

    private void Update()
    {
        _time += Time.deltaTime * _moveSpeed;

        float offset = 0f;

        switch (_movementType)
        {
            case MovementType.PingPong:
                offset = Mathf.PingPong(_time, _moveDistance);
                break;

            case MovementType.Smooth:
                offset = Mathf.Sin(_time) * (_moveDistance / 2f) + (_moveDistance / 2f);
                break;

            case MovementType.EaseInOut:
                float t = Mathf.PingPong(_time, 1f);
                t = t * t * (3f - 2f * t); 
                offset = t * _moveDistance;
                break;
        }

        transform.position = _startPosition + Vector3.up * offset;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = Application.isPlaying ? _startPosition : transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPos, startPos + Vector3.up * _moveDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPos, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPos + Vector3.up * _moveDistance, 0.2f);
    }
}