using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieControl : LivingEntity
{
    [Header("������ ��� ���̾�")]
    public LayerMask targetLayer;
    private LivingEntity targetEntity;

    // ��θ� ����� AI agent
    private NavMeshAgent agnet;

    [Header("ȿ��")]

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
            GetComponentInChildren = Ư�� ������Ʈ�� ���� ��ü �߿� ���� ���ο� �ִ� ������Ʈ�� ��ȯ
            GetComponentsInChildren = Ư�� ������Ʈ�� ���� ��ü(��ü)���� ������Ʈ�� ��ȯ �̶� ��ȯ ���´� �迭�� ��ȯ
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
            ������ ����
            �÷��̾� ���� �Ѿ��� �¾��� �� 
            �Ѿ��� ������ �Ҹ������ �ϰ�
            Hiteffect �Ѿ��� ���ƿ� ��������
        */

        if(!isDead)
        {
            hitEffect.transform.position = hitPos;
            // hit ȸ������ �ٶ󺸴� ȸ���� ���·� ��ȯ
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
        // ��� ���� �� -> ������ ȣ��
        /*
            enter > ��� ����
            stay  > ��� ���� ��
            exit  > ��� ���� ������ ��
        */

        if (!isDead && Time.time >= LastAttackTimebet + TimebetATK) 
        {
            if(other.TryGetComponent(out LivingEntity e))
            {
                if(targetEntity.Equals(e))
                {
                    LastAttackTimebet = Time.time;

                    // closestpoint = ��� ��ġ
                    // �� ���� �ǰ� ��ġ�� �ǰ� ������ �ٻ簪���� ���
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
