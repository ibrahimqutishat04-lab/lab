using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [Header("Question Panels")]
    public GameObject firstQuestion;
    public GameObject secondQuestion;

    [Header("Main Questions Panel")]
    public GameObject questionsPanel;

    [Header("Experiments Panel")]
    public GameObject experimentsPanel;

    [Header("Settings")]
    public float waitTime = 1.5f;
    public Color defaultButtonColor = Color.white; // تأكد من الـ Inspector إنو لونه أبيض شفافيته 255

    [Header("Audio Settings")]
    public AudioSource quizAudioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    public void OnButtonClick(bool isCorrect)
    {
        GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

        if (selectedObj != null)
        {
            Button clickedButton = selectedObj.GetComponent<Button>();

            if (clickedButton != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                StartCoroutine(HandleAnswer(isCorrect, clickedButton));
            }
        }
    }

    private IEnumerator HandleAnswer(bool isCorrect, Button clickedButton)
    {
        GameObject currentPanel = clickedButton.transform.parent.gameObject;

        if (quizAudioSource != null)
        {
            if (isCorrect && correctSound != null)
                quizAudioSource.PlayOneShot(correctSound);
            else if (!isCorrect && wrongSound != null)
                quizAudioSource.PlayOneShot(wrongSound);
        }

        Image btnImage = clickedButton.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.color = isCorrect ? Color.green : Color.red;
        }

        // تعطيل الأزرار مؤقتاً
        Button[] allButtons = currentPanel.GetComponentsInChildren<Button>();
        foreach (Button b in allButtons)
        {
            b.interactable = false;
        }

        yield return new WaitForSeconds(waitTime);

        if (currentPanel == firstQuestion)
        {
            firstQuestion.SetActive(false);
            secondQuestion.SetActive(true);
        }
        else if (currentPanel == secondQuestion)
        {
            questionsPanel.SetActive(false);
            experimentsPanel.SetActive(true);
            Debug.Log("Experiment Finished");
        }
    }

    // دالة إعادة الضبط لتنظيف الأزرار والألوان
    public void ResetQuizState()
    {
        if (firstQuestion != null) ResetButtonsInPanel(firstQuestion);
        if (secondQuestion != null) ResetButtonsInPanel(secondQuestion);

        // إرجاع السؤال الأول للظهور
        if (firstQuestion != null) firstQuestion.SetActive(true);
        if (secondQuestion != null) secondQuestion.SetActive(false);
    }

    private void ResetButtonsInPanel(GameObject panel)
    {
        Button[] allButtons = panel.GetComponentsInChildren<Button>(true);

        foreach (Button b in allButtons)
        {
            b.interactable = true; // إعادة التفعيل

            Image btnImage = b.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.color = defaultButtonColor; // إرجاع اللون الأبيض
            }
        }
    }
}