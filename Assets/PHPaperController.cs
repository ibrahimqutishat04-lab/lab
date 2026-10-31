using UnityEngine;

public class PHPaperController : MonoBehaviour
{
    private Renderer paperRenderer;
    private bool isDipped = false;

    public static int dippedPapersCount = 0;
    public AudioSource dipAudioSource;

    void Start()
    {
        paperRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Liquid") && !isDipped)
        {
            LiquidProperty liquid = other.GetComponent<LiquidProperty>();

            if (liquid != null)
            {
                ChangeColor(liquid.liquidColor);

                isDipped = true;
                dippedPapersCount++;

                if (dipAudioSource != null)
                {
                    dipAudioSource.Play();
                }

                if (dippedPapersCount >= 3)
                {
                    FindObjectOfType<MenuManager>().MarkWireAsDone(3);
                }
            }
        }
    }

    void ChangeColor(Color newColor)
    {
        if (paperRenderer != null)
        {
            Material mat = paperRenderer.material;

            // حافظ على الشفافية الأصلية
            float originalAlpha = mat.color.a;
            newColor.a = originalAlpha;

            // Standard Shader
            if (mat.HasProperty("_Color"))
            {
                mat.SetColor("_Color", newColor);
            }

            // URP Shader
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", newColor);
            }
        }
    }

    public void ResetPaperLogic()
    {
        isDipped = false;
        dippedPapersCount = 0;
    }
}