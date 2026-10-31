using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    [Header("Liquid Settings")]
    public MeshRenderer liquidRenderer; 
    private Material liquidMaterial;

    [Header("Shader Calibration")]
    public float minShaderFill = -1.2f; // الرقم اللي بيخفي السائل تماماً (عدله حسب الـ Shader تبعك)
    public float maxShaderFill = 1f;    // الرقم اللي بيعبي السائل للآخر (عدله حسب الـ Shader تبعك)

    [Header("Volume (0 to 1)")]
    [Range(0f, 1f)] 
    public float currentFillPercentage = 1f; // 1 يعني مليان، 0 يعني فاضي

    [Header("Pouring Mechanics")]
    public float pourAngle = 90f; // الزاوية اللي بيبدأ يصب عندها 
    public float pourRate = 0.2f; // سرعة نزول السائل بالثانية 

    public bool isPouring = false; 

    void Start()
    {
        if (liquidRenderer != null)
        {
            liquidMaterial = liquidRenderer.material;
            UpdateLiquidLevel();
        }
    }

    void Update()
    {
        CheckPouringAngle();
        UpdateLiquidLevel();
    }

    void CheckPouringAngle()
    {
        // نحسب زاوية ميلان الكاسة بالنسبة للوضع المستقيم
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);

        // إذا الكاسة مالت أكثر من زاوية الصب، ولسه فيها سائل
        if (tiltAngle > pourAngle && currentFillPercentage > 0f)
        {
            isPouring = true;
            // ننقص السائل مع الوقت
            currentFillPercentage -= pourRate * Time.deltaTime;
            
            // نتأكد إنه ما ينزل تحت الصفر
            if (currentFillPercentage < 0f) 
            {
                currentFillPercentage = 0f;
            }
        }
        else
        {
            isPouring = false; // وقف صب
        }
    }

    public void UpdateLiquidLevel()
    {
        if (liquidMaterial != null)
        {
            float actualShaderValue = Mathf.Lerp(minShaderFill, maxShaderFill, currentFillPercentage);
            liquidMaterial.SetFloat("_Fill", actualShaderValue); 
        }
    }
}