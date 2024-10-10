using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] Transform m_TargetParent;

    Vector3 m_TrackPosition;
    [SerializeField] float m_LerpDuration = 4;
    void FixedUpdate()
    {
        if (!m_TargetParent.gameObject.activeSelf)
            return;

        m_TrackPosition = Vector3.Slerp(m_TrackPosition, m_TargetParent.position, m_LerpDuration * Time.deltaTime);

        transform.LookAt(m_TrackPosition, Vector3.up);    
    }
}
