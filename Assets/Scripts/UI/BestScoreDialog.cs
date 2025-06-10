using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreDialog : Dialog
{
    public Text bestScoreTxt;
    public override void Show(bool isShow)
    {
        base.Show(isShow);

        if (isShow)
        {
            int bestScore = PlayerPrefs.GetInt("BestScore", 0); 
            bestScoreTxt.text = bestScore.ToString();
        }
    }
}
