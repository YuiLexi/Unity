using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类需要挂载到每一个Panel的预制件上，因此继承MonoBehaviour
/// </summary>
public abstract class BaseUIPanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    /// <summary>
    /// 打开时需要执行的方法
    /// </summary>
    public virtual void OnEnter()
    {
        this.gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 暂停时需要执行的方法
    /// </summary>
    public virtual void OnPause()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 重新启动时需要执行的方法
    /// </summary>
    public virtual void OnResume()
    {
        this.gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 关闭时需要执行的方法
    /// </summary>
    public virtual void OnEixt()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 0.5f);
    }
}