using UnityEngine;
using System.Collections.Generic;

public class LiquidReceiver : MonoBehaviour
{
    [Header("Liquid Visuals (Shader)")]
    public MeshRenderer liquidRenderer;

    public float minShaderFill = 0.51f;
    public float maxShaderFill = 0.6f;

    [Range(0, 1)]
    public float currentFill = 0f;
    private float targetFill = 0f;

    [Header("Fill Settings")]
    public float fillSpeed = 0.005f;
    public float smoothSpeed = 2f;

    [Header("Valid Pour Area")]
    public Transform topCenter;
    public float validRadius = 0.08f;

    [Header("Reaction")]
    public LabReactionController reactionController;

    [Header("Bubbles Audio Settings")]
    public AudioSource bubblesAudioSource;
    public ParticleSystem bubbleParticles;

    [Header("UI Panels For Muting")]
    public GameObject pauseMenuPanel;

    private Material liquidMaterial;
    private List<ParticleCollisionEvent> collisionEvents;
    private bool reactionStarted = false;

    void Start()
    {
        if (liquidRenderer != null)
        {
            liquidMaterial = liquidRenderer.material;
        }

        collisionEvents = new List<ParticleCollisionEvent>();
        UpdateShader();
    }

    void Update()
    {
        if (Mathf.Abs(currentFill - targetFill) > 0.0001f)
        {
            currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * smoothSpeed);
            UpdateShader();
        }

        bool isMenuOpen = false;
        if (pauseMenuPanel != null && pauseMenuPanel.activeInHierarchy) isMenuOpen = true;
        if (Time.timeScale == 0f) isMenuOpen = true;

        if (bubbleParticles != null && bubblesAudioSource != null)
        {
            if (isMenuOpen)
            {
                if (bubblesAudioSource.isPlaying) bubblesAudioSource.Pause();
            }
            else
            {
                // إرجاع الكود الخاص بك الذي كان يعمل بنجاح
                if (bubbleParticles.isPlaying && !bubblesAudioSource.isPlaying)
                    bubblesAudioSource.Play();
                else if (!bubbleParticles.isPlaying && bubblesAudioSource.isPlaying)
                    bubblesAudioSource.Stop();
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("LiquidPour")) return;

        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        if (ps == null) return;

        int numCollisionEvents = ps.GetCollisionEvents(gameObject, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 hitPoint = collisionEvents[i].intersection;
            Vector3 offset = hitPoint - topCenter.position;

            if (offset.sqrMagnitude <= (validRadius * validRadius))
            {
                targetFill += fillSpeed;
                targetFill = Mathf.Clamp01(targetFill);

                if (!reactionStarted && reactionController != null)
                {
                    reactionController.StartChemicalEffect();
                    reactionStarted = true;
                }
            }
        }
    }

    void UpdateShader()
    {
        if (liquidMaterial != null && liquidRenderer != null)
        {
            if (currentFill <= 0.001f)
            {
                liquidRenderer.enabled = false;
            }
            else
            {
                liquidRenderer.enabled = true;
                float fillValue = Mathf.Lerp(minShaderFill, maxShaderFill, currentFill);
                liquidMaterial.SetFloat("_Fill", fillValue);
            }
        }
    }

    public void ResetLiquidState()
    {
        currentFill = 0f;
        targetFill = 0f;
        reactionStarted = false;

        if (liquidRenderer != null) liquidRenderer.enabled = false;

        if (bubblesAudioSource != null) bubblesAudioSource.Stop();

        // --- التعديل هنا: مسح جسيمات الكوب فوراً ---
        if (bubbleParticles != null)
        {
            bubbleParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        // ------------------------------------------

        UpdateShader();
    }
}