using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ضرورية للـ Coroutine

public class MenuButtonsHandler : MonoBehaviour
{
    [Header("Panels Setup")]
    public GameObject pauseMenuPanel; // اسحب البانل تبعت البوز كاملة لهون
    public GameObject experimentSelectionPanel; // اسحب بانل التجارب لهون

    [Header("Audio Settings")]
    public AudioSource uiAudioSource; // مصدر الصوت
    public AudioClip clickSound;      // صوت الكبسة

    // دالة مساعدة لتشغيل الصوت
    private void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    // 1. دالة زر (Resume) - بتكمل اللعب وبتخفي القائمة
    public void ResumeGame()
    {
        PlayClickSound(); // تشغيل الصوت

        Time.timeScale = 1f; // إرجاع الوقت عشان يشتغل كود الحركة

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false); // إخفاء القائمة
    }

    // 2. دالة زر (Select Experiment) - بتخفي البوز وبتطلع بانل التجارب
    public void ShowExperimentPanel()
    {
        PlayClickSound(); // تشغيل الصوت
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false); // إخفاء قائمة البوز

        if (experimentSelectionPanel != null)
            experimentSelectionPanel.SetActive(true); // إظهار بانل التجارب
    }

    // 3. دالة زر (Main menu) - بترجعك للسين الرئيسي
    public void LoadMainMenu(string sceneName)
    {
        PlayClickSound(); // تشغيل الصوت

        Time.timeScale = 1f; // ضروري عشان السين الجديدة ما تفتح مجمدة

        // استدعاء التأخير الزمني قبل الانتقال
        StartCoroutine(LoadSceneWithDelay(sceneName, 0.3f));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        // نستخدم Realtime عشان يشتغل الكود حتى لو اللعبة معمولة Pause
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(sceneName);
    }

    // 4. دالة زر (Quit) - بتطلع من اللعبة
    public void QuitGame()
    {
        PlayClickSound(); // تشغيل الصوت
        Debug.Log("جاري الخروج من اللعبة...");

        // استدعاء التأخير الزمني قبل الإغلاق
        StartCoroutine(QuitWithDelay(0.3f));
    }

    private IEnumerator QuitWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Application.Quit();
    }
}