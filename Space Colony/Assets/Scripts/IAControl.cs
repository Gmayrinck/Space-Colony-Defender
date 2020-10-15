using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAControl : MonoBehaviour
{
    public enum Estados
    {
        Esperar,
        Patrulhar,
        Perseguir,
        Procurar
    }

    private Estados estadoAtual;

    [Header("Estado: Esperar")]
    public float tempoEsperar = 2f;
    float tempoEsperando;

    [Header("Estado: Patrulhar")]
    public Transform waypoint1;
    public Transform waypoint2;
    
    private Transform waypointAtual;
    private float distanciaWaypointAtual;
    public float distanciaMinimaWaypoint = 1f;

    [Header("Estado: Perseguir")]
    public float campoVisao = 3f;

    private float distanciaJogador;
    private GameObject jogador;

    [Header("Estado: Procurar")]
    public float tempoPersistencia = 2f;
    private float tempoSemVisao;

    private EnemyPatrolMoviment enemyPatrolMoviment;
    private Transform alvo;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        enemyPatrolMoviment = GetComponent<EnemyPatrolMoviment>();

        jogador = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();

        waypointAtual = waypoint1;

        Esperar();
    }

    // Update is called once per frame
    void Update()
    {
        ChecarEstados();
    }

    void ChecarEstados()
    {
        if (estadoAtual != Estados.Perseguir && PossuiVisaoJogador())
        {
            Perseguir();

            return;
        }        

        switch (estadoAtual)
        {
            case Estados.Esperar:
                if (EsperouTempoSuficiente())
                {
                    Patrulhar();
                }
                else
                {
                    alvo = transform;                    
                }

                break;
            case Estados.Patrulhar:
                if (PertoWaypointAtual())
                {
                    Esperar();

                    AlterarWaypoint();
                }
                else
                {
                    alvo = waypointAtual;
                }

                break;
            case Estados.Perseguir:
                if (!PossuiVisaoJogador())
                {
                    Procurar();
                }
                else
                {
                    alvo = jogador.transform;
                }

                break;
            case Estados.Procurar:
                if (SemVisaoTempoSuficiente())
                {
                    Esperar();
                }              

                break;
        }

        enemyPatrolMoviment.target = alvo;
    }

    #region ESPERAR
    void Esperar()
    {
        estadoAtual = Estados.Esperar;

        tempoEsperando = Time.time;

        bool moving = false;
        anim.SetBool("IsMoving", moving);
    }

    bool EsperouTempoSuficiente()
    {
        return tempoEsperando + tempoEsperar <= Time.time;
    }
    #endregion Esperar

    #region PATRULHAR
    void Patrulhar()
    {
        estadoAtual = Estados.Patrulhar;
        bool moving = true;
        anim.SetBool("IsMoving", moving);
    }

    bool PertoWaypointAtual()
    {
        distanciaWaypointAtual = Vector3.Distance(transform.position, waypointAtual.position);

        return distanciaWaypointAtual <= distanciaMinimaWaypoint;
    }

    void AlterarWaypoint()
    {
        waypointAtual = (waypointAtual == waypoint1) ? waypoint2 : waypoint1; // uso do if ternario
    }
    #endregion PATRULHAR

    #region PERSEGUIR
    void Perseguir()
    {
        estadoAtual = Estados.Perseguir;
        bool moving = true;
        anim.SetBool("IsMoving", moving);
    }

    bool PossuiVisaoJogador()
    {
        distanciaJogador = Vector3.Distance(transform.position, jogador.transform.position);

        return distanciaJogador <= campoVisao;
    }

    #endregion PERSEGUIR

    #region PROCURAR
    void Procurar()
    {
        estadoAtual = Estados.Procurar;

        tempoSemVisao = Time.time;

        alvo = null;
        bool moving = false;
        anim.SetBool("IsMoving", moving);
    }

    bool SemVisaoTempoSuficiente()
    {
        return tempoSemVisao + tempoPersistencia <= Time.time; ;
    }

    #endregion PROCURAR

}
