using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboMaterialManager : MonoBehaviour
{
    [SerializeField] Material[] m_Mat;
    [SerializeField] MeshRenderer[] m_Renderers;
    AbilityTransition m_PlayersAbilitys;
    PlayerController m_PlayersController;

    float m_LerpSpeed = 3;
    float m_LerpAmount;

    void Start()
    {
        m_PlayersAbilitys = FindAnyObjectByType<AbilityTransition>();
        m_PlayersController = FindAnyObjectByType<PlayerController>();
        GameObject[] Objects = GameObject.FindGameObjectsWithTag("LimboView");

        m_Mat = new Material[Objects.Length];
        m_Renderers = new MeshRenderer[Objects.Length];

        for (int i = 0; i < Objects.Length; i++)
        {
            m_Mat[i] = Objects[i].GetComponent<MeshRenderer>().material;
            m_Renderers[i] = Objects[i].GetComponent<MeshRenderer>();
        }
    }

    void FixedUpdate()
    {
        if (!m_PlayersController.GetMovementState())
            return;

        if (m_PlayersAbilitys.IsInLimbo())
        {
            m_LerpAmount = Mathf.Lerp(m_LerpAmount, 2, m_LerpSpeed * Time.deltaTime);
        }
        else
        {
            m_LerpAmount = Mathf.Lerp(m_LerpAmount, -1, m_LerpSpeed * Time.deltaTime);
        }

        for (int i = 0; i < m_Mat.Length; i++)
        {
            if (m_PlayersAbilitys.IsInLimbo())
                m_Renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            else
                m_Renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            m_Mat[i].SetFloat("_Fade", m_LerpAmount);
        }
    }
}
