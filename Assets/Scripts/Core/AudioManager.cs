using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    [System.Serializable]
    public class Sound {
        public SoundEffect type;
        public AudioMixerGroup outputMixer;
        public AudioClip clip;
        public bool loop = false;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds;
    public AudioMixer audioMixer;

    private Dictionary<SoundEffect, AudioSource> soundDict;

    #region Singleton
    public static AudioManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
        InitSounds();
    }
    #endregion
    private void InitSounds() {
        soundDict = new Dictionary<SoundEffect, AudioSource>();

        foreach (var sound in sounds) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.outputMixer;

            sound.source = source;
            soundDict[sound.type] = source;
        }
    }
    public void Play(SoundEffect type) {
        if (soundDict.TryGetValue(type, out AudioSource source)) {
            source.Play();
        } else {
            Debug.LogWarning($"Sound {type} not found!");
        }
    }
    public void Pause(SoundEffect type) {
        if (soundDict.TryGetValue(type, out AudioSource source)) {
            source.Pause();
        }
    }
    public void PauseAll() {
        foreach (var source in soundDict.Values) {
            if (source.isPlaying) {
                source.Pause();
            }
        }
    }
    public void StopAll() {
        foreach (var source in soundDict.Values) {
            source.Stop();
        }
    }
    public void ResumeAll() {
        foreach (var source in soundDict.Values) {
            if (!source.isPlaying) {
                source.UnPause();
            }
        }
    }

}
