using UnityEngine;

public static class Pref
{
    public static int BestScore
    {
        set
        {
            int oldScore = PlayerPrefs.GetInt(PrefKey.BestScore.ToString(), 0);

            if (value > oldScore || oldScore == 0)
            {
                PlayerPrefs.SetInt(PrefKey.BestScore.ToString(), value);
            }
        }
        get => PlayerPrefs.GetInt(PrefKey.BestScore.ToString(), 0);
    }
    public static bool MusicEnabled
    {
        get => PlayerPrefs.GetInt(PrefKey.MusicEnabled.ToString(), 1) == 1;
        set => PlayerPrefs.SetInt(PrefKey.MusicEnabled.ToString(), value ? 1 : 0);
    }

    public static bool SoundEnabled
    {
        get => PlayerPrefs.GetInt(PrefKey.SoundEnabled.ToString(), 1) == 1;
        set => PlayerPrefs.SetInt(PrefKey.SoundEnabled.ToString(), value ? 1 : 0);
    }
}