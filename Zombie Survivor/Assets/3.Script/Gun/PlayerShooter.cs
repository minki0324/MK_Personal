using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*
        �ѽ��, ������ -> Gun
        player input
        �� ������Ʈ �տ� ���߱� -> animator
    */

    public Gun gun;
    // �ѱ� ��ġ ���߱� ���� Transform
    public Transform gunPivot;
    public Transform leftHand_mount;
    public Transform RightHand_mount;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput input;

    private void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // input�� ���õ� �̺�Ʈ ȣ��
        if(input.isFire)
        {
            gun.Fire();
        }
        else if(input.isReload)
        {
            if(gun.Reload())
            {
                animator.SetTrigger("Reload");
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // ���� �������� ������ �Ȳ�ġ�� �̵�
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // ik�� ����Ͽ� �޼��� ��ġ�� ȸ���� �� ���� �����̿� ����
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand_mount.rotation);

        // ik�� ����Ͽ� �������� ��ġ�� ȸ���� �� �����̿� ����
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand_mount.rotation);

    }
}
