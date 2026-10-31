using UnityEngine;

public class FlameController : MonoBehaviour
{
    public ParticleSystem flameParticles; // ���� ������ �����
    public Color normalBlue = new Color(0.05f, 0.72f, 0.95f, 1f); // ����� ������ �������
    public Color halogenGreen = new Color(0f, 1f, 0.25f, 1f); // ������ (����� �������)

    private void OnTriggerEnter(Collider other)
    {
        // 1. ����� ����� �� ����� ���� ��� ����� �� ����� ��������
        // 2. ������ �� ��� ����� ���� ������ (hasSample)�
        CopperWireLogic wire = other.GetComponent<CopperWireLogic>();

        if (wire != null && wire.hasSample)
        {
            // ��� ��� ����� ������ ������ɡ ���� ����� ������
            var main = flameParticles.main;
            main.startColor = halogenGreen;
            Debug.Log("����� �������� ����: �� ������ �������!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ��� ����� ����� �� ����ѡ ���� ����� ����� ������ �������
        if (other.GetComponent<CopperWireLogic>() != null)
        {
            var main = flameParticles.main;
            main.startColor = normalBlue;
        }
    }
}
