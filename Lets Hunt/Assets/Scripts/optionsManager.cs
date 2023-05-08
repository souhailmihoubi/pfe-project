using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class optionsManager : MonoBehaviour
{
   public AudioMixer audioMixer ; 
   public void SetVolume (float volume)
   {
        audioMixer.SetFloat("volume", volume) ; 
   }
    
   public void SetVolumeSfx (float volume)
   {
        audioMixer.SetFloat("sfxVolume", volume) ; 
   }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
