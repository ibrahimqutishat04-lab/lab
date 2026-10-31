using UnityEngine;

public class LiquidPour : MonoBehaviour
{
    public Transform pourPoint;
    public LineRenderer line;
    public float maxDistance = 2f;

    // زاوية الميلان لبدء الصب
    public float pourThreshold = -0.3f;

    // Reference to the liquid GameObject inside the flask
    public Transform liquid;

    // مقدار نقصان السائل
    public float decreaseAmount = 0.2f;

    void Update()
    {
        // Stop if liquid is empty
        if (liquid == null || liquid.localScale.y <= 0f)
        {
            line.enabled = false;
            return;
        }

        // Check tilt angle
        if (transform.forward.y > pourThreshold)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;

        RaycastHit hit;

        if (Physics.Raycast(pourPoint.position, Vector3.down, out hit, maxDistance))
        {
            line.SetPosition(0, pourPoint.position);
            line.SetPosition(1, hit.point);

            CupFill cup = hit.collider.GetComponent<CupFill>();

            if (cup != null)
            {
                cup.Fill();
                DecreaseLiquid();
            }
        }
        else
        {
            line.SetPosition(0, pourPoint.position);
            line.SetPosition(1, pourPoint.position + Vector3.down * maxDistance);
        }
    }

    // تقليل السائل
    public void DecreaseLiquid()
    {
        if (liquid == null) return;

        Vector3 scale = liquid.localScale;

        scale.y -= decreaseAmount * Time.deltaTime;

        // Prevent negative scale
        scale.y = Mathf.Max(scale.y, 0f);

        liquid.localScale = scale;

        // If empty disable object
        if (scale.y <= 0f)
        {
            liquid.gameObject.SetActive(false);
        }
    }
}

