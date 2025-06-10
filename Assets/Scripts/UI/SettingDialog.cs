using UnityEngine;
using UnityEngine.UI;

public class SettingDialog : Dialog
{
    public Slider musicSlider;
    public Slider soundSlider;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
    }

    public void Resume()
    {
        GameController.Ins.ResumeGame();
        Close();
    }

    private void Start()
    {
        // Gán giá trị ban đầu từ Prefs
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);

        // Đăng ký sự kiện kéo slider
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);

        ApplySettings();
    }

    private void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        ApplySettings();
    }

    private void OnSoundSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
        ApplySettings();
    }

    private void ApplySettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);

        AudioController.Ins.SetMusicVolume(musicVolume);
        AudioController.Ins.sfxAus.volume = soundVolume;
    }
}
