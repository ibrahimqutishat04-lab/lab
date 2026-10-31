using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public InputActionReference pauseAction;

    [Header("XR Settings")]
    public float distance = 2.0f; // المسافة أمام اللاعب

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        bool isActive = pausePanel.activeSelf;
        pausePanel.SetActive(!isActive);

        // التعديل هنا: إذا فتحنا القائمة، ننقلها لمكان اللاعب
        if (!isActive)
        {
            UpdateMenuPosition();
            Time.timeScale = 0; // إيقاف اللعبة
        }
        else
        {
            Time.timeScale = 1; // استئناف اللعبة
        }
    }

    private void UpdateMenuPosition()
    {
        Transform cameraTransform = Camera.main.transform;

        // وضع القائمة أمام الكاميرا بمسافة معينة
        pausePanel.transform.position = cameraTransform.position + (cameraTransform.forward * distance);

        // جعل القائمة تنظر للاعب (Rotation)
        pausePanel.transform.LookAt(new Vector3(cameraTransform.position.x, pausePanel.transform.position.y, cameraTransform.position.z));
        pausePanel.transform.forward *= -1; // تصحيح اتجاه الـ UI
    }
}