using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("UI & Visuals")]
    public Slider volumeSlider;

    [Header("Audio Settings")]
    public AudioSource uiAudioSource; // مصدر الصوت
    public AudioClip sliderSound;     // صوت سحب الشريط (مثال: طقة صغيرة)

    // متغيرات للتحكم بسرعة تكرار الصوت عشان ما يصير إزعاج
    private float lastSoundTime = 0f;
    private float soundCooldown = 0.1f; // الصوت رح يشتغل كل 0.1 ثانية أثناء السحب

    void Start()
    {
        if (volumeSlider != null)
        {
            // التحقق من وجود إعدادات صوت محفوظة من قبل
            if (PlayerPrefs.HasKey("GameVolume"))
            {
                float savedVolume = PlayerPrefs.GetFloat("GameVolume");
                volumeSlider.value = savedVolume;
                SetVolume(savedVolume); // تطبيق الصوت المحفوظ
            }
            else
            {
                // مطابقة الشريط لمستوى الصوت الحالي إذا ما كان في شيء محفوظ
                volumeSlider.value = AudioListener.volume;
            }

            // ربط الشريط بتغيير الصوت بشكل تلقائي
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // دالة جديدة تنفذ لما اللاعب يغير قيمة الشريط
    private void OnSliderValueChanged(float sliderValue)
    {
        SetVolume(sliderValue);

        // تشغيل الصوت مع نظام حماية من التكرار المزعج
        if (uiAudioSource != null && sliderSound != null)
        {
            if (Time.time - lastSoundTime > soundCooldown)
            {
                uiAudioSource.PlayOneShot(sliderSound);
                lastSoundTime = Time.time;
            }
        }
    }

    public void SetVolume(float sliderValue)
    {
        // تغيير مستوى الصوت العام في اللعبة
        AudioListener.volume = sliderValue;

        // حفظ مستوى الصوت عشان يضل زي ما هو لما اللاعب يرجع يفتح اللعبة
        PlayerPrefs.SetFloat("GameVolume", sliderValue);
        PlayerPrefs.Save();
    }
}