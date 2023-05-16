using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIPanel : BaseUIPanel
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("SettingUIPanel OnEnter");
    }
    public override void OnPause()
    {
        base.OnPause();
        Debug.Log("SettingUIPanel OnPause");
    }
    public override void OnResume()
    {
        base.OnResume();
        Debug.Log("SettingUIPanel OnResume");
    }
    public override void OnEixt()
    {
        base.OnEixt();
        Debug.Log("SettingUIPanel OnEixt");
    }
    public void OnCloseClick()
    {
        UIManager.Instance.PopUIPanel();
    }
}
