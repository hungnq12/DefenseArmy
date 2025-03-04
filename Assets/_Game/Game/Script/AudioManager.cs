
using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioData audioData;
    public static AudioManager Instance { get; private set; }

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        }

        RandomBGMusic();
    }

    private void RandomBGMusic()
    {
        StartCoroutine(BGCoroutine());
        IEnumerator BGCoroutine()
        {
            while (true)
            {
                var music = audioData.BGMusics[Random.Range(0, audioData.BGMusics.Length)];
                EazySoundManager.PlayMusic(music);
                yield return new WaitForSeconds(music.length);
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }
    }
    
    public void PlaySound(AudioClip clip, float volume)
    {
        EazySoundManager.PlaySound(clip, volume);
    }

}

