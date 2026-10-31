using UnityEngine;

public class FacePlayerOnEnable : MonoBehaviour
{ 
    [Header("إعدادات المسافة")]
    public float distanceFromPlayer = 2.0f; // قديش بدك اللوحة تبعد عن اللاعب

    // هاي الدالة بتشتغل تلقائياً أول ما تعمل Active للوحة التجربة
    private void OnEnable()
    {
        MovePanelInFrontOfPlayer();
    }

    // دالة مسؤولة عن نقل اللوحة قدام اللاعب
    public void MovePanelInFrontOfPlayer()
    {
        if (Camera.main == null) return; // للتأكد إن الكاميرا موجودة

        Transform cameraTransform = Camera.main.transform;

        // 1. حساب المكان الجديد (قدام الكاميرا بالمسافة المحددة)
        transform.position = cameraTransform.position + (cameraTransform.forward * distanceFromPlayer);

        // 2. تدوير اللوحة عشان تصير مواجهة لعيون اللاعب مباشرة
        transform.LookAt(new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z));
        transform.forward *= -1; // عشان الواجهة ما تطلع مقلوبة
    }

    // دالة إضافية بتقدر تربطها بزر "إغلاق" أو "رجوع" جوا لوحة التجربة
    public void CloseExperimentPanel()
    {
        gameObject.SetActive(false);
    }
}