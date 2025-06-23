using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    private NavMeshAgent agent;

    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


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
