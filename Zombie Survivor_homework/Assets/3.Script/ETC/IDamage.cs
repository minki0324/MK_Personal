using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    // �Ű����� ���ط�, ���� ��ġ, ���� ����
    void OnDamage(float Damage, Vector3 hitPos, Vector3 hitNor);
}