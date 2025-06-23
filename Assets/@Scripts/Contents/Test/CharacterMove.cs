using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private Vector3 _patrolRadius = new Vector3(2, 0, 2);
    private NavMeshAgent agent;
    private float _patrolDelay = 1f;
    private float _patrolTimer = 0f;

    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        
    }

    public void PatrolMove()
    {
        if (agent.isPathStale)
        {
            agent.ResetPath();
        }

        if (agent.hasPath)
        {
            return;
        }

        else
        {
            _patrolTimer += Time.deltaTime;
            if (_patrolTimer < _patrolDelay)
                return;
            Patrol();
            _patrolTimer = 0f;
        }
    }

    private void Patrol()
    {
        agent.SetDestination(RandomDestination(this.transform.position, _patrolRadius));
    }

    private Vector3 RandomDestination(Vector3 curPos, Vector3 halfExtents, int areaMask = NavMesh.AllAreas)
    {
        for (int i = 0; i < 10; i++)
        {
            var random = curPos + new Vector3(
                Random.Range(-halfExtents.x, halfExtents.x),
                0f,
                Random.Range(-halfExtents.z, halfExtents.z)
            );

            if (NavMesh.SamplePosition(random, out var hit, 1f, areaMask))
                return hit.position;
        }
        return curPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        //인사모션
    }
    //////////////////////////////////////////////////////////////////////////////

    [ContextMenu("Move 1")]
    public void Move1()
    {
        agent.SetDestination(cube1.gameObject.transform.position);
    }

    [ContextMenu("Move 2")]
    public void Move2()
    {
        agent.SetDestination(cube2.gameObject.transform.position);
    }
    [ContextMenu("Move 3")]
    public void Move3()
    {
        agent.SetDestination(cube3.gameObject.transform.position);
    }
    
    [ContextMenu("Move 4")]
    public void Move4()
    {
        agent.SetDestination(cube4.gameObject.transform.position);
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        agent.ResetPath();
    }
}
