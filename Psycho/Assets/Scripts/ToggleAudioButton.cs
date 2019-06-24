using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudioButton : MonoBehaviour
{
    AudioSource audioSource;
    Image image;

    bool isOn = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        image = GetComponent<Image>();

        TurnOff();
    }

    public void TurnOn()
    {
        image.color = new Color32(255, 255, 255, 255);
        isOn = true;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            return;
        }

        audioSource.UnPause();
    }

    public void TurnOff()
    {
        image.color = new Color32(125, 125, 125, 255);
        isOn = false;

        if (!audioSource.isPlaying)
        {
            return;
        }

        audioSource.Pause();
    }


    // Когда нажимаем на одну и ту же кнопку
    void ToggleAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    public void Toggle()
    {
        if (isOn)
        {
            ToggleAudio();
            return;
        }

        foreach (var toggleButton in gameObject.transform.parent.GetComponentsInChildren<ToggleAudioButton>())
        {
            toggleButton.TurnOff();
        }

        TurnOn();
    }
}
