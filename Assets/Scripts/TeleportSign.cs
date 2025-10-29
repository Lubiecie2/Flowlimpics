using UnityEngine;
using TMPro;

public class TeleportSign : MonoBehaviour
{
    [Header("Sign Settings")]
    [SerializeField] private string signText = "ARENA £UCZNICTWA";
    [SerializeField] private float heightAbovePlatform = 2.5f;

    [Header("Sign Design")]
    [SerializeField] private Vector3 signSize = new Vector3(2f, 0.5f, 0.1f);
    [SerializeField] private Color signColor = new Color(0.1f, 0.1f, 0.2f);
    [SerializeField] private Color textColor = Color.cyan;
    [SerializeField] private Material signMaterial;
    [SerializeField] private float textSize = 1.5f;

    [Header("Animation")]
    [SerializeField] private bool rotateSign = true;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool floatAnimation = true;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 0.2f;

    [Header("Effects")]
    [SerializeField] private bool addGlow = true;
    [SerializeField] private float glowIntensity = 2f;

    private GameObject signObject;
    private GameObject textObject;
    private Vector3 startPosition;
    private float timeCounter = 0f;

    private void Start()
    {
        CreateSign();
    }

    private void CreateSign()
    {
        // G³ówny obiekt znaku
        signObject = new GameObject("TeleportSign");
        signObject.transform.SetParent(transform);
        signObject.transform.localPosition = Vector3.up * heightAbovePlatform;
        startPosition = signObject.transform.localPosition;

        // T³o znaku (Cube/Panel)
        GameObject background = GameObject.CreatePrimitive(PrimitiveType.Cube);
        background.name = "Background";
        background.transform.SetParent(signObject.transform);
        background.transform.localPosition = Vector3.zero;
        background.transform.localScale = signSize;

        // Usuñ collider
        Destroy(background.GetComponent<Collider>());

        // Materia³ t³a
        Renderer bgRenderer = background.GetComponent<Renderer>();
        if (signMaterial != null)
        {
            bgRenderer.material = signMaterial;
        }
        else
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = signColor;

            if (addGlow)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", signColor * 0.5f);
            }

            bgRenderer.material = mat;
        }

        // Ramka (opcjonalna)
        CreateFrame(signObject);

        // Tekst 3D na przodzie
        CreateText3D(signObject);

        // Tekst 3D na tyle (¿eby by³o widaæ z obu stron)
        CreateText3D(signObject, true);
    }

    private void CreateFrame(GameObject parent)
    {
        // Górna belka
        CreateFramePart(parent, new Vector3(0, signSize.y / 2, 0), new Vector3(signSize.x + 0.1f, 0.05f, 0.05f));
        // Dolna belka
        CreateFramePart(parent, new Vector3(0, -signSize.y / 2, 0), new Vector3(signSize.x + 0.1f, 0.05f, 0.05f));
        // Lewa belka
        CreateFramePart(parent, new Vector3(-signSize.x / 2, 0, 0), new Vector3(0.05f, signSize.y, 0.05f));
        // Prawa belka
        CreateFramePart(parent, new Vector3(signSize.x / 2, 0, 0), new Vector3(0.05f, signSize.y, 0.05f));
    }

    private void CreateFramePart(GameObject parent, Vector3 position, Vector3 scale)
    {
        GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frame.transform.SetParent(parent.transform);
        frame.transform.localPosition = position;
        frame.transform.localScale = scale;
        Destroy(frame.GetComponent<Collider>());

        Renderer renderer = frame.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = Color.white;
        mat.SetFloat("_Metallic", 0.8f);

        if (addGlow)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", textColor * glowIntensity);
        }

        renderer.material = mat;
    }

    private void CreateText3D(GameObject parent, bool isBack = false)
    {
        GameObject textObj = new GameObject(isBack ? "Text_Back" : "Text_Front");
        textObj.transform.SetParent(parent.transform);

        float zOffset = signSize.z / 2 + 0.01f;
        textObj.transform.localPosition = new Vector3(0, 0, isBack ? -zOffset : zOffset);

        // POPRAWIONE OBROTY
        if (isBack)
        {
            textObj.transform.localRotation = Quaternion.Euler(0, 0, 0); // Ty³ normalnie
        }
        else
        {
            textObj.transform.localRotation = Quaternion.Euler(0, 180, 0); // Przód odwrócony
        }

        // NAJPIERW utwórz TextMeshPro
        TextMeshPro text3D = textObj.AddComponent<TextMeshPro>();
        text3D.text = signText;
        text3D.fontSize = textSize;
        text3D.color = textColor;
        text3D.alignment = TextAlignmentOptions.Center;
        text3D.fontStyle = FontStyles.Bold;

        // Rozmiar tekstu
        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(signSize.x * 0.9f, signSize.y * 0.9f);

        // Outline
        text3D.outlineWidth = 0.1f;
        text3D.outlineColor = Color.black;

        // Emisja (œwiecenie)
        if (addGlow)
        {
            text3D.fontMaterial.EnableKeyword("_EMISSION");
            text3D.fontMaterial.SetColor("_EmissionColor", textColor * glowIntensity);
        }
    }

    private void Update()
    {
        if (signObject == null) return;

        // Rotacja
        if (rotateSign)
        {
            signObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Animacja unoszenia
        if (floatAnimation)
        {
            timeCounter += Time.deltaTime * floatSpeed;
            float offsetY = Mathf.Sin(timeCounter) * floatHeight;
            signObject.transform.localPosition = startPosition + Vector3.up * offsetY;
        }
    }
}