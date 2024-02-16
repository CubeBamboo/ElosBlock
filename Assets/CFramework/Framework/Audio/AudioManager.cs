using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Framework.MonoSingletons<AudioManager>
{
    private AudioSource bgmSource, sfxSource, voiceSource;
    //public enum ClipName { GameWin, GameFail, ClickButton, HighLight }
    //private Dictionary<ClipName, AudioClip> clipDict;


    //private AudioData audioData;

    protected override void Awake()
    {
        base.Awake();
        InitComponent();
        SetDontDestroyOnLoad();

        //clipDict = new Dictionary<ClipName, AudioClip>();
        //InitClipDict();
    }

    //TODO: ... you have to reconsruct it...
    //protected void InitClipDict()
    //{
    //    clipDict.Add(ClipName.GameWin, clipsData.GameWin);
    //    clipDict.Add(ClipName.GameFail, clipsData.GameFail);
    //    clipDict.Add(ClipName.ClickButton, clipsData.ClickButton);
    //    clipDict.Add(ClipName.HighLight, clipsData.HighLight);
    //}

    private void InitComponent()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();
    }

    #region PlayAudio


    public void PlayBgm(AudioClip clip)
    {
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayVoice(AudioClip clip)
    {
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    #endregion
}