using UnityEngine;

public class LiquidProperty : MonoBehaviour
{
    public Color liquidColor;

    void Start()
    {
        if (liquidColor == Color.clear)
        {
            liquidColor = GetComponent<Renderer>().material.color;
        }
    }
}