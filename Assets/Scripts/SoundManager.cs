using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    [SerializeField] private List<Sound> sounds;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        LoadSounds();
    }
    private void LoadSounds()
    {
        foreach (Sound s in sounds)
        {   
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.audioClipVolume;
            s.source.loop = s.isLoop;       
        }
    }
    public void Play(SoundType type)
    {
        if (type == SoundType.BGM)
        {
            Sound s = sounds.Find(sound => sound.soundType == type);
            s.source.Play();
        }
        else
        {
            List<Sound> sfx = sounds.Where(sound => sound.soundType == type).ToList();
            Sound s = sfx[Random.Range(0, sfx.Count)];
            s.source.PlayOneShot(s.clip);
        }
        
        
    }

}