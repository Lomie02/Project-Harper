using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorParenter : MonoBehaviour
{
    [SerializeField] Transform m_ParentTarget;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = m_ParentTarget.transform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
