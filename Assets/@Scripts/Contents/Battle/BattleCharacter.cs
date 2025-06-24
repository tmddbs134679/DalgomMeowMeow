using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleCharacter : MonoBehaviour
{
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDelay = 1f; // 공격 딜레이 (초 단위)
    
    public float attackDamage = 10f; // 공격력
    public float heatlh = 100f; // 체력
    public float moveSpeed = 3.5f; // 이동 속도





    private float attacktimer = 0f; 

    private NavMeshAgent agent;
    private Transform targetEnemy;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed; // NavMeshAgent의 이동 속도 설정
        agent.stoppingDistance = attackRange; // 공격 범위 내에서 멈추도록 설정
    }



    void Update()
    {
        if (targetEnemy == null)
        {
            targetEnemy = FindClosestEnemyInRange(detectRange);
            if (targetEnemy != null)
                agent.SetDestination(targetEnemy.position);
        }
        else
        {
            float dist = Vector3.Distance(transform.position, targetEnemy.position);

            if (dist <= attackRange)
            {
                agent.isStopped = true; //거리가 가까우면 공격
                TryAttack();
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(targetEnemy.position); // 계속 추적
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    private Transform FindClosestEnemyInRange(float range)
    {
        int enemyLayerMask = LayerMask.GetMask("Enemy");
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayerMask);

        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(myPos, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }
        return closest;
    }

    public void TryAttack()
    {
        attacktimer += Time.deltaTime;
        if (attacktimer >= attackDelay)
        {
            //Attack();
            Debug.Log("Attack! Damage: " + attackDamage);
            attacktimer = 0f; // 공격 후 타이머 초기화
        }
    }
}
