using UnityEngine;
using System.Collections;

public class Flamered : MonoBehaviour
{
    public ParticleSystem myCustomFire;
    public AudioSource fireAudioSource;

    private Color orangeColor = new Color(1f, 0.5f, 0f);
    private Color redColor = Color.red;

    private Coroutine fireDelay;

    private bool touchedRedLiquid = false;
    private bool canBurn = false;

    private void OnTriggerEnter(Collider other)
    {
        // ·„« ÌœŒ· «·„Õ·Ê·
        if (other.CompareTag("RedLiquid"))
        {
            touchedRedLiquid = true;
            canBurn = true;
        }

        // ·„« ÌœŒ· «·‰«—
        if (other.CompareTag("Burner"))
        {
            water burner = other.GetComponent<water>();

            // ·«“„ ÌﬂÊ‰ œŒ· «·„Õ·Ê· Ê«·‰«— ‘€«·…
            if (canBurn && touchedRedLiquid && burner != null && burner.isWaterOn)
            {
                fireDelay = StartCoroutine(PlayFireAfterDelay(2f));

                // »⁄œ „« Ì‘ €· Ì„‰⁄ ≈⁄«œ… «· ‘€Ì·
                canBurn = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ≈–« ÿ·⁄ „‰ «·‰«—
        if (other.CompareTag("Burner"))
        {
            if (fireDelay != null)
            {
                StopCoroutine(fireDelay);
            }

            if (myCustomFire != null)
            {
                var mainModule = myCustomFire.main;
                mainModule.startColor = orangeColor;

                myCustomFire.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                if (fireAudioSource != null)
                {
                    fireAudioSource.Stop();
                }
            }

            // „Â„ Ãœ«: Ì—Ã⁄ Ìÿ·» «·„Õ·Ê· „—… À«‰Ì…
            touchedRedLiquid = false;
        }
    }

    private IEnumerator PlayFireAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (myCustomFire != null)
        {
            myCustomFire.gameObject.SetActive(true);

            var mainModule = myCustomFire.main;
            mainModule.startColor = redColor;

            myCustomFire.Play();

            if (fireAudioSource != null && !fireAudioSource.isPlaying)
            {
                fireAudioSource.Play();
            }

            FindObjectOfType<MenuManager>().MarkWireAsDone(2);
        }
    }
}