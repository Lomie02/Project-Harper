using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseSwitch : MonoBehaviour
{
    [SerializeField] Transform m_SwitchMesh;
    [SerializeField] int m_SwitchId = 0;

    [SerializeField] Vector3 m_OnRotation;
    [SerializeField] Vector3 m_OffRotation;

    [SerializeField] float m_LerpSpeed;

    bool m_IsSwitchedOn = false;
    [SerializeField] Light m_LightSwitch;
    Animator m_SwitchAnimator;

    FuseBoxManager m_Manager;
    void Start()
    {
        m_SwitchAnimator = m_SwitchMesh.gameObject.GetComponent<Animator>();
        m_LightSwitch.color = Color.green;
        m_LightSwitch.gameObject.SetActive(false);
        m_Manager = GetComponentInParent<FuseBoxManager>();
    }

    public void FlickSwitch()
    {
        if (m_IsSwitchedOn)
        {
            m_IsSwitchedOn = false;
            m_LightSwitch.gameObject.SetActive(false);
            m_SwitchAnimator.SetBool("SwitchState", m_IsSwitchedOn);
            m_Manager.SubtractSwitch();
        }
        else
        {
            m_IsSwitchedOn = true;
            m_LightSwitch.gameObject.SetActive(true);
            m_SwitchAnimator.SetBool("SwitchState", m_IsSwitchedOn);
            m_Manager.AddSwitchUsage();
        }
    }

    public void ResetSwitch()
    {
        m_IsSwitchedOn = false;
        m_LightSwitch.gameObject.SetActive(false);
        m_SwitchAnimator.SetBool("SwitchState", m_IsSwitchedOn);
    }
    public bool GetSwitchState()
    {
        return m_IsSwitchedOn;
    }

    public int GetSwitchID()
    {
        return m_SwitchId;
    }
}
