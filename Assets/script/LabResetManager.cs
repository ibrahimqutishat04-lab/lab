using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LabResetManager : MonoBehaviour
{
    // ==========================================
    // 1. الجزء الخاص بك (إعادة الضبط - Reset)
    // ==========================================
    [System.Serializable]
    public class ResetableItem
    {
        public GameObject itemObject;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Quaternion startRotation;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public Renderer[] renderers;
        [HideInInspector] public List<Material[]> originalMaterials;
    }

    [Header("الأدوات التي سيتم إرجاعها لمكانها")]
    public List<ResetableItem> itemsToReset;

    [Header("تأثيرات الدخان أو الانفجارات لإيقافها")]
    public List<ParticleSystem> particlesToStop;

    [Header("أصوات التفاعلات التي سيتم إيقافها")]
    public List<AudioSource> soundsToStop;

    [Header("💡 ضع هنا سكربتات التفاعل لتصفيرها 💡")]
    public UnityEvent onResetLogic;

    // ==========================================
    // 2. الإضافة الجديدة (لإدارة التجارب والـ Disable)
    // ==========================================
    [System.Serializable]
    public class ChemistryExperiment
    {
        public string experimentName; // اسم التجربة (للتنظيم فقط)
        public GameObject[] experimentObjects; // أدوات تظهر فقط في هذه التجربة
        public MonoBehaviour[] experimentScripts; // سكربتات التفاعلات (عشان نعملها Disable/Enable)
    }

    [Header("قائمة التجارب (للتفعيل والإلغاء)")]
    public List<ChemistryExperiment> experiments;


    void Start()
    {
        // كود الـ Start الخاص بك كما هو
        foreach (var item in itemsToReset)
        {
            if (item.itemObject != null)
            {
                item.startPosition = item.itemObject.transform.position;
                item.startRotation = item.itemObject.transform.rotation;
                item.rb = item.itemObject.GetComponent<Rigidbody>();
                item.renderers = item.itemObject.GetComponentsInChildren<Renderer>(true);
                item.originalMaterials = new List<Material[]>();

                foreach (Renderer r in item.renderers)
                {
                    item.originalMaterials.Add(r.sharedMaterials);
                }
            }
        }
    }

    // دالة التصفير الخاصة بك كما هي
    public void ResetLabEnvironment()
    {
        foreach (var item in itemsToReset)
        {
            if (item.itemObject != null)
            {
                item.itemObject.transform.position = item.startPosition;
                item.itemObject.transform.rotation = item.startRotation;

                if (item.rb != null)
                {
                    item.rb.linearVelocity = Vector3.zero;
                    item.rb.angularVelocity = Vector3.zero;
                }

                if (item.renderers != null)
                {
                    for (int i = 0; i < item.renderers.Length; i++)
                    {
                        if (item.renderers[i] != null)
                        {
                            item.renderers[i].sharedMaterials = item.originalMaterials[i];
                        }
                    }
                }

                item.itemObject.SetActive(true);
            }
        }

        foreach (var ps in particlesToStop)
        {
            if (ps != null)
            {
                ps.Stop();
                ps.Clear();
            }
        }

        foreach (var audio in soundsToStop)
        {
            if (audio != null && audio.isPlaying)
            {
                audio.Stop();
            }
        }

        if (onResetLogic != null)
        {
            onResetLogic.Invoke();
        }
    }

    // ==========================================
    // 3. الدالة التي ستربطها بأزرار الـ Panel
    // ==========================================
    public void StartExperiment(int experimentIndex)
    {
        // أولاً: نعمل Reset للطاولة باستخدام دالتك
        ResetLabEnvironment();

        // ثانياً: نمر على كل التجارب، نطفي اللي ما بدنا إياه ونشغل المطلوبة
        for (int i = 0; i < experiments.Count; i++)
        {
            // إذا كان الـ index الحالي هو نفس رقم التجربة المطلوبة، المتغير هيكون true، غير هيك false
            bool isActive = (i == experimentIndex);

            // تفعيل أو إخفاء الأدوات الخاصة بالتجربة
            foreach (GameObject obj in experiments[i].experimentObjects)
            {
                if (obj != null) obj.SetActive(isActive);
            }

            // تفعيل أو إطفاء سكربتات التفاعل (هذا اللي بيمنع تفاعلات التجربة القديمة تشتغل)
            foreach (MonoBehaviour script in experiments[i].experimentScripts)
            {
                if (script != null) script.enabled = isActive;
            }
        }

        Debug.Log("تم تبديل المختبر إلى التجربة رقم: " + experimentIndex);
    }
}