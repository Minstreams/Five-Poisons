using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音系统
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(GameSystem))]
[RequireComponent(typeof(AudioSource))]
public class AudioSystem : MonoBehaviour
{
    //【Setting】
    [System.Serializable]
    public struct Setting
    {

    }
    [HideInInspector]
    public Setting setting;

    //【Interface】
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioClip">音效呀</param>
    public void Play(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }
        StartCoroutine(play(audioClip));
    }
    /// <summary>
    /// 播放bgm
    /// </summary>
    /// <param name="bgmClip">bgm</param>
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip == null)
        {
            return;
        }
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }
    /// <summary>
    /// 更改bgm音量
    /// </summary>
    /// <param name="volume">音量，应该是0到1</param>
    public void ChangeBgmVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    //【Core】
    /// <summary>
    /// bgm播放器
    /// </summary>
    private AudioSource bgmSource;

    private void Awake()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
    }

    private IEnumerator play(AudioClip audioClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioClip.length);
        Destroy(audioSource);
    }
}
