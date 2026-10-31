using UnityEngine;

public class CopperWireLogic : MonoBehaviour
{
    [Header("Effects")]
    public ParticleSystem greenFireOnWire; // اسحب النار الخضراء الموجودة تحت السلك هنا

    [Header("Status")]
    public bool hasSample = false; // هل السلك مبلل؟

    private void OnTriggerEnter(Collider other)
    {
        // 1. غمس السلك في السائل
        if (other.name.Contains("Liquid"))
        {
            hasSample = true;
            Debug.Log("السلك مبلل الآن!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 2. إذا لمس السلك منطقة النار (الـ Burner)
        if (other.name.Contains("Burner"))
        {
            // إذا كان السلك مبللاً، تخرج منه النار الخضراء
            if (hasSample && greenFireOnWire != null)
            {
                if (!greenFireOnWire.isPlaying) 
                {
                    greenFireOnWire.Play();
                    Debug.Log("اشتعال المادة الكيميائية باللون الأخضر!");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // إطفاء النار الخضراء عند إبعاد السلك عن البوتاجاز
        if (other.name.Contains("Burner"))
        {
            if (greenFireOnWire != null)
            {
                greenFireOnWire.Stop();
            }
        }
    }
}