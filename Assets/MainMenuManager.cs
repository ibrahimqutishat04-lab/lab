using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    [Header("Player Tracking")]
    public Transform playerCamera;
    public float spawnDistance = 1.2f;

    [Header("Audio Settings")]
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    private bool wasMainMenuActive = false;
    private bool wasPauseMenuActive = false;

    private void PlayClickSound()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    public void StartGame()
    {
        PlayClickSound();
        StartCoroutine(ActionWithDelay(() => SceneManager.LoadScene("Laboratory Scene"), 0.3f));
    }

    public void OpenSettings()
    {
        PlayClickSound();

        // حفظ حالة القوائم الحالية قبل ما نخفيها
        if (mainMenuPanel != null) wasMainMenuActive = mainMenuPanel.activeSelf;
        if (pauseMenuPanel != null) wasPauseMenuActive = pauseMenuPanel.activeSelf;

        // إخفاء القوائم (الرئيسية والبوز)
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        // إظهار الإعدادات (يمين ويسار فقط، وعلى مستوى العين)
        if (settingsPanel != null)
        {
            if (playerCamera != null)
            {
                // 1. ناخذ الاتجاه الأمامي ونلغي منه الارتفاع والانخفاض (عشان تلف يمين/يسار بس)
                Vector3 playerForward = playerCamera.forward;
                playerForward.y = 0;
                playerForward.Normalize();

                // 2. نحسب الموقع الجديد (موقع اللاعب + الاتجاه)
                Vector3 spawnPosition = playerCamera.position + (playerForward * spawnDistance);

                // نخلي ارتفاع القائمة مساوي تماماً لارتفاع الكاميرا (بدون ما ننزلها)
                spawnPosition.y = playerCamera.position.y;

                settingsPanel.transform.position = spawnPosition;

                // 3. ندوّر القائمة عشان تواجه اللاعب بشكل مستقيم
                Vector3 lookAtPos = playerCamera.position;
                lookAtPos.y = spawnPosition.y;

                settingsPanel.transform.LookAt(lookAtPos);
                settingsPanel.transform.Rotate(0, 180, 0);
            }

            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        PlayClickSound();

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (mainMenuPanel != null && wasMainMenuActive)
        {
            mainMenuPanel.SetActive(true);
        }

        if (pauseMenuPanel != null && wasPauseMenuActive)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    public void ExitGame()
    {
        PlayClickSound();
        Debug.Log("Exiting the game...");
        StartCoroutine(ActionWithDelay(() => Application.Quit(), 0.3f));
    }

    private IEnumerator ActionWithDelay(System.Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        action.Invoke();
    }
}