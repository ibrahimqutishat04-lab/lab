using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    public TextMeshProUGUI weightText;
    private float totalWeight = 0f;
    private List<GameObject> objectsOnScale = new List<GameObject>();

    void Start()
    {
        UpdateUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ItemWeight item = collision.gameObject.GetComponent<ItemWeight>();
        if (item != null && !objectsOnScale.Contains(collision.gameObject))
        {
            objectsOnScale.Add(collision.gameObject);
            totalWeight += item.weightValue;
            UpdateUI();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ItemWeight item = collision.gameObject.GetComponent<ItemWeight>();
        if (item != null && objectsOnScale.Contains(collision.gameObject))
        {
            objectsOnScale.Remove(collision.gameObject);
            totalWeight -= item.weightValue;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        weightText.text = totalWeight.ToString() + "g";
    }
}