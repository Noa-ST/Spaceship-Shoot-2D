using UnityEngine;

public class MenuUIManager : Singleton<MenuUIManager>
{
    public GameObject menu;
    public BestScoreDialog bestScoreDialog;
    public SettingDialog settingDialog;


    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowMenu(bool isShow)
    {
        if (menu)
            menu.SetActive(isShow);
    }

    public void ShowBestScorePanel(bool isShow)
    {
        if (bestScoreDialog)
        {
            bestScoreDialog.Show(isShow);
            ShowMenu(!isShow);
        }
    }

    public void ShowSettingPanel(bool isShow)
    {
        if (settingDialog)
        {
            settingDialog.Show(isShow);
        }
    }
}
