using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager
{
    #region 单例模式
    //单例模式
    private static UIManager s_instance;
    public static UIManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new UIManager();
            return s_instance;
        }
    }
    #endregion 单例模式

    #region 文件路径
    //存放 UIPanel 预制件的文件夹路径
    private readonly string _basePahtUIPanelPrefabFolder = Application.dataPath + @"/Resources/UIPanelPrefab/";
    //存放UIPanel信息的Json文件的路径
    private readonly string _jsonFolderPath = Application.dataPath + @"/Json/UIJson/";
    private readonly string _jsonFileName = "UIPanelInfo.json";
    #endregion 文件路径

    private List<UIPanelInfo> _uIPanelInfos;   //该列表是存放所有的UIPanelInfo信息，包括未加载到游戏场景中的

    private Transform _canvasTransform; //存放当前场景中的Canvas对象的Transform属性

    private Stack<BaseUIPanel> _currentUIPanels; //使用栈，存放当前场景中的所有UIPanel


    //获取Transform
    public Transform CanvasTransform
    {
        get
        {
            if (_canvasTransform == null)
                _canvasTransform = GameObject.Find("Canvas").transform;
            return _canvasTransform;
        }
        set
        {
            if (value.name == "Canvas")
                _canvasTransform = value;
        }
    }

    private UIManager()
    {
        InitUIPanelInfo(); //初始化UIPanelInfo
        CheckUIPanelWhenGameBegin();
    }


    /// <summary>
    /// 传入路径和对应的数据，把该数据写入到对应路径的Json文件里
    /// </summary>
    /// <param name="folderPath">文件夹路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="t">写入的数据</param>
    /// <typeparam name="T">数据类型</typeparam>
    private void WriteToJsonFile<T>(string folderPath, string fileName, T t)
    {
        string json = JsonMapper.ToJson(t);
        //如果文件夹不存在，就创建文件夹
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        //如果文件不存在，就创建文件
        if (!File.Exists(folderPath + fileName))
            File.Create(folderPath + fileName).Dispose();
        //将数据写入到文件中
        File.WriteAllText(folderPath + fileName, json);
    }

    /// <summary>
    /// 传入文件的路径，就能读取到该文件的数据
    /// </summary>
    /// <param name="folderPath">文件夹路径</param>
    /// <param name="fileName">文件名</param>
    /// <typeparam name="T"></typeparam>
    private List<T> ReadFromJsonFIle<T>(string folderPath, string fileName) where T : class
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        if (!File.Exists(folderPath + fileName))
            File.WriteAllText(folderPath + fileName, "[]");
        List<T> t = JsonMapper.ToObject<List<T>>(File.ReadAllText(folderPath + fileName));
        return t;
    }

    /// <summary>
    /// 自动更新Json文件，并获取到所有的UIPanelInfo数据
    /// </summary>
    private void InitUIPanelInfo()
    {
        //先获取当前Json文件中的UIPanelInfo数据
        _uIPanelInfos = ReadFromJsonFIle<UIPanelInfo>(_jsonFolderPath, _jsonFileName);

        //创建UIPanel的文件夹对象
        if (!Directory.Exists(_basePahtUIPanelPrefabFolder))
            Directory.CreateDirectory(_basePahtUIPanelPrefabFolder);
        DirectoryInfo directoryInfo = new DirectoryInfo(_basePahtUIPanelPrefabFolder);

        //遍历文件夹下的每一个prefab文件。
        //如果当前UIPanelInfo列表中有对应类型的模板信息，就更新路径；没有，就添加对应信息到UIPanelInfo列表
        foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.prefab"))
        {
            //将对应UIPanel模板的名字转换为UIPanel类型
            UIPanelType type = (UIPanelType)Enum.Parse(typeof(UIPanelType), fileInfo.Name.Replace(".prefab", ""));
            string path = @"UIPanelPrefab/" + Convert.ToString(type); //基址+对应模板文件名，组成完整地址（不要后缀）

            //尝试在列表中寻找UIpanelInfo对象，如果有，则返回对应的对象；如果没有，则返回null
            UIPanelInfo uIPanelInfo = _uIPanelInfos.TrySearchUIPanel(type);

            if (uIPanelInfo == null)    //UIPanel不在该List中
            {
                uIPanelInfo = new UIPanelInfo(type, path);
                _uIPanelInfos.Add(uIPanelInfo);
            }
            else //UIPanel在该List中,更新path值
            {
                uIPanelInfo.Path = path;
            }
        }
        WriteToJsonFile<List<UIPanelInfo>>(_jsonFolderPath, _jsonFileName, _uIPanelInfos); //将更新后的模板信息写入Json文件中去
        AssetDatabase.Refresh(); //刷新资源
    }



    /// <summary>
    /// 根据对应的UIPanel类型，创建游戏实例对象，并返回该对象的BaseUIPanel脚本
    /// </summary>
    /// <param name="uIPanelType">UIPanlel的类型</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public BaseUIPanel GetUIPanel(UIPanelType uIPanelType)
    {
        if (_uIPanelInfos == null)
            return null;

        string path = _uIPanelInfos.TrySearchUIPanel(uIPanelType).Path;

        #region 可能会出现的错误，实际开发时，可不用添加这块代码

        if (path == null)
            throw new Exception("找不到该UIPanelType的Prefab");
        if (Resources.Load(path) == null)
            throw new Exception("找不到该UIPanelType的Prefab");

        #endregion 可能会出现的错误，实际开发时，可不用添加这块代码

        GameObject insUIPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject; //创建游戏实例对象
        insUIPanel.transform.SetParent(CanvasTransform, false); //设置父级对象
        return insUIPanel.GetComponent<BaseUIPanel>(); //返回BaseUIPanel脚本
    }

    /// <summary>
    /// 将对应的UIPanel压入栈中，并调用OnEnter方法
    /// </summary>
    /// <param name="uIPanelType"></param>
    public void PushUIPanel(UIPanelType uIPanelType)
    {
        //如果当前栈为空
        if (_currentUIPanels == null)
            _currentUIPanels = new Stack<BaseUIPanel>();
        //如果当前栈不为空，就把栈顶的UIPanel暂停
        if (_currentUIPanels.Count > 0)
        {
            BaseUIPanel topUIPanel = _currentUIPanels.Peek();
            topUIPanel.OnPause();
        }
        BaseUIPanel newUIPanel = GetUIPanel(uIPanelType);
        _currentUIPanels.Push(newUIPanel);
        newUIPanel.OnEnter();
    }

    /// <summary>
    /// 输入顶级UIPanel的类型，就能弹出对应的UIPanel，并调用OnExit方法
    /// </summary>
    /// <param name="uIPanelType"></param>
    public void PopUIPanel()
    {
        //如果当前栈为空，就返回
        if (_currentUIPanels == null)
            return;
        //如果当前栈中没有UIPanel，就返回
        if (_currentUIPanels.Count <= 0)
            return;

        //弹出栈顶的UIPanel，并调用OnExit方法
        BaseUIPanel topUIPanel = _currentUIPanels.Pop();
        topUIPanel.OnEixt();
        //如果当前栈不为空，就把栈顶的UIPanel恢复
        if (_currentUIPanels.Count == 0)
            return;
        topUIPanel = _currentUIPanels.Peek();
        topUIPanel.OnResume();
    }

    /// <summary>
    /// 摧毁场景中原来已经存在的UIPanel
    /// </summary>
    private void CheckUIPanelWhenGameBegin()
    {
        for (int i = 0; CanvasTransform.childCount > i;)
        {
            GameObject.DestroyImmediate(CanvasTransform.GetChild(i).gameObject);
        }
    }
}