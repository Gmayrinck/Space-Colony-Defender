using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoviment : MonoBehaviour
{
    Transform player;                 // Referencia para a posição do jogador
    PlayerHealth playerHealth;        // Referencia para a vida do jogador
    EnemyHealth enemyHealth;          // Referencia para a vida do inimigo
    NavMeshAgent nav;                 // Referencia para o componente Nav Mesh Agent
    Animator anim;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
       
    // Update is called once per frame
    void Update()
    {
        if(enemyHealth.currentEnemyHealth > 0 && playerHealth.currentPlayerHealth > 0)
        {
            nav.SetDestination(player.position);
            bool moving = true;
            anim.SetBool("IsMoving", moving);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
