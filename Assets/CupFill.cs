using UnityEngine;

public class CupFill : MonoBehaviour
{
    public Transform liquid;        // «·”«∆· œ«Œ· «·ﬂ√”
    public Transform flaskLiquid;   // «·”«∆· œ«Œ· «·√‰»Ê»

    public float fillSpeed = 0.02f;
    public float maxHeight = 0.5f;

    public void Fill()
    {
        // ≈–« ·„ Ì„ ·∆ «·ﬂ√” »⁄œ
        if (liquid.localScale.y < maxHeight)
        {
            float amount = fillSpeed * Time.deltaTime;

            //  ⁄»∆… «·ﬂ√”
            liquid.localScale += new Vector3(0, amount, 0);
            liquid.position += new Vector3(0, amount / 2f, 0);

            // ≈‰ﬁ«’ «·”«∆· „‰ «·√‰»Ê»
            if (flaskLiquid != null && flaskLiquid.localScale.y > 0.05f)
            {
                flaskLiquid.localScale -= new Vector3(0, amount, 0);
                flaskLiquid.position -= new Vector3(0, amount / 2f, 0);
            }
        }
    }
}