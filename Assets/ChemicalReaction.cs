using UnityEngine;

public class ChemicalReaction : MonoBehaviour
{
    public GameObject powder;
    public ParticleSystem foamEffect;
    public float dissolveSpeed = 2f;
    public float liquidRiseSpeed = 0.3f;

    private bool reacting = false;
    private Transform liquidTransform;

    void Start()
    {
        // ‰÷„‰ ≈‰Ê «·€«“ ÿ«›Ì »«·»œ«Ì…
        if (foamEffect != null)
        {
            foamEffect.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Liquid") && !reacting)
        {
            reacting = true;

            //  ‘€Ì· «·€«“
            if (foamEffect != null)
            {
                foamEffect.Play();
            }

            //  Œ“Ì‰ «·”«∆·
            liquidTransform = other.transform;
        }
    }

    void Update()
    {
        if (!reacting) return;

        //  ≈–«»… «·»Êœ—…
        if (powder != null)
        {
            powder.transform.localScale -= Vector3.one * dissolveSpeed * Time.deltaTime;

            if (powder.transform.localScale.x <= 0.05f)
            {
                Destroy(powder);
            }
        }

        //  —›⁄ «·”«∆·  œ—ÌÃÌ«
        if (liquidTransform != null)
        {
            liquidTransform.localScale += new Vector3(0, liquidRiseSpeed * Time.deltaTime, 0);
        }
    }
}