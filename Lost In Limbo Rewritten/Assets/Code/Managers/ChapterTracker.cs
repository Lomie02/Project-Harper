using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
struct ChapterFloor
{
    public string m_Name;
    public GameObject m_FloorObject;
    public UnityEvent m_OnStartUp;
}
public class ChapterTracker : MonoBehaviour
{
    [SerializeField] ChapterFloor[] m_Floors;
    int m_CurrentFloor = 0;

    DataManager m_DataManager;

    private void Start()
    {
        m_DataManager = FindAnyObjectByType<DataManager>();
        m_CurrentFloor = m_DataManager.GetCurrentFloor();

        SetCurrentFloor();
    }

    void SetCurrentFloor()
    {
        for (int i = 0; i < m_Floors.Length; i++)
        {
            if (i == m_CurrentFloor)
            {
                m_Floors[i].m_FloorObject.SetActive(true);
                m_Floors[i].m_OnStartUp.Invoke();
            }
            else
            {
                m_Floors[i].m_FloorObject.SetActive(false);

            }
        }
    }


    public void SaveCurrentFloor(int _index)
    {
        m_DataManager.SaveCurrentFloor(_index);
    }
}
