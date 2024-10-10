using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct ObjectiveTask
{
    public string m_TaskName;
    public string m_TaskDescription;
}

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] ObjectiveTask[] m_Tasks;

    [Header("User Interface Main")]
    [SerializeField] GameObject m_ObjectiveTaskMain;
    [SerializeField] Text m_Description;

    [Header("User Interface Pause")]
    [SerializeField] Text m_PauseDescription;

    DataManager m_DataManager;

    int m_PlayersCurrentObjective = 0;

    void Start()
    {
        m_DataManager = FindAnyObjectByType<DataManager>();
        m_PlayersCurrentObjective = m_DataManager.GetPlayersCurrentObjective();

        UpdateDescriptions(m_Tasks[m_PlayersCurrentObjective].m_TaskDescription);
        m_ObjectiveTaskMain.SetActive(false);
    }

    public void ChangeTaskTo(int _taskIndex)
    {
        if (!m_ObjectiveTaskMain.activeSelf)
            m_ObjectiveTaskMain.SetActive(true);

        m_PlayersCurrentObjective = _taskIndex;

        m_ObjectiveTaskMain.GetComponent<Animator>().Play(0);
        UpdateDescriptions(m_Tasks[_taskIndex].m_TaskDescription);
    }

    public void SavePlayersObjective()
    {
        m_DataManager.SetPlayersCurrentObjective(m_PlayersCurrentObjective);
    }

    void UpdateDescriptions(string _Task)
    {
        m_PauseDescription.text = _Task;
        m_Description.text = _Task;
    }
}
