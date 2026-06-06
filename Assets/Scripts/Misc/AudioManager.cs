using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MyBox;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Foldout("Play audio", true)]
    public static AudioManager instance;
    AudioSource audioPlayer;
    public AudioMixer mixer;
    [Foldout("Sound effects", true)]
    [SerializeField] AudioClip menuSound; public void Menu(float volume = 0.3f) => PlaySound(menuSound, volume);
    [SerializeField] AudioClip damageSound; public void Damage(float volume = 0.3f) => PlaySound(damageSound, volume);
    [SerializeField] AudioClip healSound; public void Heal(float volume = 0.3f) => PlaySound(healSound, volume);
    [SerializeField] AudioClip missSound; public void Miss(float volume = 0.3f) => PlaySound(missSound, volume);
    [SerializeField] AudioClip shootSound; public void Shoot(float volume = 0.3f) => PlaySound(shootSound, volume);

    private void Awake()
    {
        if (instance == null)
        {
            audioPlayer = GetComponent<AudioSource>();
            instance = this;
            mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume")) * 20);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void PlaySound(AudioClip audio, float volume)
    {
        audioPlayer.PlayOneShot(audio, volume);
    }
}
