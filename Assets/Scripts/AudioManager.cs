using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip audioClip;
        public float volume;
    }

    private AudioSource audioSource;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private List<Sound> sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        InputAction upAction = InputSystem.actions.FindAction("Up");
        if (upAction.WasPerformedThisFrame())
        {
            PlaySound("Happy");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaySound("Sad");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaySound("Surprised");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaySound("Angry");
        }
        */
    }

    public void PlaySound(string sound, float volume = 1, float pitch = 1)
    {
        for (int i = 0; i < instance.sounds.Count; i++)
        {
            if (instance.sounds[i].name == sound)
            {
                instance.audioSource.pitch = pitch;
                instance.audioSource.PlayOneShot(instance.sounds[i].audioClip, instance.sounds[i].volume * volume);
            }
        }
    }

    public static void SetGlobalVolume(float volume)
    {
        instance.audioMixer.SetFloat("volume", Mathf.Log(volume + 0.001f, 2f) * 10);
    }
}
