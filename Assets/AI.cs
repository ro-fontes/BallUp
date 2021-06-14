using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    NavMeshAgent agent; //navmeshAgent
    float rotSpeed = 5.0f; //Velocidade de rotacao

    //Criando as variaveis
    [Header("Random Area")]
    //Setando a area randômica do inimigo
    [Range(-1000, 1000)]
    public float MaxZ = 0;
    [Range(-1000, 1000)]
    public float MinZ = 0;
    [Range(-1000, 1000)]
    public float MaxX = 0;
    [Range(-1000, 1000)]
    public float MinX = 0;

    [Header("Enemy Config")]

    [Tooltip("Transform do Player")]
    public GameObject player;       //Gameobj do Player
    [Tooltip("Range da visao do inimigo")]
    [Range(0,200)]
    public float visibleRange = 30.0f; // Distancia de visao do inimigo
    public Vector3 target;      // posicao do inimigo.

    void Start()
    {
        //Pegando o NavMeshAgent do Gameobject
        agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    //Setando uma posicao aleatoria com o SetDestination
    [Task]
    public void PickRandomDestination()
    {
        Vector3 dest = new Vector3(Random.Range(MaxX, MinX), 2, Random.Range(MaxZ, MinZ)); // definindo o destino
        agent.SetDestination(dest); //Setando o destino do inimigo 
        Task.current.Succeed();
    }

    //Movendo o pesonagem ate a posicoes
    [Task]
    public void MoveToDestination()
    {
        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        }
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();
        }
    }

    //Patrol
    [Task]
    public void PickDestination(int x, int z)
    {
        Vector3 dest = new Vector3(x, 0, z);
        //setando o destino do navmesh para uma posicao definida
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void TargetPlayer()
    {
        target = player.transform.position; //Pegando a posicao do jogador
        Task.current.Succeed();
    }

    [Task]
    public void LookAtTarget()
    {
        //Direcao que esta apontando
        Vector3 direction = target - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

        Task.current.Succeed();
    }

    [Task]
    public bool Position()
    {
        agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        return true;
    }

    [Task]
    public bool SeePlayer()
    {
        //Distancia do player com o personagem
        Vector3 distance = player.transform.position - this.transform.position;
        bool seeWall = false;

        //Criando um rayCast
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, distance, Color.red);
        if (Physics.Raycast(this.transform.position, distance, out hit))
        {
            //se o raycast colidir com a parede
            if (hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Water")
            {
                seeWall = true;
            }
        }
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("wall={0}", seeWall);

        if (distance.magnitude < visibleRange && !seeWall)
            return true;
        else
            return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Caso o inimigo colida com o player, ele ira morrer e respawnar no inicio da fase
        if (other.CompareTag("Player"))
        {
            player.GetComponent<Player>().SpawnPlayer();
        }
    }
}

