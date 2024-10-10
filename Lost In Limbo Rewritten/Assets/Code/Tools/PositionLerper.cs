using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PositionLerp
{
    public string m_Name;
    public float m_LerpSpeed;
    public Vector3 m_LerpPosition;
}
public class PositionLerper : MonoBehaviour
{
    [SerializeField] PositionLerp[] m_Positions = new PositionLerp[1];
    int m_LerpPosCurrent = 0;

    private void Start()
    {
        // Create the default lerper position. Reserved for system
        m_Positions[0].m_LerpSpeed = 1;
        m_Positions[0].m_Name = "Default";
        m_Positions[0].m_LerpPosition = transform.localPosition;
    }
    void FixedUpdate()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, m_Positions[m_LerpPosCurrent].m_LerpPosition, m_Positions[m_LerpPosCurrent].m_LerpSpeed * Time.deltaTime);
    }

    public void SetLerpPosition(int _index)
    {
        if (_index >= m_Positions.Length)
            return;

        m_LerpPosCurrent = _index;
    }
}
