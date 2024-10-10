using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] AudioTrack[] m_AudioTrack;
    [SerializeField] Text m_SubtitleTextObject;
    [SerializeField] bool m_UseSubtitles;

    int m_CurrentTrack = 0;
    bool m_AudioClipIsPlaying = false;
    DataManager m_DataManager;

    void Start()
    {
        if (m_SubtitleTextObject)
            m_SubtitleTextObject.gameObject.SetActive(false);

        m_DataManager = FindAnyObjectByType<DataManager>();

        if (m_DataManager)
            m_UseSubtitles = m_DataManager.GetSubtitlesState();
    }

    private void Update() // Play through all dialogue in given track.
    {
        if (!m_AudioClipIsPlaying)
            return;

        if (m_AudioTrack[m_CurrentTrack].m_WrapMode == ClipWrapMode.PlayAll)
        {
            if (m_AudioTrack[m_CurrentTrack].m_CurrentClip < m_AudioTrack[m_CurrentTrack].m_Clip.Length - 1)
            {
                if (!m_AudioTrack[m_CurrentTrack].m_Source.isPlaying)
                {
                    m_AudioTrack[m_CurrentTrack].m_CurrentClip++;
                    m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                    m_AudioTrack[m_CurrentTrack].m_Source.Play();

                    if (m_UseSubtitles && m_AudioTrack[m_CurrentTrack].m_Subtitles.Length > 0)
                    {
                        m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_AudioTrack[m_CurrentTrack].m_CurrentClip].m_SubtitleText;
                    }
                }
            }
            else
            {
                m_AudioClipIsPlaying = false;
                if (m_UseSubtitles && m_AudioTrack[m_CurrentTrack].m_Subtitles.Length > 0)
                    m_SubtitleTextObject.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!m_AudioTrack[m_CurrentTrack].m_Source.isPlaying)
            {
                if (m_UseSubtitles && m_AudioTrack[m_CurrentTrack].m_Subtitles.Length > 0)
                {
                    m_AudioClipIsPlaying = false;
                    m_SubtitleTextObject.gameObject.SetActive(false);
                }

            }
        }
    }

    public void PlayTrack(int _index) // Play Audio tracks given.
    {
        m_AudioClipIsPlaying = true;
        m_CurrentTrack = _index;

        m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
        m_AudioTrack[m_CurrentTrack].m_Source.Play();
    }
}
