using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] GameObject gameoverPanel;

    public void SetScoreText(string txt)
    {
        scoreText.text = txt;
    }

    public void ShowGameOverPanel(bool isShow)
    {
        if (gameoverPanel)
            gameoverPanel.SetActive(isShow);
    }
}
