
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Voice : MonoBehaviour
{
    public int m_Frequency = 44100;
    public int m_Lenght = 1;
    public bool m_IsLoop = true;

    protected AudioSource m_AudioSource;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        StartCaptureVoice();
    }

    public void StartCaptureVoice()
    {
        if (Microphone.devices == null || Microphone.devices.Length == 0)
            return;

        if (m_AudioSource.clip != null)
            m_AudioSource.clip.UnloadAudioData();

        m_AudioSource.clip = Microphone.Start(null, m_IsLoop, m_Lenght, m_Frequency);
        m_AudioSource.loop = m_IsLoop;
        while (Microphone.GetPosition(null) < 0)
        {

        }
        m_AudioSource.Play();
    }

    public void StopCaptureVoice()
    {
        if (Microphone.IsRecording(null) == false)
            return;

        Microphone.End(null);
        m_AudioSource.Stop();
    }
}
