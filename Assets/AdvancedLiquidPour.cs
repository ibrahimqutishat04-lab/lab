using UnityEngine;

public class AdvancedLiquidPour : MonoBehaviour
{
    [Header("Liquid & Shader")]
    public MeshRenderer liquidRenderer;
    public float minShaderFill = 0.51f;
    public float maxShaderFill = 0.6f;

    [Range(0, 1)]
    public float currentFill = 1f;

    [Header("Pour Settings")]
    public ParticleSystem pourParticles;
    public float maxPourRate = 0.1f;
    public float startPourAngle = 45f;
    public float fullPourAngle = 90f;

    [Header("Dynamic Rim")]
    public Transform openingCenter;
    public float cupRadius = 0.05f;

    // --- أضفنا إعدادات الصوت هنا ---
    [Header("Audio Settings")]
    public AudioSource pourAudioSource;
    // -------------------------------

    private Material liquidMaterial;
    private ParticleSystem.EmissionModule emissionModule;

    private float initialFill;

    void Start()
    {
        if (liquidRenderer != null)
            liquidMaterial = liquidRenderer.material;

        if (pourParticles != null)
            emissionModule = pourParticles.emission;

        initialFill = currentFill;

        UpdateShader();
    }

    void Update()
    {
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);
        float pourIntensity = Mathf.InverseLerp(startPourAngle, fullPourAngle, tiltAngle);

        if (pourIntensity > 0 && currentFill > 0)
        {
            MoveEmitterToLowestEdge();
            DoPour(pourIntensity);
            UpdateShader();
        }
        else
        {
            StopPour();
        }
    }

    void DoPour(float intensity)
    {
        currentFill -= intensity * maxPourRate * Time.deltaTime;
        currentFill = Mathf.Clamp01(currentFill);

        if (!pourParticles.isPlaying)
            pourParticles.Play();

        emissionModule.rateOverTime = intensity * 150f;

        // --- تشغيل صوت السكب وتعديل شدته ---
        if (pourAudioSource != null)
        {
            if (!pourAudioSource.isPlaying)
            {
                pourAudioSource.Play();
            }
            // تعديل ارتفاع الصوت بناءً على كمية السائل المنسكبة
            pourAudioSource.volume = intensity;
        }
        // ------------------------------------
    }

    void StopPour()
    {
        if (pourParticles != null && pourParticles.isPlaying)
            pourParticles.Stop();

        // --- إيقاف الصوت عند التوقف عن السكب ---
        if (pourAudioSource != null && pourAudioSource.isPlaying)
        {
            pourAudioSource.Stop();
        }
        // ----------------------------------------
    }

    void MoveEmitterToLowestEdge()
    {
        if (openingCenter == null || pourParticles == null) return;

        Vector3 localDown = transform.InverseTransformDirection(Vector3.down);
        localDown.y = 0;
        localDown.Normalize();

        Vector3 lowestEdgePoint = openingCenter.position + transform.TransformDirection(localDown * cupRadius);

        pourParticles.transform.position = lowestEdgePoint;
        pourParticles.transform.rotation = Quaternion.LookRotation(Vector3.down);
    }

    void UpdateShader()
    {
        if (liquidMaterial != null)
        {
            float fillValue = Mathf.Lerp(minShaderFill, maxShaderFill, currentFill);
            liquidMaterial.SetFloat("_Fill", fillValue);
        }
    }

    public void ResetPourState()
    {
        currentFill = initialFill;
        StopPour();
        UpdateShader();
    }
}