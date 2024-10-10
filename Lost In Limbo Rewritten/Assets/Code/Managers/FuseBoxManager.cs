using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuseBoxManager : MonoBehaviour
{
    [SerializeField] UnityEvent m_OnPuzzleComplete;

    [SerializeField] FuseSwitch[] m_FuseSwitched;
    [SerializeField] int[] m_PuzzleCode = new int[6];
    int m_SwitchUsage;
    bool m_PuzzleComplete = false;

    Light m_BoxLight;
    DoorModule m_Cover;
    private void Start()
    {
        m_FuseSwitched = GetComponentsInChildren<FuseSwitch>();
        m_BoxLight = GetComponentInChildren<Light>();
        m_Cover = GetComponentInChildren<DoorModule>();
    }

    void CheckSwitches()
    {
        if (m_FuseSwitched[m_PuzzleCode[0]].GetSwitchState() && m_FuseSwitched[m_PuzzleCode[1]].GetSwitchState() && m_FuseSwitched[m_PuzzleCode[2]].GetSwitchState() && m_FuseSwitched[m_PuzzleCode[3]].GetSwitchState()
            && m_FuseSwitched[m_PuzzleCode[4]].GetSwitchState() && m_FuseSwitched[m_PuzzleCode[5]].GetSwitchState())
        {
            m_OnPuzzleComplete.Invoke();
            m_BoxLight.color = Color.green;
            m_Cover.LockDoor();
            m_PuzzleComplete = true;
        }

    }

    void ResetAllSwitches()
    {
        for (int i = 0; i < m_FuseSwitched.Length; i ++)
        {
            m_FuseSwitched[i].ResetSwitch();
        }
    }

    public void AddSwitchUsage()
    {
        m_SwitchUsage++;
        m_SwitchUsage = Mathf.Clamp(m_SwitchUsage,0, m_PuzzleCode.Length);
        CheckSwitches();

        if (!m_PuzzleComplete && m_SwitchUsage >= m_PuzzleCode.Length)
        {
            m_SwitchUsage = 0;
            ResetAllSwitches();
        }
    }

    public void SubtractSwitch()
    {
        m_SwitchUsage--;

        m_SwitchUsage = Mathf.Clamp(m_SwitchUsage,0, m_PuzzleCode.Length);
    }
}
