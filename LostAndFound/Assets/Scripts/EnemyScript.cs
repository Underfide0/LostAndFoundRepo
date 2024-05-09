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

    [SerializeField] private Transform cabinTransform;

    [SerializeField] private GameObject comebackUI;

    [SerializeField] private GameObject EscapeUI;

    public bool enemyInCabin;


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
        enemyInCabin = false;
        killing = false;
    }

    
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    
    public void enemyTP()
    {
        transform.position = cabinTransform.position;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cabin"))
        {
            enemyInCabin = true;
            comebackUI.SetActive(false);
            EscapeUI.SetActive(true);   
        }
    }

    public void killPlayer()
    {
        killing = true;
        enemyAnimator.Play("metarig|Kill_Animation");
        agent.SetDestination(transform.position);
        
        transform.localRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
    }
}
