using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public float MasterVolume
    {
        get => AudioListener.volume;
        set => AudioListener.volume = value;
    }

    public float MusicVolume
    {
        get => musicSource.volume;
        set => musicSource.volume = value;
    }

    
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 2f)]
        public float volume = 1;
        [Range(0.1f, 2f)]
        public float pitch = 1;

    }

    public Sound[] sounds;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    new void Awake()
    {
        base.Awake();
        InitializeSoundDictionary();
    }
    private void InitializeSoundDictionary()
    {
        foreach (Sound sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            AudioSource availableSource = sfxSource;
            if (sfxSource != null)
            {
                sfxSource.clip = sound.clip;
                sfxSource.volume = sound.volume;
                sfxSource.pitch = sound.pitch;
                sfxSource.Play();
            }
            else
            {
                Debug.LogWarning("No available audio source to play sound: " + soundName);
            }
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void PlayMusic(string musicName)
    {
        if (soundDictionary.TryGetValue(musicName, out Sound sound))
        {
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume;
            musicSource.pitch = sound.pitch;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music not found: " + musicName);
        }
    }

    public void PlayBigCreatureSound(float volume){
        sfxSource.volume = volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void TurnOnMusic()
    {
        musicSource.mute = false;
    }

    public void TurnOffMusic()
    {
        musicSource.mute = true;
    }

    public void TurnOnSound()
    {
        sfxSource.mute = false;
    }

    public void TurnOffSound()
    {
        sfxSource.mute = true;
    }


}
