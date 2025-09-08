using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

// 拡張メソッド
static class MyAudioMethod
{
    // 再生
    public static void Play(this AudioSource source, AudioResource resource, float volume = 1.0f)
    {
        source.resource = resource;
        source.volume = volume;
        source.Play();
    }

    // 再生（フェードイン）
    public static void PlayWithFadeIn(this AudioSource source, AudioResource resource, float volume = 1.0f, float fadeTime = 0.1f)
    {
        // 無音で再生
        source.Play(resource, 0.0f);
        // ボリュームを徐々に上げる
        source.DOFade(volume, fadeTime);
    }

    // 停止（フェードアウト）
    public static void StopWithFadeOut(this AudioSource source, float fadeTime = 0.1f)
    {
        source.DOFade(0.0f, fadeTime);
        source.Stop();
    }
}

public partial class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource sourcePlayerSE;

    private void Start()
    {
        PlayerAudioInitialize();
    }

    private void Update()
    {
        PlayerAudioUpdate();
    }

}
