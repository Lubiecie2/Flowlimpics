using UnityEngine;

public class SignEffects : MonoBehaviour
{
    [Header("Efekt Wybierz Jeden")]
    [SerializeField] private bool pulsowanie = true;
    [SerializeField] private bool obracanie = false;
    [SerializeField] private bool kolysanie = false;
    [SerializeField] private bool lewitacja = false;

    [Header("Śledzenie Gracza")]
    [SerializeField] private bool sledzGracza = true;
    [SerializeField] private string tagGracza = "MainCamera"; // lub "Player"
    [SerializeField] private float predkoscObracania = 5f;
    [SerializeField] private bool blokowanieTylu = true; // Zapobiega odwracaniu tyłem

    [Header("Intensywność")]
    [Range(0.5f, 3f)]
    [SerializeField] private float predkosc = 1.5f;

    [Range(0.01f, 0.1f)]
    [SerializeField] private float sila = 0.03f;

    [Header("Światło (Opcjonalne)")]
    [SerializeField] private bool dodajSwiatlo = true;
    [SerializeField] private Color kolorSwiatla = new Color(1f, 0.85f, 0.6f);

    private Vector3 startScale;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Light glow;
    private Transform gracz;

    void Start()
    {
        startScale = transform.localScale;
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;

        if (dodajSwiatlo)
            SetupLight();


        if (sledzGracza)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(tagGracza);
            if (playerObj == null)
            {

                playerObj = Camera.main?.gameObject;
            }

            if (playerObj != null)
                gracz = playerObj.transform;
        }
    }

    void Update()
    {
        float time = Time.time * predkosc;

        if (sledzGracza && gracz != null)
        {
            Vector3 kierunek = gracz.position - transform.position;
            kierunek.y = 0; 

            if (kierunek != Vector3.zero)
            {
                Quaternion docelowyObrot = Quaternion.LookRotation(kierunek);

                if (blokowanieTylu)
                {
                    float kat = Quaternion.Angle(transform.rotation, docelowyObrot);

                    if (kat < 90f)
                    {
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            docelowyObrot,
                            Time.deltaTime * predkoscObracania
                        );
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        docelowyObrot,
                        Time.deltaTime * predkoscObracania
                    );
                }
            }
        }

        if (pulsowanie)
        {
            float pulse = 1f + Mathf.Sin(time) * sila;
            transform.localScale = startScale * pulse;

            if (glow != null)
                glow.intensity = 1f + Mathf.Sin(time) * 0.15f;
        }

        if (obracanie)
        {
            transform.Rotate(Vector3.up, predkosc * 20f * Time.deltaTime);
        }

        if (kolysanie)
        {
            float swing = Mathf.Sin(time) * sila * 10f;

            if (!sledzGracza) 
            {
                transform.localRotation = startRotation * Quaternion.Euler(0, 0, swing);
            }
        }

        if (lewitacja)
        {
            float hover = Mathf.Sin(time) * sila;
            transform.localPosition = startPosition + new Vector3(0, hover, 0);
        }
    }

    void SetupLight()
    {
        GameObject lightObj = new GameObject("SignGlow");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = new Vector3(0, 0, -0.3f);

        glow = lightObj.AddComponent<Light>();
        glow.type = LightType.Point;
        glow.color = kolorSwiatla;
        glow.range = 3f;
        glow.intensity = 1.2f;
        glow.shadows = LightShadows.None;
    }
}