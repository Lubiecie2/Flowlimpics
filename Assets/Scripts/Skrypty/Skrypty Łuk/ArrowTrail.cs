using UnityEngine;
using System.Collections.Generic;

public class ArrowTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private float _trailWidth = 0.05f;
    [SerializeField] private int _maxPoints = 30;
    [SerializeField] private float _pointSpacing = 0.05f;
    [SerializeField] private float _fadeTime = 0.5f; 
    [SerializeField] private Color _startColor = Color.cyan;
    [SerializeField] private Color _endColor = new Color(0, 1, 1, 0);

    private LineRenderer _lineRenderer;
    private Queue<TrailPoint> _trailPoints = new Queue<TrailPoint>();
    private Vector3 _lastPosition;

    private class TrailPoint
    {
        public Vector3 position;
        public float timestamp;

        public TrailPoint(Vector3 pos, float time)
        {
            position = pos;
            timestamp = time;
        }
    }

    private void Awake()
    {
        SetupLineRenderer();
        _lastPosition = transform.position;
    }

    private void SetupLineRenderer()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = _trailWidth;
        _lineRenderer.endWidth = _trailWidth * 0.1f;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = _startColor;
        _lineRenderer.endColor = _endColor;
        _lineRenderer.numCornerVertices = 5;
        _lineRenderer.numCapVertices = 5;
        _lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lineRenderer.receiveShadows = false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, _lastPosition);

        if (distance > _pointSpacing)
        {
            _trailPoints.Enqueue(new TrailPoint(transform.position, Time.time));
            _lastPosition = transform.position;

            if (_trailPoints.Count > _maxPoints)
            {
                _trailPoints.Dequeue();
            }
        }
        RemoveOldPoints();
        UpdateTrail();
    }

    private void RemoveOldPoints()
    {
        float currentTime = Time.time;

        while (_trailPoints.Count > 0 && currentTime - _trailPoints.Peek().timestamp > _fadeTime)
        {
            _trailPoints.Dequeue();
        }
    }

    private void UpdateTrail()
    {
        _lineRenderer.positionCount = _trailPoints.Count;
        int index = 0;
        foreach (TrailPoint point in _trailPoints)
        {
            _lineRenderer.SetPosition(index, point.position);
            index++;
        }
    }

    private void OnDestroy()
    {
        if (_lineRenderer != null)
        {
            Destroy(_lineRenderer);
        }
    }
}