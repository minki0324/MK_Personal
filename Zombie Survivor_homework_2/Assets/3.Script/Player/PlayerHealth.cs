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
        base.OnEnable(); // 부모의 메소드를 호출
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = currentHealth;

        // 죽었을때 movement하고 shooter 비활성화 할거기 때문에 활성화하면 켜주기
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
        // 시각적 효과, 청각적 효과를 넣기

        playerAni.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);

        playerMove.enabled = false;
        playerShooter.enabled = false;
    }
}
