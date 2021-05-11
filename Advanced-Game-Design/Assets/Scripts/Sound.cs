using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private List<AudioClip> _clips;
    public List<AudioClip> Clips => _clips;

    [Range(0f, 1f)]
    public float MinVolume;
    [Range(0f, 1f)]
    public float MaxVolume;

    [Range(0.1f, 3f)]
    public float MinPitch;
    [Range(0.1f, 3f)]
    public float MaxPitch;

    public bool Loop;

    public AudioSource Source { get; private set; }

    private int _lastClipIndex;

    public void LinkAudioSource(AudioSource audioSource)
    {
        Source = audioSource;
    }

    private AudioClip RandomClip()
    {
        if (Clips.Count < 2)
        {
            return Clips[0];
        }

        int clipIndex;

        do
        {
            clipIndex = Random.Range(0, Clips.Count - 1);
        } while (clipIndex == _lastClipIndex);

        _lastClipIndex = clipIndex;
        return Clips[clipIndex];
    }

    public void Play()
    {
        Source.volume = Random.Range(MinVolume, MaxVolume);
        Source.pitch = Random.Range(MinPitch, MaxPitch);

        var clip = RandomClip();

        if (!Loop)
        {
            Source.PlayOneShot(clip);
        }
        else
        {
            Source.loop = true;
            Source.clip = clip;
            Source.Play();
        }
    }
}
