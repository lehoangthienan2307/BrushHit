using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType {
    BGM,
    SFX
}

[System.Serializable]
public class Sound
{
    public SoundType soundType;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float audioClipVolume=1f;

    public bool isLoop;

    [HideInInspector]
    public AudioSource source;
}