using UnityEngine;
using UnityEngine.UI;

public class SettingsLoader : MonoBehaviour
{
    [Header("Brightness Setup For This Scene")]
    public Image sceneBrightnessPanel; // الصورة السوداء تبعت المشهد الحالي

    void Start()
    {
        // 1. تحميل الصوت (هذا بيطبق على كل المشهد لحاله)
        if (PlayerPrefs.HasKey("GameVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("GameVolume");
        }

        // 2. تحميل الإضاءة (هذا بيطبق على الصورة السوداء تبعت هذا المشهد)
        if (PlayerPrefs.HasKey("GameBrightness") && sceneBrightnessPanel != null)
        {
            float savedBrightness = PlayerPrefs.GetFloat("GameBrightness");

            // نحسب العتمة ونطبقها
            float alpha = 1f - savedBrightness;
            Color panelColor = sceneBrightnessPanel.color;
            panelColor.a = alpha;
            sceneBrightnessPanel.color = panelColor;
        }
    }
}