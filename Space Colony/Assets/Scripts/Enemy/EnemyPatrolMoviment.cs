using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolMoviment : MonoBehaviour
{
    public Transform target;          // Referencia para a posição do alvo
    PlayerHealth playerHealth;        // Referencia para a vida do jogador
    EnemyHealth enemyHealth;          // Referencia para a vida do inimigo
    NavMeshAgent nav;                 // Referencia para o componente Nav Mesh Agent

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.currentEnemyHealth > 0 && playerHealth.currentPlayerHealth > 0)
        {
            nav.SetDestination(target.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
