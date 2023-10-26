using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControl : LivingEntity
{
    [Header("추적할 대상 레이어")]
    public LayerMask targetLayer;
    private LivingEntity targetEntity;

    // 경로를 계산할 AI agent
    private NavMeshAgent agnet;

    [Header("효과")]

    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private ParticleSystem hitEffect;

    private Animator zombieAni;
    private AudioSource zombieAudio;

    private Renderer Zombie_renderer;

    [Header("Info")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float TimebetATK = 0.5f;
    private float LastAttackTimebet;

    private bool isTarget
    {
        get
        {
            if(targetEntity != null && !targetEntity.isDead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        TryGetComponent(out agnet);
        TryGetComponent(out zombieAni);
        TryGetComponent(out zombieAudio);

        /*
            GetComponentInChildren = 특정 컴포넌트의 하위 객체 중에 가장 선두에 있는 컴포넌트를 반환
            GetComponentsInChildren = 특정 컴포넌트의 하위 객체(전체)들의 컴포넌트를 반환 이때 반환 형태는 배열로 반환
        */

        Zombie_renderer = GetComponentInChildren<Renderer>();
    }

    public void Setup(ZombieData Data)
    {
        startHealth = Data.Health;
        currentHealth = Data.Health;
        damage = Data.Damage;
        agnet.speed = Data.Speed;
        Zombie_renderer.material.color = Data.Skincolor;
    }

    public override void OnDamage(float Damage, Vector3 hitPos, Vector3 hitNor)
    {
        /*
            좀비의 입장
            플레이어 한테 총알을 맞았을 때 
            총알을 맞으면 소리내줘야 하고
            Hiteffect 총알이 날아온 방향으로
        */

        if(!isDead)
        {
            hitEffect.transform.position = hitPos;
            // hit 회전값을 바라보는 회전의 상태로 변환
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNor);
            hitEffect.Play();

            zombieAudio.PlayOneShot(hitClip);
        }
        base.OnDamage(Damage, hitPos, hitNor);
    }

    public override void Die()
    {
        base.Die();

        Collider[] cols = GetComponents<Collider>();
        foreach(Collider c in cols)
        {
            c.enabled = false;
        }

        agnet.isStopped = true;
        agnet.enabled = false;

        zombieAni.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        // 닿고 있을 때 -> 지속적 호출
        /*
            enter > 닿기 시작
            stay  > 닿고 있을 때
            exit  > 닿는 것이 끝났을 때
        */

        if (!isDead && Time.time >= LastAttackTimebet + TimebetATK) 
        {
            if(other.TryGetComponent(out LivingEntity e))
            {
                if(targetEntity.Equals(e))
                {
                    LastAttackTimebet = Time.time;

                    // closestpoint = 닿는 위치
                    // 즉 상대방 피격 위치와 피격 방향을 근사값으로 계산
                    Vector3 hitpoint = other.ClosestPoint(transform.position);

                    Vector3 hitnormal = transform.position - other.transform.position;

                    e.OnDamage(damage, hitpoint, hitnormal);
                }
            }
        }
    }

    private void Update()
    {
        zombieAni.SetBool("HasTarget", isTarget);
    }
    private void Start()
    {
        StartCoroutine(Update_TargetPos());
    }
    private IEnumerator Update_TargetPos()
    {
        while(!isDead)
        {
            if (isTarget)
            {
                agnet.isStopped = false;
                agnet.SetDestination(targetEntity.transform.position);
            }
            else
            {
                agnet.isStopped = true;
                Collider[] col = Physics.OverlapSphere(transform.position, 20f, targetLayer);
                for(int i = 0; i < col.Length; i++)
                {
                    if(col[i].TryGetComponent(out LivingEntity e))
                    {
                        if(!e.isDead)
                        {
                            targetEntity = e;
                            break;
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
