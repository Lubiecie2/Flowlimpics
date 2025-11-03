using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private Rigidbody rb;

    private bool hasHit = false;

    void Awake()
    {
        // Automatycznie znajdź Rigidbody jeśli nie jest przypisane
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasHit && rb.linearVelocity != Vector3.zero)
        {
            // Obróć strzałę w kierunku lotu
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            hasHit = true;
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;

            // Przyczep strzałę do obiektu
            transform.SetParent(collision.transform);
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        rb.linearVelocity = direction * force;
    }
}