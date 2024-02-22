using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // 目标点的Transform
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent组件未附加到对象上.");
        }
        else
        {
            SetDestination();
        }
    }

    void SetDestination()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
