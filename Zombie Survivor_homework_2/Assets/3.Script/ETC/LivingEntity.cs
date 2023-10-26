using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage
{
    // 모든 생명체에게 붙일 컴포넌트
    /*
        전체 체력
        현재 체력
        생존여부 -> 이벤트로 처리
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
        // 죽었는지 안죽었는지 판별
        if(currentHealth <= 0 && !isDead)
        {
            // 죽는 메소드를 호출
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
