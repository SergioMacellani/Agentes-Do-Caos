using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    public List<AudioClip> music;
    void Start()
    {
        TryGetComponent(out _audioSource);
        _audioSource.clip = music[Random.Range(0,music.Count)];
        _audioSource.Play();
    }
}
