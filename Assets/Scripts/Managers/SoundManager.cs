using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	public static SoundManager Instance
	{
		get
		{
			var instance = Singleton<SoundManager>.Instance;
			instance.Init();
			
			return instance;
		}
	}
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    public float _soundVolumn = 1f;

    public Define.BGMs _BGM;
    // MP3 Player   -> AudioSource
    // MP3 음원     -> AudioClip
    // 관객(귀)     -> AudioListener

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string name, Define.Sound type = Define.Sound.effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(name, type);
        Play(audioClip, type, pitch);
    }

	public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.effect, float pitch = 1.0f)
	{
        if (audioClip == null)
            return;

		if (type == Define.Sound.bgm)
		{
			AudioSource audioSource = _audioSources[(int)Define.Sound.bgm];
			if (audioSource.isPlaying)
				audioSource.Stop();

            _BGM = (Define.BGMs)System.Enum.Parse(typeof(Define.BGMs), audioClip.name);

            audioSource.pitch = pitch;
			audioSource.clip = audioClip;
			audioSource.Play();
		}
		else
		{
			AudioSource audioSource = _audioSources[(int)Define.Sound.effect];
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(audioClip);
		}
	}

	AudioClip GetOrAddAudioClip(string name, Define.Sound type = Define.Sound.effect)
    {
		AudioClip audioClip = null;

		if (type == Define.Sound.bgm)
		{
            string path = $"Audio/BGM/{name}";
			audioClip = ResourceManager.Instance.Load<AudioClip>(path);
		}
		else
		{
			if (!_audioClips.TryGetValue(name, out audioClip))
			{
                string path = $"Audio/Effect/{name}";
                audioClip = ResourceManager.Instance.Load<AudioClip>(path);
				_audioClips.Add(name, audioClip);
			}
		}

		if (audioClip == null)
			Debug.Log($"AudioClip Missing ! {name}");

		return audioClip;
    }
    public void SetAudioVolumn(Define.Sound type = Define.Sound.bgm, float volumn = 1)
    {
        _audioSources[(int)type].volume = volumn;
    }
}
