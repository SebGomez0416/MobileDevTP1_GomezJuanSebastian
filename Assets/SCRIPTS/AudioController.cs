using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool isMute;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
    }

    private void Start()
    {
        isMute= DatosPartida.instance.Mute;
        _audioSource.mute = isMute;
    }

    private void OnEnable()
    {
        UI_Buttons.OnMute += Mute;
    }

    private void OnDisable()
    {
        UI_Buttons.OnMute -= Mute;
    }

    private void Mute()
    {
        isMute = !isMute;
        _audioSource.mute = isMute;
        DatosPartida.instance.Mute = isMute;
    }
    
    
}
