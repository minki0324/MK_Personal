using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
        �Ѿ� -> LineRender -> RayCast
        ��Ÿ�
        �߻�� ��ġ
        GunData
        Effect
        ���� ���� -> Enum {������, źâ�� ����� ��, �߻� �غ�}
        Audio Source

        Method
        �߻� -> Fire
        ������ -> Reload
        Effect
    */

    public Camera camera_;

    public enum State
    {
        Ready,
        Empty,
        Reload
    }

    public State state { get; private set; }

    // �Ѿ��� �߻�� ��ġ
    public Transform Fire_Transform;

    // �Ѿ� ���η�����
    public LineRenderer lineRenderer;

    // �Ѿ� �߻� source
    private AudioSource clip;

    // ��Ÿ�
    private float Distance = 50f;

    // �� Data
    public GunData Data;

    // Effect
    public ParticleSystem shot_effect;
    public ParticleSystem shell_effect;

    private float LastFireTime;
    public int AmmoRemain = 100;

    // ���� �Ѿ� ����
    public int MagAmmo;

    private void Awake()
    {
        clip = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        // ���� ���� ������ ������Ʈ ��Ȱ��ȭ
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        AmmoRemain = Data.StartAmmoRemain;
        MagAmmo = Data.MagCapacity;

        state = State.Ready;
        LastFireTime = 0;
    }

    public void Fire()
    {
        // �÷��̾��� ���� �� ���°� �غ� �����̸鼭 ������ �߻� �ð��� ���� �ð����� ���� �� �߻�
        if(state.Equals(State.Ready) && LastFireTime+Data.TimebetFire <= Time.time)
        {
            LastFireTime = Time.time;
            // �߻�
            Shot();
        }
    }

    public void Shot()
    {
        // �� -> Raycast
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;

        Vector3 gunpos = LookAtMouse();

        if(Physics.Raycast(Fire_Transform.position, gunpos, out hit, Distance))
        {
            // �Ѿ��� �¾��� ��� ���� �������̽��� ������ �ͼ� ���� ������Ʈ���� �������� �����
            IDamage target = hit.collider.GetComponent<IDamage>();

            if(target != null)
            {
                target.OnDamage(Data.Damage, hit.point, hit.normal);
            }
            hitPos = hit.point;
        }
        else
        {
            // Ray�� �ٸ� ��ü�� �浹���� �ʾ��� ���
            // ź���� �ִ� �����Ÿ����� ��������
            hitPos = Fire_Transform.position + Fire_Transform.forward * Distance;
        }
        // ���� �� ����Ʈ �÷���
        StartCoroutine(ShotEffect_co(hitPos));

        // ���� �Ѿ� ���� ����
        MagAmmo--;
        if(MagAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect_co(Vector3 hitpos)
    {
        shot_effect.Play();
        shell_effect.Play();

        clip.PlayOneShot(Data.Shot_Clip);

        // ���� ������ ����
        lineRenderer.SetPosition(0, Fire_Transform.position);
        lineRenderer.SetPosition(1, hitpos);

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;
    }

    public bool Reload()
    {
        // ���� �������� �ʿ����� ������ Return�� �޼ҵ�

        // �̹� ������ �� �̰ų� �Ѿ��� ���ų� źâ�� �̹� �Ѿ��� ������ ��� => false

        if(state.Equals(State.Reload) || AmmoRemain <= 0 || MagAmmo >= Data.MagCapacity)
        {
            return false;
        }
        StartCoroutine(Reload_co());
        return true;
    }

    private IEnumerator Reload_co()
    {
        state = State.Reload;
        clip.PlayOneShot(Data.Reload_Clip);
        yield return new WaitForSeconds(Data.ReloadTime);

        // ������ ���� ���
        int ammofill = Data.MagCapacity - MagAmmo;

        // źâ�� ä���� �� ź���� ���� ź�ຸ�� ���ٸ� ä���� �� ź����� ���� ź����� ���� ���δ�.
        if(AmmoRemain < ammofill)
        {
            ammofill = AmmoRemain;
        }

        // źâ�� ä��� ��ü źâ�� ���� ���δ�.
        MagAmmo += ammofill;
        AmmoRemain -= ammofill;
        state = State.Ready;
    }

    private Vector3 LookAtMouse()
    {
        Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;

        

        Debug.DrawRay(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        if (Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, hitResult.point.y, hitResult.point.z) - transform.position;
            return mouseDir;
        }
        return Vector3.zero;
    }
}
