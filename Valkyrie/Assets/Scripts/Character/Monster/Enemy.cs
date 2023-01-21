using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : MonoBehaviour, IStatus
{
    public float hp = 100.0f;
    float maxHP = 100.0f;
    public float mp = 100.0f;
    float maxMP = 100.0f;
    public float attackPos = 10.0f;
    public float defensePos = 10.0f;

    public float getExp = 50.0f;

    public Transform patrolRoute;
    Animator anim;
    NavMeshAgent agent;

    EnemyState state = EnemyState.Idle;
    IEnumerator repeatChase = null;

    bool scream = true;
    float waitTime = 3.0f;
    float timeCountDown = 3.0f;
    float screamTime = 1.0f;

    int childCount = 0;
    int index = 0;
  
    public float sightRange = 10.0f;
    public float closeSightRange = 2.5f;
    Vector3 targetPos = new();
    float sightAngle = 150.0f;

    IStatus attackTarget;

    public float attackCooltime = 1.0f;
    public float attackSpeed = 1.0f;

    bool isDead = false;

    public Material[] materials;

    public float HP
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHP);
                onHPchange?.Invoke();
            }
        }
    }

    public float MaxHP => maxHP;

    public float MP
    {
        get => mp;
        set
        {
            if (mp != value)
            {
                mp = Mathf.Clamp(value, 0, maxMP);
                onMPchange?.Invoke();
            }
        }
    }

    public float MaxMP => maxMP;

    public float AttackPos => attackPos;
    public float DefensePos => defensePos;


    public Action onHPchange { get; set; }
    public Action onMPchange { get; set; }

    public Action onDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        this.gameObject.SetActive(false);
        if (GameManager.Inst.IsWarp)
        {
            this.gameObject.SetActive(true);
        }
        if (patrolRoute)
        {
            childCount = patrolRoute.childCount;    // 자식 개수 설정
        }
    }
    bool SearchPlayer()
    {
        bool result = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            Vector3 pos = colliders[0].transform.position;
            if (InSightAngle(pos))
            {
                if (!BlockByWall(pos))
                {
                    targetPos = pos;
                    result = true;
                }
            }
            if (!result && (pos.sqrMagnitude - transform.position.sqrMagnitude) > closeSightRange * closeSightRange)
            {
                targetPos = pos;
                result = true;
            }
        }

        return result;
    }
    public void AttackTarget(IStatus target)
    {
        if (target != null)
        {
            float damage = attackPos;
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defensePos;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        HP -= finalDamage;
        if (HP > 0.0f)
        {
            anim.SetTrigger("Hit");
            attackCooltime = attackSpeed;
        }
        else
        {
            Dead();
        }
    }
    public void AttackAnim()
    {
        AttackTarget(attackTarget);
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
            case EnemyState.Attack:
                AttackUpdate();
                break;
            case EnemyState.Dead:
            default:
                break;
        }
    }



    void IdleUpdate()
    {
        if (SearchPlayer())
        {
            if (scream)
            {
                StartCoroutine(Scream());
                return;
            }
        }
        if (!scream)
        {
            anim.SetBool("Scream", scream);
            if (SearchPlayer())
            {
                ChangeState(EnemyState.Chase);
            }
            timeCountDown -= Time.deltaTime;
            if (timeCountDown < 0.0f)
            {
                ChangeState(EnemyState.Patrol);
                return;
            }
        }
    }
    void PatrolUpdate()
    {
        if (SearchPlayer())
        {
            ChangeState(EnemyState.Chase);
            return;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            index++;
            index %= childCount;

            ChangeState(EnemyState.Idle);
            return;
        }
    }
    void ChaseUpdate()
    {
        if (!SearchPlayer())
        {
            ChangeState(EnemyState.Patrol);
            return;
        }
    }
    void AttackUpdate()
    {
        attackCooltime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if (attackCooltime < 0.0f)
        {
            anim.SetTrigger("BasicAttack");
            attackCooltime = attackSpeed;
        }
    }
    void ChangeState(EnemyState newState)
    {
        if (isDead)
        {
            return;
        }
        switch (state)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                break;
            case EnemyState.Patrol:
                agent.isStopped = true;
                break;
            case EnemyState.Chase:
                agent.isStopped = true;
                StopCoroutine(repeatChase);
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                attackTarget = null;
                break;
            case EnemyState.Dead:
                agent.isStopped = true;
                isDead = false;
                break;
            default:
                break;
        }

        switch (newState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                timeCountDown = waitTime;
                break;
            case EnemyState.Patrol:
                agent.isStopped = false;
                agent.speed = 5;
                agent.SetDestination(patrolRoute.GetChild(index).position);
                break;
            case EnemyState.Chase:
                agent.isStopped = false;
                agent.speed = 7;
                agent.SetDestination(targetPos);
                repeatChase = RepeatChase();
                StartCoroutine(repeatChase);
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                attackCooltime = attackSpeed;
                break;
            case EnemyState.Dead:
                gameObject.layer = LayerMask.NameToLayer("Default");
                onDead?.Invoke();
                anim.SetBool("Dead", true);
                anim.SetTrigger("Die");
                isDead = true;
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                HP = 0;
                GameManager.Inst.MainPlayer.Exp += getExp;
                StartCoroutine(DeadEffect());
                ItemDrop();
                break;
            default:
                break;
        }
        state = newState;
        anim.SetInteger("EnemyState", (int)state);
    }
    IEnumerator Scream()
    {
        anim.SetBool("Scream", scream);
        yield return new WaitForSeconds(screamTime);
        scream = false;
    }
    IEnumerator DeadEffect()
    {
        Material myM = GetComponentInChildren<Material>();
        myM = materials[1];
        yield return new WaitForSeconds(3.0f);
    }
    IEnumerator RepeatChase()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            agent.SetDestination(targetPos);
        }
    }
    bool InSightAngle(Vector3 targetPosition)
    {
        float angle = Vector3.Angle(transform.forward, targetPosition - transform.position);

        return (sightAngle * 0.5f) > angle;
    }
    public void Dead()
    {
        if (!isDead)
        {
            GameManager.Inst.Quest.QuestPoint++;
            ChangeState(EnemyState.Dead);
        }
    }
    void ItemDrop()
    {
        ItemFactory.MakeItem(ItemCode.HealingPotion, transform.position, true);
    }
    bool BlockByWall(Vector3 targetPosition)
    {
        bool result = true;
        Ray ray = new(transform.position, targetPosition - transform.position);
        ray.origin += Vector3.up * 0.5f; 
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            if (hit.collider.CompareTag("Player"))    
            {
                result = false; 
            }
        }

        return result;  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            attackTarget = other.GetComponent<IStatus>();
            ChangeState(EnemyState.Attack);
            return;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Inst.MainPlayer.gameObject)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);


        Handles.color = Color.green;
        if (state == EnemyState.Chase || state == EnemyState.Attack)
        {
            Handles.color = Color.red;  // 추적이나 공격 중일 때만 빨간색
        }
        Handles.DrawWireDisc(transform.position, transform.up, closeSightRange); // 근접 시야 범위

        Vector3 forward = transform.forward * sightRange;
        Quaternion q1 = Quaternion.Euler(0.5f * sightAngle * transform.up);
        Quaternion q2 = Quaternion.Euler(-0.5f * sightAngle * transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);    // 시야각 오른쪽 끝
        Handles.DrawLine(transform.position, transform.position + q2 * forward);    // 시야각 왼쪽 끝

        Handles.DrawWireArc(transform.position, transform.up, q2 * transform.forward, sightAngle, sightRange, 5.0f);// 전체 시야범위
    }
#endif
}
