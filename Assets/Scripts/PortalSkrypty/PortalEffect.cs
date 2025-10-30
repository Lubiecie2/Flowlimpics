using UnityEngine;

public class PortalMovement : MonoBehaviour
{
    [Header("Ruch G�ra-D�")]
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float moveSpeed = 1f;

    [Header("�wiat�o")]
    [SerializeField] private Color lightColor = new Color(1f, 0.3f, 0f); // Pomara�czowy
    [SerializeField] private float lightIntensity = 3f;
    [SerializeField] private float lightRange = 8f;

    [Header("D�wi�k")]
    [SerializeField] private AudioClip portalSound;
    [SerializeField] private float volume = 0.5f;

    private Vector3 startPosition;
    private Light portalLight;
    private AudioSource audioSource;

    void Start()
    {
        startPosition = transform.position;

        // Dodaj �wiat�o
        GameObject lightObj = new GameObject("PortalLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.zero;

        portalLight = lightObj.AddComponent<Light>();
        portalLight.type = LightType.Point;
        portalLight.color = lightColor;
        portalLight.intensity = lightIntensity;
        portalLight.range = lightRange;

        // Dodaj d�wi�k
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = portalSound;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.maxDistance = 15f;

        if (portalSound != null)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        // Ruch g�ra-d�
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPosition + new Vector3(0, offset, 0);
    }
}