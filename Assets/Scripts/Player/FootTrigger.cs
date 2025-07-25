using UnityEngine;
using System.Collections;

public class FootTrigger : MonoBehaviour
{
    [SerializeField] private FootstepSound footstepController;
    [SerializeField] private int footIndex;

    private bool[] _footPlaying = new bool[2];

    private void OnTriggerEnter(Collider other)
    {
        // ��������� ����������� � ������ ����
        if (other.TryGetComponent(out Terrain terrain) && _footPlaying[footIndex] == false)
        {
            //StartCoroutine(PlayFootstepWithCooldown(footIndex));
        }
    }

    //private IEnumerator PlayFootstepWithCooldown(int index)
    //{
    //    _footPlaying[index] = true;

    //    // ������������� ���� � �������� ���������
    //    (AudioClip clip, float pitch) = footstepController.PlayFootstepSound();

    //    if (clip != null)
    //    {
    //        // ������������ ������������ � ������ pitch
    //        float duration = clip.length / Mathf.Abs(pitch);

    //        // ���� 110% �� ������������ �����
    //        yield return new WaitForSeconds(clip.length);
    //    }
    //    else
    //    {
    //        // ����������� �������� ���� ����� ���
    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    // ������������ ����
    //    _footPlaying[index] = false;
    //}
}