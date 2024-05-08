using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    [SerializeField] private Animator enemyAnimator;

    private bool killing;


    private void Update()
    {
        if (!killing)
        {
            ChasePlayer();
        }
       
    }
    
    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        killing = false;
        


    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void killPlayer()
    {
        killing = true;
        enemyAnimator.Play("metarig|Kill_Animation");
        agent.SetDestination(transform.position);
        //transform.eulerAngles = new Vector3 (0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
    }
}
