using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;
    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemdropClip;

    private AudioSource playerAudio;
    private Animator playerAni;

    private PlayerMovement playerMove;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAni = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // �θ��� �޼ҵ带 ȣ��
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = currentHealth;

        // �׾����� movement�ϰ� shooter ��Ȱ��ȭ �Ұű� ������ Ȱ��ȭ�ϸ� ���ֱ�
        playerMove.enabled = true;
        playerShooter.enabled = true;
    }
    public override void OnDamage(float Damage, Vector3 hitPos, Vector3 hitNor)
    {
        if(!isDead)
        {
            playerAudio.PlayOneShot(hitClip);
        }

        base.OnDamage(Damage, hitPos, hitNor);
        healthSlider.value = currentHealth;
    }

    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);
        // �ð��� ȿ��, û���� ȿ���� �ֱ�

        playerAni.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);

        playerMove.enabled = false;
        playerShooter.enabled = false;
    }
}
