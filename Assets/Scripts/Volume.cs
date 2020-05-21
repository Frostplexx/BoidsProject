using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class Volume : MonoBehaviour
{
    public AudioMixer mixer; 

    public void SetLevel(float sliderVal)
    {
        mixer.SetFloat("volume", Mathf.Log10(sliderVal) * 20); 
    }
}
