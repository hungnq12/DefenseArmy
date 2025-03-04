using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioClip[] bgMusics;
    public AudioClip[] BGMusics => bgMusics;
}
