using UnityEngine;

public class FlaskLiquidShader : MonoBehaviour
{
    public Material liquidMaterial;
    public float pourSpeed = 0.3f;

    float fillAmount;

    void Start()
    {
        fillAmount = liquidMaterial.GetFloat("Fill");
    }

    public void Pour()
    {
        if (fillAmount > 0f)
        {
            fillAmount -= pourSpeed * Time.deltaTime;
            fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);

            liquidMaterial.SetFloat("Fill", fillAmount);
        }
    }
}



