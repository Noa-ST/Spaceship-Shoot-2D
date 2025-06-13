using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverDialog : Dialog
{
    public Text totalScoreTxt;
    public Text bestScoreTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);

        if (totalScoreTxt && GameController.Ins)
            totalScoreTxt.text = GameController.Ins.Score.ToString();

        if (bestScoreTxt)
            bestScoreTxt.text = Pref.BestScore.ToString();
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        GameController.Ins.ResetGame();
        SceneLoader.Ins.LoadCurrentScene();
    }


    private void OnSceneLoadEvent(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadEvent;
    }
}
