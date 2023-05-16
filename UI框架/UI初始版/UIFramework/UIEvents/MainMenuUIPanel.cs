using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIPanel : BaseUIPanel
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("MainMenuUIPanel OnEnter");
    }
    public override void OnPause()
    {
        base.OnPause();
        Debug.Log("MainMenuUIPanel OnPause");
    }
    public override void OnResume()
    {
        base.OnResume();
        Debug.Log("MainMenuUIPanel OnResume");
    }
    public override void OnEixt()
    {
        base.OnEixt();
        Debug.Log("MainMenuUIPanel OnEixt");
    }

    public void OnSettingClick()
    {
        UIManager.Instance.PushUIPanel(UIPanelType.SettingUIPanel);
    }
}
