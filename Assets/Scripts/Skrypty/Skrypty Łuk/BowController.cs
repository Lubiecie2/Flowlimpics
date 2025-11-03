using UnityEngine;


public class BowController : MonoBehaviour
{
    [Header("Bow Components")]
    [SerializeField] private Transform bowString;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private LineRenderer bowStringRenderer;

    [Header("Arrow Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float maxPullDistance = 0.5f;
    [SerializeField] private float arrowForceMultiplier = 30f;

    [Header("VR Hand References")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    [SerializeField] private Transform pullHand;

    private Vector3 originalStringPosition;
    private GameObject currentArrow;
    private bool isGrabbed = false;
    private bool isPulling = false;

    void Start()
    {
        originalStringPosition = bowString.localPosition;

        // Ustaw LineRenderer dla ci�ciwy
        bowStringRenderer.positionCount = 2;
        UpdateBowString(0);
    }

    void Update()
    {
        if (isPulling && pullHand != null)
        {
            HandlePull();
        }
    }

    public void OnBowGrabbed()
    {
        isGrabbed = true;
        SpawnArrow();
    }

    public void OnBowReleased()
    {
        isGrabbed = false;
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
    }

    public void OnStringGrabbed(Transform hand)
    {
        if (isGrabbed)
        {
            pullHand = hand;
            isPulling = true;
        }
    }

    public void OnStringReleased()
    {
        if (isPulling && currentArrow != null)
        {
            FireArrow();
        }

        isPulling = false;
        pullHand = null;
        UpdateBowString(0);
    }

    private void HandlePull()
    {
        Vector3 pullPosition = transform.InverseTransformPoint(pullHand.position);
        float pullDistance = Mathf.Clamp(-pullPosition.z, 0, maxPullDistance);

        // Aktualizuj pozycj� ci�ciwy
        bowString.localPosition = new Vector3(
            originalStringPosition.x,
            originalStringPosition.y,
            originalStringPosition.z - pullDistance
        );

        // Aktualizuj pozycj� strza�y
        if (currentArrow != null)
        {
            currentArrow.transform.position = bowString.position;
        }

        UpdateBowString(pullDistance);
    }

    private void UpdateBowString(float pullDistance)
    {
        // Pozycje ko�c�w �uku (g�ra i d�)
        Vector3 topPoint = transform.TransformPoint(new Vector3(0, 0.3f, 0));
        Vector3 bottomPoint = transform.TransformPoint(new Vector3(0, -0.3f, 0));

        bowStringRenderer.SetPosition(0, topPoint);
        bowStringRenderer.SetPosition(1, bowString.position);
    }

    private void SpawnArrow()
    {
        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        currentArrow.transform.SetParent(bowString);
    }

    private void FireArrow()
    {
        float pullDistance = Vector3.Distance(bowString.position, transform.TransformPoint(originalStringPosition));
        float force = (pullDistance / maxPullDistance) * arrowForceMultiplier;

        currentArrow.transform.SetParent(null);
        Arrow arrowScript = currentArrow.GetComponent<Arrow>();

        if (arrowScript != null)
        {
            arrowScript.Launch(arrowSpawnPoint.forward, force);
        }

        currentArrow = null;
        SpawnArrow();
    }
}