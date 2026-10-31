using UnityEngine;

public class FlameTestScript : MonoBehaviour
{
    public ParticleSystem myCustomFire;
    public AudioSource fireSound;

    private bool touchedLiquid = false;
    private bool canBurn = false;

    private void OnTriggerEnter(Collider other)
    {
        // لما يدخل المحلول
        if (other.CompareTag("Liquid"))
        {
            touchedLiquid = true;
            canBurn = true;
        }

        // لما يدخل النار
        if (other.CompareTag("Burner"))
        {
            water burner = other.GetComponent<water>();

            // لازم يكون دخل المحلول والنار شغالة
            if (canBurn && touchedLiquid && burner != null && burner.isWaterOn)
            {
                if (myCustomFire != null)
                {
                    myCustomFire.gameObject.SetActive(true);
                    myCustomFire.Play();

                    // تشغيل الصوت
                    if (fireSound != null && !fireSound.isPlaying)
                    {
                        fireSound.Play();
                    }

                    // تسجيل التجربة
                    FindObjectOfType<MenuManager>().MarkWireAsDone(1);
                }

                // يمنع التشغيل مرة ثانية إلا بعد المحلول
                canBurn = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // إذا طلع من النار
        if (other.CompareTag("Burner"))
        {
            if (myCustomFire != null)
            {
                myCustomFire.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                // إيقاف الصوت
                if (fireSound != null)
                {
                    fireSound.Stop();
                }
            }

            // لازم يرجع للمحلول مرة ثانية
            touchedLiquid = false;
        }
    }
}