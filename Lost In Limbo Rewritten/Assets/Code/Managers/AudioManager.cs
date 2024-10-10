using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

enum ClipWrapMode
{
    Default = 0,
    Cycle,
    PlayAll,
    Random,
    Loop,
}

[System.Serializable]
struct AudioTrack
{
    public string m_Name;
    public AudioSource m_Source;
    public ClipWrapMode m_WrapMode;
    public AudioClip[] m_Clip;
    public int m_CurrentClip;

    [Space]
    public SubtitleText[] m_Subtitles;
}

[System.Serializable]
struct SubtitleText
{
    public string m_SubtitleText;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioTrack[] m_AudioTrack;

    [Header("Subtitles")]
    [SerializeField] Text m_SubtitleTextObject;
    [SerializeField] bool m_UseSubtitles;

    int m_CurrentTrack = 0;

    public void PlayTrack(int _track)
    {
        m_CurrentTrack = _track;

        switch (m_AudioTrack[m_CurrentTrack].m_WrapMode)
        {
            case ClipWrapMode.Default:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_CurrentTrack].m_SubtitleText;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                break;

            case ClipWrapMode.Cycle:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_CurrentTrack].m_SubtitleText;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                m_AudioTrack[m_CurrentTrack].m_CurrentClip++;
                break;

            case ClipWrapMode.PlayAll:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                break;


            case ClipWrapMode.Random:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[Random.Range(0, m_AudioTrack[m_CurrentTrack].m_Clip.Length)];
                m_SubtitleTextObject.text = m_AudioTrack[m_CurrentTrack].m_Subtitles[m_CurrentTrack].m_SubtitleText;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                break;

            case ClipWrapMode.Loop:
                m_AudioTrack[m_CurrentTrack].m_Source.clip = m_AudioTrack[m_CurrentTrack].m_Clip[m_AudioTrack[m_CurrentTrack].m_CurrentClip];
                m_AudioTrack[m_CurrentTrack].m_Source.loop = true;
                m_AudioTrack[m_CurrentTrack].m_Source.Play();
                break;
        }
    }
}
