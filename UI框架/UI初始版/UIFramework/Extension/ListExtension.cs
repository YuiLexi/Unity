using System.Collections;
using System.Collections.Generic;

public static class ListExtesion
{
    /// <summary>
    /// 尝试在列表中查找指定的元素
    /// </summary>
    /// <param name="uIPanelInfos">当前操作的列表</param>
    /// <param name="uIPanelType">定的元素</param>
    /// <returns>如果列表中由指定的元素，就返回该元素；如果没有，就返回null</returns>
    public static UIPanelInfo TrySearchUIPanel(this List<UIPanelInfo> uIPanelInfos, UIPanelType uIPanelType)
    {
        foreach (UIPanelInfo uIPanelInfo in uIPanelInfos)
        {
            if (uIPanelInfo.Type == uIPanelType)
                return uIPanelInfo;
        }
        return null;
    }
}
