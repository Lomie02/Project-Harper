using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboObject : MonoBehaviour
{
    AbilityTransition m_Ability;
    Collider m_ObjectsCollider;
    Collider m_PlayersCollider;
    void Start()
    {
        m_ObjectsCollider = GetComponent<Collider>();

        m_PlayersCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();

        m_Ability = FindAnyObjectByType<AbilityTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Ability.IsInLimbo())
        {
            Physics.IgnoreCollision(m_ObjectsCollider, m_PlayersCollider, true);
        }
        else
        {
            Physics.IgnoreCollision(m_ObjectsCollider, m_PlayersCollider, false);
        }
    }
}
