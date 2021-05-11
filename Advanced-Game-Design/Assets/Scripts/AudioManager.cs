using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> Sounds;

    public static AudioManager Instance { get; private set; }

    private Dictionary<string, Sound> _soundMap;

    void Awake()
    {
        _soundMap = new Dictionary<string, Sound>();

        foreach (var sound in Sounds)
        {
            _soundMap[sound.Name] = sound;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            sound.LinkAudioSource(source);
        }

        if (Instance)
        {
            throw new InvalidOperationException($"Don't create multiple AudioManagers in a scene use {gameObject} instead!");
        }
        Instance = this;
    }

    public void Play(string soundName)
    {
        if (!_soundMap.ContainsKey(soundName))
        {
            throw new ArgumentException($"No audio file with name {soundName} registered with {gameObject}!");
        }

        _soundMap[soundName].Play();
    }
}
