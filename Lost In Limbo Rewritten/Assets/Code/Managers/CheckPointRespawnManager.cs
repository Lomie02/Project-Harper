using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointRespawnManager : MonoBehaviour
{
    [SerializeField] Transform[] m_RespawnPoints;
    [SerializeField] Transform m_PlayersPosition;

    DataManager m_DataManager;

    int m_CurrentRespawnPoint = 0;

    private void Start()
    {
        m_DataManager = FindAnyObjectByType<DataManager>();
        m_CurrentRespawnPoint = m_DataManager.GetCheckPoint();

        RespawnPlayerAtCheckpoint();
    }

    public void RespawnPlayerAtCheckpoint()
    {
        if (m_CurrentRespawnPoint != m_RespawnPoints.Length)
            m_PlayersPosition.position = m_RespawnPoints[m_CurrentRespawnPoint].position;
    }

    public void NextCheckpointReached()
    {
        m_CurrentRespawnPoint++;
        m_DataManager.SetPlayersCurrentCheckpoint(m_CurrentRespawnPoint);
    }

}
