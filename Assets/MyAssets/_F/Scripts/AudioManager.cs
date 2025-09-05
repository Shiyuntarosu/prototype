using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> audioSourceList_BGM = new List<AudioSource>();

    public static void PlayWithFadeIn(AudioSource source, AudioClip clip, float fadeTime)
    {
    }

    public static void StopWithFadeOut()
    {
    }
}
