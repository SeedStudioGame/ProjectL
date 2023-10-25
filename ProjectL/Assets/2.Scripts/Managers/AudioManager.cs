using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBase
{
    private struct AudioObject
    {
        public Transform _object;
        public AudioSource _audio;
    }


    private AudioSource _bgmSource;
    private AudioObject _sfxSource;
    private List<AudioObject> _sfxSources = new List<AudioObject>();
    private int _sfxIndex;
    private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
    private bool _isSilence;

    public override void Init()
    {
        _isSilence = false;

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _sfxSources.Clear();
        _clips.Clear();

        ClipLoad<Define.BGM>("BGM");
        ClipLoad<Define.SFX>("SFX");
    }

    private void ClipLoad<T>(string parentFolder) where T : System.Enum
    {
        string[] clips = System.Enum.GetNames(typeof(T));
        foreach (string clip in clips)
        {
            _clips.Add(clip, Util.Load<AudioClip>($"/Audio/{parentFolder}/{clip}"));
        }
    }

    public void PlayBGM(Define.BGM tag)
    {
        _bgmSource.clip = GetClip(tag);
        PlayBGM();
    }

    public void PlaySFX(Define.SFX tag, Transform target)
    {
        _sfxSource = GetPlayableSFX();
        _sfxSource._object.position = target.position;
        _sfxSource._audio.clip = GetClip(tag);
        PlaySFX();
    }

    private AudioObject GetPlayableSFX()
    {
        for (int i = 0; i < _sfxSources.Count; i++)
        {
            _sfxIndex = (_sfxIndex + 1) % _sfxSources.Count;
            if (!_sfxSources[_sfxIndex]._audio.isPlaying)
            {
                return _sfxSources[_sfxIndex];
            }
        }

        AudioObject go = new AudioObject();
        go._object = new GameObject().transform;
        go._object.name = "sfx" + _sfxSources.Count;
        go._audio = Util.GetOrAddComponent<AudioSource>(go._object.gameObject);

        return go;
    }

    private AudioClip GetClip<T>(T tag) where T : System.Enum
    {
        AudioClip clip = FindClip(System.Enum.GetName(typeof(T), tag));
        return clip;
    }

    private AudioClip FindClip(string tag)
    {
        if (!_clips.ContainsKey(tag))
        {
            Debug.LogError("No tag");
            return null;
        }

        AudioClip clip = _clips[tag];

        if (clip == null)
        {
            Debug.LogError("No clip");
            return null;
        }

        return clip;
    }

    public void PlayBGM()
    {
        if (_isSilence)
            return;

        _bgmSource.Play();
    }

    public void PlaySFX()
    {
        if (_isSilence)
            return;

        _sfxSource._audio.Play();
    }

    public void PauseBGM() { _bgmSource.Pause(); }

    public void PauseSFX()
    {
        for (int i = 0; i < _sfxSources.Count; i++)
        {
            _sfxSources[i]._audio.Pause();
        }
    }

    public void StopBGM() { _bgmSource.Stop(); }

    public void StopSFX()
    {
        for (int i = 0; i < _sfxSources.Count; i++)
        {
            _sfxSources[i]._audio.Stop();
        }
    }

    public void UnSlience() { _isSilence = false; }

    public void Slience() { _isSilence = true; }

    public void Pause()
    {
        PauseBGM();
        PauseSFX();
    }

    public void Stop()
    {
        StopBGM();
        StopSFX();
    }
}
