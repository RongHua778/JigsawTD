using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class UISetting : IUserInterface
{
    private Animator m_Anim;
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    List<Resolution> resolutions;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        InitializeResolution();
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    private void InitializeResolution()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToList();
        resolutionDropdown.ClearOptions();

        Resolution resolution = new Resolution();
        resolution.width = 2560;
        resolution.height = 1080;
        resolution.refreshRate = 60;
        resolutions.Add(resolution);

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("effectVolume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
    }

    public override void ClosePanel()
    {
        StaticData.SetTipsPos();
        m_Anim.SetBool("OpenLevel", false);
        MenuManager.Instance.ShowMenu();
    }
}
