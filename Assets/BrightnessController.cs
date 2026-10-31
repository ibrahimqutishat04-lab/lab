using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    [Header("UI & Visuals")]
    public Slider brightnessSlider;
    public Image brightnessPanel; // هاد رح نحط فيه الصورة السوداء اللي قدام الكاميرا

    [Header("Audio Settings")]
    public AudioSource uiAudioSource; // مصدر الصوت
    public AudioClip sliderSound;     // صوت سحب الشريط (مثال: طقة صغيرة)

    // متغيرات للتحكم بسرعة تكرار الصوت عشان ما يصير إزعاج
    private float lastSoundTime = 0f;
    private float soundCooldown = 0.1f; // الصوت رح يشتغل كل 0.1 ثانية أثناء السحب

    void Start()
    {
        if (brightnessSlider != null && brightnessPanel != null)
        {
            // التحقق من وجود إعدادات إضاءة محفوظة
            if (PlayerPrefs.HasKey("GameBrightness"))
            {
                float savedBrightness = PlayerPrefs.GetFloat("GameBrightness");
                brightnessSlider.value = savedBrightness;
                SetBrightness(savedBrightness);
            }
            else
            {
                // الإضاءة الافتراضية 100% (القيمة 1)
                brightnessSlider.value = 1f;
                SetBrightness(1f);
            }

            // ربط الشريط بتغيير الإضاءة والصوت
            brightnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // دالة جديدة تنفذ لما اللاعب يغير قيمة الشريط
    private void OnSliderValueChanged(float sliderValue)
    {
        SetBrightness(sliderValue);

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

    public void SetBrightness(float sliderValue)
    {
        if (brightnessPanel != null)
        {
            // حسبة بسيطة: لما الشريط يكون 1 (أعلى إضاءة)، الشفافية بتكون 0
            // لما الشريط يقل، الشفافية بتزيد (عشان تعتم الشاشة)
            float alpha = 1f - sliderValue;

            Color panelColor = brightnessPanel.color;
            panelColor.a = alpha;
            brightnessPanel.color = panelColor;

            // حفظ مستوى الإضاءة
            PlayerPrefs.SetFloat("GameBrightness", sliderValue);
            PlayerPrefs.Save();
        }
    }
}