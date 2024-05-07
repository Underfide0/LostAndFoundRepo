using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private void Update()
    {
        ChasePlayer();
    }
    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
}
