using UnityEngine;

public class water : MonoBehaviour
{
    [Header("Particle Settings")]
    public ParticleSystem waterParticles;

    [Header("Audio Settings")]
    public AudioSource waterAudioSource;
    public AudioClip waterSound;

    [Header("UI Panels For Muting")]
    public GameObject pauseMenuPanel;

    // صار public عشان أسلاك التجربة تقدر تتحقق إذا النار شغالة
    public bool isWaterOn = false;

    void Start()
    {
        if (waterAudioSource != null && waterSound != null)
        {
            waterAudioSource.clip = waterSound;
            waterAudioSource.loop = true;
            waterAudioSource.Stop();
        }
    }

    void Update()
    {
        bool isMenuOpen = false;

        if (pauseMenuPanel != null && pauseMenuPanel.activeInHierarchy)
            isMenuOpen = true;

        if (Time.timeScale == 0f)
            isMenuOpen = true;

        // التحكم بالصوت حسب حالة النار
        if (waterParticles != null && waterAudioSource != null)
        {
            if (isMenuOpen)
            {
                if (waterAudioSource.isPlaying)
                    waterAudioSource.Pause();
            }
            else
            {
                // إذا النار شغالة والصوت واقف
                if (waterParticles.isPlaying && !waterAudioSource.isPlaying)
                {
                    waterAudioSource.UnPause();
                }
                // إذا النار مطفية والصوت شغال
                else if (!waterParticles.isPlaying && waterAudioSource.isPlaying)
                {
                    waterAudioSource.Stop();
                }
            }
        }
    }

    public void ToggleWater()
    {
        isWaterOn = !isWaterOn;

        if (isWaterOn)
            StartWater();
        else
            StopWater();
    }

    public void StartWater()
    {
        // تشغيل النار
        isWaterOn = true;

        if (waterParticles != null)
        {
            waterParticles.Play();
        }

        // تشغيل الصوت
        if (waterAudioSource != null && !waterAudioSource.isPlaying)
        {
            waterAudioSource.Play();
        }
    }

    public void StopWater()
    {
        // إطفاء النار
        isWaterOn = false;

        if (waterParticles != null)
        {
            waterParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // إيقاف الصوت
        if (waterAudioSource != null)
        {
            waterAudioSource.Stop();
        }
    }

    // إعادة ضبط كاملة
    public void ResetWater()
    {
        isWaterOn = false;

        if (waterParticles != null)
        {
            waterParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (waterAudioSource != null)
        {
            waterAudioSource.Stop();
        }
    }
}