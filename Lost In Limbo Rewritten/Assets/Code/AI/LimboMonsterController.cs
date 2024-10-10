using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;

enum AiBehaviourStates
{
    Idle = 0,
    Roaming,
    Attack,
    ChasePlayer,
    Death,
}
public class LimboMonsterController : MonoBehaviour
{
    float m_MonsterHealth = 0;
    float m_MonsterMaxHealth = 100;

    [SerializeField] UnityEvent m_OnDeath;

    [SerializeField] float m_MonstersViewRangeOnPlayer = 5f;
    [SerializeField] float m_MonsterDetectionSpherecast = 2f;

    [SerializeField] Transform m_RayCastBox;
    [SerializeField] LayerMask m_RayCastLayer;
    [Space]

    NavMeshAgent m_Agent;
    [SerializeField] float m_RoamingDistance = 10;
    [SerializeField] AiBehaviourStates m_AiState;

    float m_WaitingToMoveTimer = 0;
    float m_WaitForDuration = 4;

    Animator m_MonsterAnimations;
    [Space]
    [SerializeField] Transform m_PlayersLocation;
    [SerializeField] Transform m_PlayersHeadLocation = null;

    public bool m_CanSeeThePlayer = false;
    [SerializeField] MultiAimConstraint m_AimingConstrain;

    float m_SearchForPlayerTimer = 0;
    float m_SearchingDuration = 10;

    Material m_MonsterMaterial;

    float m_AttackCooldownTimer = 0;
    float m_AttackCooldownDuration = 2;
    bool m_AttackOnCooldown = false;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_MonsterAnimations = GetComponentInChildren<Animator>();

        m_WaitForDuration = Random.Range(3, 7);
        m_WaitingToMoveTimer = m_WaitForDuration;

        m_SearchForPlayerTimer = m_SearchingDuration;

        m_AttackCooldownTimer = m_AttackCooldownDuration;
        m_MonsterHealth = m_MonsterMaxHealth;
        m_MonsterMaterial = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_AiState)
        {
            case AiBehaviourStates.Idle:
                m_MonsterAnimations.SetBool("IsMoving", false);

                if (m_Agent.isStopped)
                    m_Agent.isStopped = false;

                m_WaitingToMoveTimer -= Time.deltaTime;

                if (m_WaitingToMoveTimer <= 0)
                {
                    m_AiState = AiBehaviourStates.Roaming;
                    m_WaitForDuration = Random.Range(3, 7);
                    m_WaitingToMoveTimer = m_WaitForDuration;
                }
                break;

            case AiBehaviourStates.Roaming:

                UpdateSeekingBehaviour();
                m_MonsterAnimations.SetBool("IsMoving", true);
                break;

            case AiBehaviourStates.ChasePlayer:
                m_MonsterAnimations.SetBool("IsMoving", true);
                UpdateSeekingPlayerBehaviour();
                break;

            case AiBehaviourStates.Death:

                m_MonsterMaterial.SetFloat("_NoiseHeight", Mathf.Lerp(m_MonsterMaterial.GetFloat("_NoiseHeight"), -3, Time.deltaTime));

                if (m_MonsterMaterial.GetFloat("_NoiseHeight") <= -1.01015f)
                {
                    Destroy(gameObject);
                }

                break;

            case AiBehaviourStates.Attack:
                AttackPlayer();
                break;
        }
        if (m_AttackOnCooldown)
        {
            m_AttackCooldownTimer -= Time.deltaTime;
            if (m_AttackCooldownTimer <= 0)
            {
                m_AttackCooldownTimer = m_AttackCooldownDuration;
                m_AttackOnCooldown = false;
            }
        }
    }

    public void TakeDamage(float _amount)
    {
        m_MonsterHealth -= _amount;

        m_AiState = AiBehaviourStates.ChasePlayer;
        if (m_MonsterHealth <= 0)
        {
            m_OnDeath.Invoke();
            m_Agent.isStopped = true;
            m_AiState = AiBehaviourStates.Death;
        }
    }
    void AttackPlayer()
    {
        m_MonsterAnimations.SetTrigger("Attack");
        m_AttackOnCooldown = true;
    }

    public void AttackRayCheck()
    {
        RaycastHit m_CastInfo;
        if (Physics.Raycast(transform.position, transform.forward, out m_CastInfo, 2, m_RayCastLayer))
        {
            if (m_CastInfo.collider.tag == "Player")
            {
                m_CastInfo.collider.gameObject.GetComponent<PlayerController>().TakeDamage(30);
            }
        }

    }

    void UpdateSeekingBehaviour()
    {
        if (m_Agent.remainingDistance < 1)
        {
            int GoToIdle = Random.Range(0, 3);

            if (GoToIdle == 1)
                m_AiState = AiBehaviourStates.Idle;
            else
                m_Agent.SetDestination(RandomNavSphere(transform.position, m_RoamingDistance, -1));
        }

        RaycastHit m_CastInfo;
        if (Physics.SphereCast(transform.position, 5f, transform.forward, out m_CastInfo, m_MonsterDetectionSpherecast, m_RayCastLayer))
        {
            if (m_CastInfo.collider.tag == "Player")
            {
                m_Agent.SetDestination(m_PlayersLocation.position);
                m_AiState = AiBehaviourStates.ChasePlayer;
            }
            else if (m_CastInfo.collider.gameObject.GetComponent<DoorModule>())
            {
                m_CastInfo.collider.gameObject.GetComponent<DoorModule>().CycleDoorStates();
            }
        }

    }

    void UpdateSeekingPlayerBehaviour()
    {
        RaycastHit m_CastInfo;

        Vector3 DirectionOfPlayerToMonster = m_PlayersLocation.position - transform.position;
        m_Agent.SetDestination(m_PlayersLocation.position);

        if (Physics.Raycast(transform.position, DirectionOfPlayerToMonster, out m_CastInfo, m_MonstersViewRangeOnPlayer, m_RayCastLayer))
        {
            if (m_CastInfo.collider.tag == "Player")
            {
                m_SearchForPlayerTimer = m_SearchingDuration;
                m_CanSeeThePlayer = true;
            }
            else
                m_CanSeeThePlayer = false;
        }


        if (Physics.Raycast(transform.position, transform.forward, out m_CastInfo, 2, m_RayCastLayer))
        {
            if (m_CastInfo.collider.tag == "Player" && !m_AttackOnCooldown)
            {
                AttackPlayer();
            }

            if (m_CastInfo.collider.gameObject.GetComponent<DoorModule>())
            {
                m_CastInfo.collider.gameObject.GetComponent<DoorModule>().CycleDoorStates();
            }
        }

        if (!m_CanSeeThePlayer)
        {
            m_SearchForPlayerTimer -= Time.deltaTime;

            if (m_SearchForPlayerTimer <= 0)
            {
                m_SearchForPlayerTimer = m_SearchingDuration;
                m_CanSeeThePlayer = false;
                m_AiState = AiBehaviourStates.Idle;
            }
            m_AimingConstrain.weight = Mathf.Lerp(m_AimingConstrain.weight, 0f, 1 * Time.deltaTime);
        }
        else
        {
            m_AimingConstrain.weight = Mathf.Lerp(m_AimingConstrain.weight, 1f, 1 * Time.deltaTime);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
