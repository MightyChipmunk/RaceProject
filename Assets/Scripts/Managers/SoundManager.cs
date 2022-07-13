using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound
    {
        Bgm,
        Drift,
        Booster,
        Engine,
        Jump,
        Collide,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가.
    }
    public static SoundManager Instance { get; private set; }

    // 여러 개의 소리를 한번에 재생하기 위해 오디오 소스를 배열로 여러개를 저장
    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        GameObject root = GameObject.Find("SoundManager");

        string[] soundNames = System.Enum.GetNames(typeof(Sound));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _audioSources[(int)Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
        _audioSources[(int)Sound.Drift].loop = true; 
        _audioSources[(int)Sound.Booster].loop = true; 
        _audioSources[(int)Sound.Engine].loop = true; 
    }

    public void Play(AudioClip audioClip, Sound type, float pitch = 1.0f)
    {
        if (type == Sound.Bgm) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == Sound.Drift)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Drift];
            if (audioSource.isPlaying)
                return;

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == Sound.Booster)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Booster];
            if (audioSource.isPlaying)
                return;

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == Sound.Engine)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Engine];
            if (!audioSource.isPlaying)
                audioSource.Play();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
        }
        else if (type == Sound.Jump)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Jump];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        else if (type == Sound.Collide)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Collide];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Stop(Sound type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Stop();
    }
}
