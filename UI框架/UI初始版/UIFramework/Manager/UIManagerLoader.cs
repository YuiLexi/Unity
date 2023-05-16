using UnityEngine;

public class UIManagerLoader : MonoBehaviour
{
    //先把当前场景中的Canvas对象的Transform属性赋值给UIManager的CanvasTransform属性
    //游戏一开始就加载MainMenuUIPanel
    private void Awake()
    {
        UIManager.Instance.CanvasTransform = this.transform;
        UIManager.Instance.PushUIPanel(UIPanelType.MainMenuUIPanel);
    }
}