using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformParent : MonoBehaviour
{
    [SerializeField] Transform m_Parent;

    bool m_LookOverShoulder = false;

    [SerializeField] Animator m_Body;
    private void Start()
    {
    }
    void LateUpdate()
    {
        transform.position = m_Parent.position;

        if (m_Body)
            m_Body.SetBool("OverShoulder", m_LookOverShoulder);
    }

    public void SetOverShoulderState(bool _State)
    {
        m_LookOverShoulder = _State;
    }
}
