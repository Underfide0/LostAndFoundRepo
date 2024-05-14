using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider, sfxSlider, brightnessSlider;
   
    private void Awake()
    {
      
        DontDestroyOnLoad(gameObject);
       
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }
    public void brightnessAmount()
    {
        Screen.brightness = brightnessSlider.value;
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex:2);
    }

    public void resolutionDropdown(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1280, 800, true);
                break;
            case 2:
                Screen.SetResolution(640, 480, true);
                break;
        }
    }
}
