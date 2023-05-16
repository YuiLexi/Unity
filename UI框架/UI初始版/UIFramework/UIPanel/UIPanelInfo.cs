using System;

/// <summary>
/// 描述UIPanel的信息，包括类型和路径
/// </summary>
[Serializable]
public class UIPanelInfo
{
    private UIPanelType _type;
    private string _path;       //注意，脚本使用Rescources.load()动态加载，这里的文件路径不需要后缀

    public UIPanelInfo()
    { }

    public UIPanelInfo(UIPanelType type, string path)
    {
        _type = type;
        _path = path;
    }
    public UIPanelType Type
    {
        get { return _type; }
        set { _type = value; }
    }
    public string Path
    {
        get { return _path; }
        set { _path = value; }
    }
}
