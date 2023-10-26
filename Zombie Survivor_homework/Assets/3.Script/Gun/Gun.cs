using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
        총알 -> LineRender -> RayCast
        사거리
        발사될 위치
        GunData
        Effect
        총의 상태 -> Enum {재장전, 탄창이 비었을 때, 발사 준비}
        Audio Source

        Method
        발사 -> Fire
        재장전 -> Reload
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

    // 총알이 발사될 위치
    public Transform Fire_Transform;

    // 총알 라인렌더러
    public LineRenderer lineRenderer;

    // 총알 발사 source
    private AudioSource clip;

    // 사거리
    private float Distance = 50f;

    // 총 Data
    public GunData Data;

    // Effect
    public ParticleSystem shot_effect;
    public ParticleSystem shell_effect;

    private float LastFireTime;
    public int AmmoRemain = 100;

    // 현재 총알 개수
    public int MagAmmo;

    private void Awake()
    {
        clip = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        // 총을 쏘지 않을땐 컴포넌트 비활성화
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
        // 플레이어의 현재 총 상태가 준비 상태이면서 마지막 발사 시간이 현재 시간보다 작을 때 발사
        if(state.Equals(State.Ready) && LastFireTime+Data.TimebetFire <= Time.time)
        {
            LastFireTime = Time.time;
            // 발사
            Shot();
        }
    }

    public void Shot()
    {
        // 총 -> Raycast
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;

        Vector3 gunpos = LookAtMouse();

        if(Physics.Raycast(Fire_Transform.position, gunpos, out hit, Distance))
        {
            // 총알이 맞았을 경우 만든 인터페이스를 가지고 와서 맞은 오브젝트한테 데미지를 줘야함
            IDamage target = hit.collider.GetComponent<IDamage>();

            if(target != null)
            {
                target.OnDamage(Data.Damage, hit.point, hit.normal);
            }
            hitPos = hit.point;
        }
        else
        {
            // Ray가 다른 물체와 충돌되지 않았을 경우
            // 탄알이 최대 사정거리까지 날라갔을때
            hitPos = Fire_Transform.position + Fire_Transform.forward * Distance;
        }
        // 총을 쏜 이펙트 플레이
        StartCoroutine(ShotEffect_co(hitPos));

        // 현재 총알 개수 감소
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

        // 라인 렌더러 설정
        lineRenderer.SetPosition(0, Fire_Transform.position);
        lineRenderer.SetPosition(1, hitpos);

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;
    }

    public bool Reload()
    {
        // 현재 재장전이 필요한지 안한지 Return할 메소드

        // 이미 재장전 중 이거나 총알이 없거나 탄창에 이미 총알이 가득한 경우 => false

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

        // 재장전 후의 계산
        int ammofill = Data.MagCapacity - MagAmmo;

        // 탄창에 채워야 할 탄약이 남은 탄약보다 많다면 채워야 할 탄약수를 남은 탄약수에 맞춰 줄인다.
        if(AmmoRemain < ammofill)
        {
            ammofill = AmmoRemain;
        }

        // 탄창을 채우고 전체 탄창의 수를 줄인다.
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
