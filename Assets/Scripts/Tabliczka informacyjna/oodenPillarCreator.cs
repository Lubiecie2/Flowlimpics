using UnityEngine;

public class WoodenPillarCreator : MonoBehaviour
{
    [Header("Pillar Settings")]
    [SerializeField] private float pillarWidth = 0.3f;
    [SerializeField] private float pillarHeight = 5f;
    [SerializeField] private float pillarDepth = 0.3f;

    [Header("Material")]
    [SerializeField] private Material woodMaterial;

    void Start()
    {
        CreatePillar();
    }

    void CreatePillar()
    {
        GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pillar.name = "Wooden_Pillar";
        pillar.transform.position = transform.position;
        pillar.transform.localScale = new Vector3(pillarWidth, pillarHeight, pillarDepth);
        pillar.transform.SetParent(transform);

        // Dodaj materia³ jeœli jest
        if (woodMaterial != null)
        {
            Renderer renderer = pillar.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = woodMaterial;
            }
        }

        Debug.Log("S³upek drewniany stworzony!");
    }

    [ContextMenu("Usuñ S³upek")]
    void RemovePillar()
    {
        Transform pillar = transform.Find("Wooden_Pillar");
        if (pillar != null)
        {
            DestroyImmediate(pillar.gameObject);
        }
    }
}