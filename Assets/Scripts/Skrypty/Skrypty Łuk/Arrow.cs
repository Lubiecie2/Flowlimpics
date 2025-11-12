using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _maxLifetime = 10f;
    [SerializeField] private float _launchDelay = 0.3f;
    [SerializeField] private float _stickTime = 30f;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _hasHit = false;
    private bool _inFlight = false;
    private Quaternion _impactRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (_inFlight && !_hasHit)
        {
            AlignToVelocity();
        }
    }

    public void Launch(Vector3 velocity)
    {
        _inFlight = true;
        _hasHit = false;

        if (_collider != null)
        {
            _collider.enabled = false;
        }

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.angularDamping = 100f;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        transform.rotation = Quaternion.LookRotation(velocity);
        _rigidbody.linearVelocity = velocity;

        StartCoroutine(DestroyIfNotHit());
        StartCoroutine(EnableColliderAfterDelay());
    }

    private IEnumerator DestroyIfNotHit()
    {
        yield return new WaitForSeconds(_maxLifetime);

        if (!_hasHit)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        yield return new WaitForSeconds(_launchDelay);
        if (_collider != null && !_hasHit)
        {
            _collider.enabled = true;
        }
    }

    private void AlignToVelocity()
    {
        if (_rigidbody.linearVelocity.magnitude > 0.5f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rigidbody.linearVelocity);
            transform.rotation = targetRotation;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit) return;

        _impactRotation = transform.rotation;
        _hasHit = true;
        _inFlight = false;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        Vector3 worldPos = transform.position;
        transform.SetParent(collision.transform);
        transform.position = worldPos;
        transform.rotation = _impactRotation;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"Hit enemy for {_damage} damage!");

            TargetHit target = collision.gameObject.GetComponent<TargetHit>();
            if (target != null)
            {
                target.OnArrowHit(_damage);
            }
        }

        Destroy(gameObject, _stickTime);
    }
}