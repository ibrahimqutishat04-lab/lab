using UnityEngine;

public class PourController1 : MonoBehaviour
{
    public ParticleSystem pourParticles; // اربط الـ Particle System هنا
    public float pourThresholdAngle = 45f; // الزاوية اللي ببدأ عندها الصب

    void Update()
    {
        // تحقق من زاوية ميل الكأس
        bool isPouring = Vector3.Angle(Vector3.up, transform.up) > pourThresholdAngle;

        // إذا مالت الكأس بما يكفي، شغل السائل، وإلا وقفه
        if (isPouring)
        {
            if (!pourParticles.isPlaying) pourParticles.Play();
        }
        else
        {
            if (pourParticles.isPlaying) pourParticles.Stop();
        }
    }
}