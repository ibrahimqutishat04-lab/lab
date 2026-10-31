using System.Collections;
using UnityEngine;
using TMPro;

public class CounterText : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public float countSpeed = 0.05f;

    void Start()
    {
        StartCoroutine(CountUp());
    }

    IEnumerator CountUp()
    {
        for (int i = 0; i <= 50; i++)
        {
            counterText.text = i.ToString();
            yield return new WaitForSeconds(countSpeed);
        }
    }
}