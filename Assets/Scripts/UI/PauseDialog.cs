using UnityEngine;

public class PauseDialog : Dialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        GameController.Ins.ResumeGame();
        Close();
    }

    public void BackToMenu()
    {
        GameController.Ins.ResumeGame(); 
        SceneLoader.Ins.LoadCurrentScene();
    }
}