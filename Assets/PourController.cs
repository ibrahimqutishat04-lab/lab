using UnityEngine;
using UnityEngine.VFX; // ضروري للتعامل مع الـ VFX

public class PourController : MonoBehaviour
{
    [Header("إعدادات الكأس الأول")]
    public Transform liquidInside;     // الأسطوانة اللي جوا الكأس الأول
    public VisualEffect pourVFX;       // شلال السائل اللي عملناه
    public Transform pourOrigin;       // نقطة وهمية عند فوهة الكأس (مكان نزول السائل)

    [Header("إعدادات السكب")]
    public float pourAngle = 45f;      // الزاوية اللي بيبدأ يكب عندها
    public float flowRate = 0.5f;      // سرعة السكب

    private bool isPouring = false;

    void Update()
    {
        // حساب زاوية ميلان الكأس
        float tilt = Vector3.Angle(Vector3.up, transform.up);

        // إذا الكأس مايل + لسه فيه سائل جواه
        if (tilt > pourAngle && liquidInside.localScale.y > 0)
        {
            StartPouring();
        }
        else
        {
            StopPouring();
        }
    }

    void StartPouring()
    {
        if (!isPouring)
        {
            pourVFX.Play(); // تشغيل الشلال
            isPouring = true;
        }

        // 1. تنقيص السائل من الكأس الأول
        Vector3 scale = liquidInside.localScale;
        scale.y -= flowRate * Time.deltaTime;
        liquidInside.localScale = new Vector3(scale.x, Mathf.Max(0, scale.y), scale.z);

        // 2. إرسال ليزر لتحت عشان يعبي الكأس الثاني
        RaycastHit hit;
        // نطلق ليزر من فوهة الكأس باتجاه الأسفل بمسافة 2 متر
        if (Physics.Raycast(pourOrigin.position, Vector3.down, out hit, 2.0f))
        {
            // إذا الليزر ضرب بكأس ثاني عليه سكريبت الاستقبال
            ReceiverCup receiver = hit.collider.GetComponent<ReceiverCup>();
            if (receiver != null)
            {
                // إرسال أمر تعبئة للكأس الثاني
                receiver.ReceiveLiquid(flowRate * Time.deltaTime);
            }
        }
    }

    void StopPouring()
    {
        if (isPouring)
        {
            pourVFX.Stop(); // إيقاف الشلال
            isPouring = false;
        }
    }
}