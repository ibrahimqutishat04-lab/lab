using UnityEngine;

public class ReceiverCup : MonoBehaviour
{
    [Header("إعدادات الكأس الثاني")]
    public Transform liquidInside;  // الأسطوانة اللي جوا الكأس الثاني
    public float maxFillY = 1.0f;   // أقصى حد للامتلاء (حسب حجم كأسك)

    // هاي الوظيفة بيستدعيها الكأس الأول لما يسكب عليه
    public void ReceiveLiquid(float amount)
    {
        Vector3 scale = liquidInside.localScale;
        // زيادة السائل
        scale.y += amount;
        // التأكد إن السائل ما بيطلع برا الكأس
        liquidInside.localScale = new Vector3(scale.x, Mathf.Min(maxFillY, scale.y), scale.z);
    }
}