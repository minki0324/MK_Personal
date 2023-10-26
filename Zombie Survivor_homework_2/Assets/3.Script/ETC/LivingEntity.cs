using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage
{
    // ��� ����ü���� ���� ������Ʈ
    /*
        ��ü ü��
        ���� ü��
        �������� -> �̺�Ʈ�� ó��
    */

    public float startHealth = 100f;
    public float currentHealth { get; protected set; }
    public bool isDead { get; protected set; }

    public event Action onDead;
    
    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startHealth;
    }

    public virtual void OnDamage(float Damage, Vector3 hitPos, Vector3 hitNor)
    {
        currentHealth -= Damage;
        // �׾����� ���׾����� �Ǻ�
        if(currentHealth <= 0 && !isDead)
        {
            // �״� �޼ҵ带 ȣ��
            Die();
        }
    }

    public virtual void Die()
    {
        if(onDead != null)
        {
            onDead();
        }

        isDead = true;
    }

    public virtual void Restore_Health(float newHealth)
    {
        if(isDead)
        {
            return;
        }

        currentHealth += newHealth;
    }
}
