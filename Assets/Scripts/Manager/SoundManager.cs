using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    ButtonClick,
    HexagonClick,
    Win,
    Lose,


}
[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private SoundList[] soundLists;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        if(!_musicSource.isPlaying) _musicSource.Play();
    }

    public static void PlaySound(SoundType soundName, float volume = 1)
    {
        AudioClip[] clips = Instance.soundLists[(int)soundName].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance._soundSource.PlayOneShot(randomClip, volume);
        // Instance._soundSource.PlayOneShot(Instance);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundLists, names.Length);
        for(int i = 0; i < names.Length; i++)
        {
            soundLists[i].name = names[i];
        }
    }
#endif
}
[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds{ get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
