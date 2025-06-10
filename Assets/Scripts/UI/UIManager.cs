using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject gamePlay;
    public Text scoreTxt;
    public PauseDialog pauseDialog;
    public GameoverDialog gameoverDialog;
    public SettingDialog settingDialog;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGamePlay(bool isShow)
    {
        if (gamePlay)
            gamePlay.SetActive(isShow);
    }

    public void UpdateScore(int score)
    {
        if (scoreTxt)
        {
            scoreTxt.text = "Score: " + score.ToString();
        }
    }

    public void ShowPausePanel(bool isShow)
    {
        if (pauseDialog)
        {
            pauseDialog.Show(isShow);
        }
    }

    public void ShowSettingPanel(bool isShow)
    {
        if (settingDialog)
        {
            settingDialog.Show(isShow);
        }
    }

    public void PauseButtonPressed()
    {
        GameController.Ins.PauseGame();
        ShowPausePanel(true);
    }

    public void SettingButtonPressed()
    {
        GameController.Ins.PauseGame();
        ShowSettingPanel(true);
    }
}
