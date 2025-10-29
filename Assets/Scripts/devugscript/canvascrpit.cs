using UnityEngine;

public class ForceCanvasScaleConstant : MonoBehaviour
{
    void Awake()
    {
        transform.localScale = Vector3.one;
    }

    void Start()
    {
        transform.localScale = Vector3.one;
        Debug.Log("Canvas scale forced to 1 in Start");
    }

    void LateUpdate()
    {
        // Wymuszaj scale w ka¿dej klatce
        if (transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.one;
        }
    }
}