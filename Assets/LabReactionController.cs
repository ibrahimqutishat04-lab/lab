using UnityEngine;
using System.Collections;

public class LabReactionController : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem bubbleMain;
    public ParticleSystem bubbleReaction;

    [Header("Settings")]
    public float reactionDuration = 3.0f;

    [Header("References")]
    public MenuManager menuManager;

    private bool hasStarted = false;
    private Coroutine reactionCoroutine;

    void Start()
    {
        if (menuManager == null)
        {
            menuManager = FindObjectOfType<MenuManager>();
        }
    }

    public void StartChemicalEffect()
    {
        if (!hasStarted)
        {
            reactionCoroutine = StartCoroutine(SmoothReactionSequence());
        }
    }

    IEnumerator SmoothReactionSequence()
    {
        hasStarted = true;

        yield return new WaitForSeconds(1.0f);

        if (bubbleMain != null) bubbleMain.Play();
        if (bubbleReaction != null) bubbleReaction.Play();

        yield return new WaitForSeconds(reactionDuration);

        if (menuManager != null)
        {
            menuManager.MarkWireAsDone(4);
        }
    }

    public void ResetReactionLogic()
    {
        if (reactionCoroutine != null)
        {
            StopCoroutine(reactionCoroutine);
            reactionCoroutine = null;
        }

        hasStarted = false;

        // --- التعديل هنا بناءً على فكرتك ---
        if (bubbleMain != null)
        {
            bubbleMain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (bubbleReaction != null)
        {
            bubbleReaction.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        // ---------------------------------
    }
}