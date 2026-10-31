using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [Header("Movement Control")]
    public DynamicMoveProvider moveProvider;
    public GameObject xrOrigin;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject experimentsPanel;
    public GameObject questionPanel;

    [Header("Experiments Quiz Panels")]
    public GameObject beilsteinQuiz;
    public GameObject co2Quiz;
    public GameObject phQuiz;

    [Header("Quiz Managers")]
    public QuizManager beilsteinQuizManager;
    public QuizManager co2QuizManager;
    public QuizManager phQuizManager;

    [Header("Beilstein Questions")]
    public GameObject beilsteinQ1;
    public GameObject beilsteinQ2;

    [Header("CO2 Questions")]
    public GameObject co2Q1;
    public GameObject co2Q2;

    [Header("PH Questions")]
    public GameObject phQ1;
    public GameObject phQ2;

    [Header("Return Locations")]
    public Transform quizAreaLocation;

    [Header("UI Audio Settings")]
    public AudioSource uiAudioSource;
    public AudioClip clickSound;
    public AudioClip popupSound;

    [Header("Experiment Reset Objects")]
    public List<LiquidReceiver> allReceivers;
    public List<LabReactionController> allReactions;
    public List<AdvancedLiquidPour> allPouringSources;

    [System.Serializable]
    public class TransformData
    {
        public Transform target;
        [HideInInspector] public Vector3 startPos;
        [HideInInspector] public Quaternion startRot;
    }

    public List<TransformData> objectsToResetPositions;

    private int activeExperiment = 0;

    private bool wire1Finished = false;
    private bool wire2Finished = false;
    private bool phExperimentFinished = false;
    private bool co2ReactionFinished = false;

    // يمنع ظهور السؤال أكثر من مرة
    private bool questionShown = false;

    void Start()
    {
        if (moveProvider != null)
            moveProvider.enabled = false;

        mainMenuPanel.SetActive(true);
        experimentsPanel.SetActive(false);

        if (questionPanel != null)
            questionPanel.SetActive(false);

        ResetQuizzes();

        foreach (var item in objectsToResetPositions)
        {
            if (item.target != null)
            {
                item.startPos = item.target.position;
                item.startRot = item.target.rotation;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
            uiAudioSource.PlayOneShot(clip);
    }

    private void ResetQuizzes()
    {
        if (beilsteinQuizManager != null)
            beilsteinQuizManager.ResetQuizState();

        if (co2QuizManager != null)
            co2QuizManager.ResetQuizState();

        if (phQuizManager != null)
            phQuizManager.ResetQuizState();

        if (beilsteinQuiz != null)
            beilsteinQuiz.SetActive(false);

        if (co2Quiz != null)
            co2Quiz.SetActive(false);

        if (phQuiz != null)
            phQuiz.SetActive(false);
    }

    public void MarkWireAsDone(int wireNumber)
    {
        if (wireNumber == 1)
            wire1Finished = true;

        if (wireNumber == 2)
            wire2Finished = true;

        if (wireNumber == 3)
            phExperimentFinished = true;

        if (wireNumber == 4)
            co2ReactionFinished = true;

        // يمنع التكرار
        if (!questionShown &&
            ((wire1Finished && wire2Finished)
            || phExperimentFinished
            || co2ReactionFinished))
        {
            questionShown = true;
            StartCoroutine(WaitAndShowQuestion(5f));
        }
    }

    private IEnumerator WaitAndShowQuestion(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (questionPanel != null)
        {
            Vector3 playerForward = xrOrigin.transform.forward;
            playerForward.y = 0;
            playerForward.Normalize();

            Vector3 spawnPos =
                xrOrigin.transform.position + playerForward * 1.2f;

            spawnPos.y = 1.6f;

            questionPanel.transform.position = spawnPos;

            Vector3 lookAtPos = xrOrigin.transform.position;
            lookAtPos.y = spawnPos.y;

            questionPanel.transform.LookAt(lookAtPos);
            questionPanel.transform.Rotate(0, 180, 0);

            questionPanel.SetActive(true);

            PlaySound(popupSound);
        }
    }

    public void StartGameAndEnableMovement()
    {
        PlaySound(clickSound);

        mainMenuPanel.SetActive(false);

        if (moveProvider != null)
            moveProvider.enabled = true;
    }

    public void ShowExperimentsMenu()
    {
        PlaySound(clickSound);

        experimentsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void ResetAllExperimentObjects()
    {
        ResetQuizzes();

        foreach (var receiver in allReceivers)
            if (receiver != null)
                receiver.ResetLiquidState();

        foreach (var reaction in allReactions)
            if (reaction != null)
                reaction.ResetReactionLogic();

        foreach (var source in allPouringSources)
            if (source != null)
                source.ResetPourState();

        foreach (var item in objectsToResetPositions)
        {
            if (item.target != null)
            {
                item.target.position = item.startPos;
                item.target.rotation = item.startRot;

                Rigidbody rb =
                    item.target.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
    }

    public void TeleportToBeilstein(Transform target)
    {
        activeExperiment = 1;
        ResetStepTracker();
        ResetAllExperimentObjects();
        Teleport(target);
    }

    public void TeleportToCO2(Transform target)
    {
        activeExperiment = 2;
        ResetStepTracker();
        ResetAllExperimentObjects();
        Teleport(target);
    }

    public void TeleportToPH(Transform target)
    {
        activeExperiment = 3;
        ResetStepTracker();
        ResetAllExperimentObjects();
        Teleport(target);
    }

    private void ResetStepTracker()
    {
        wire1Finished = false;
        wire2Finished = false;
        phExperimentFinished = false;
        co2ReactionFinished = false;

        // يسمح بالظهور مرة واحدة للتجربة الجديدة
        questionShown = false;
    }

    private void Teleport(Transform targetLocation)
    {
        if (xrOrigin != null && targetLocation != null)
        {
            CharacterController cc =
                xrOrigin.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            xrOrigin.transform.position =
                targetLocation.position;

            xrOrigin.transform.rotation =
                targetLocation.rotation;

            if (cc != null)
                cc.enabled = true;

            if (moveProvider != null)
                moveProvider.enabled = true;

            experimentsPanel.SetActive(false);
        }
    }

    public void OnClickYes()
    {
        PlaySound(clickSound);

        if (questionPanel != null)
            questionPanel.SetActive(false);

        ResetAllExperimentObjects();
        Teleport(quizAreaLocation);

        if (activeExperiment == 1)
        {
            beilsteinQuiz.SetActive(true);
            beilsteinQ1.SetActive(true);
            beilsteinQ2.SetActive(false);
        }
        else if (activeExperiment == 2)
        {
            co2Quiz.SetActive(true);
            co2Q1.SetActive(true);
            co2Q2.SetActive(false);
        }
        else if (activeExperiment == 3)
        {
            phQuiz.SetActive(true);
            phQ1.SetActive(true);
            phQ2.SetActive(false);
        }
    }

    public void OnClickNo()
    {
        PlaySound(clickSound);

        if (questionPanel != null)
            questionPanel.SetActive(false);

        ResetAllExperimentObjects();
        Teleport(quizAreaLocation);

        experimentsPanel.SetActive(true);
    }
}