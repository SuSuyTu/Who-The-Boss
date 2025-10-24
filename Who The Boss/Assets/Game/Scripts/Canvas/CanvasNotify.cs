using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the notification canvas, including loading and notification messages.
/// </summary>
public class CanvasNotify : FastSingleton<CanvasNotify>
{
    [Header("Loading")]
    [SerializeField] GameObject loading;
    [SerializeField] GameObject loadingIcon;
    [Header("Notify")]
    [SerializeField] private List<Notify> notifyList;

    public bool showing = false;
    private Coroutine loadingCoroutine;


    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Shows or hides the loading screen.
    /// </summary>
    /// <param name="isShow">If true, shows the loading screen; otherwise, hides it.</param>
    public void ShowLoading(bool isShow)
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }

        //notifyList.ForEach(x => x.gameObject.SetActive(false));

        if (isShow)
        {
            loading.SetActive(true);
            loadingIcon.LeanRotateAround(Vector3.forward, 360, 2f).setLoopClamp();
            loadingCoroutine = StartCoroutine(ShowLoadingDelay());
        }
        else
        {
            loading.SetActive(false);
        }
    }

    /// <summary>
    /// Shows a notification with the specified parameters.
    /// </summary>
    /// <param name="_type">The type of notification.</param>
    /// <param name="_titleMess">The title message.</param>
    /// <param name="_contentMess">The content message.</param>
    /// <param name="_notifyMess">The notification message.</param>
    /// <param name="_confirmBtn">The action to perform when the confirm button is clicked.</param>
    public void ShowNotify(EnumType.NotifyType _type, string _titleMess, string _contentMess, string _notifyMess, Action _confirmCallback)
    {
        loading.SetActive(false);
        notifyList.ForEach(x => x.gameObject.SetActive(notifyList.IndexOf(x) == (int)_type));
        notifyList[(int)_type].SetMessage(_titleMess, _contentMess, _notifyMess, _confirmCallback);
    }

    public void ShowNotify(EnumType.NotifyType _type, string _titleMess, string _contentMess, string _notifyMess, Action _confirmCallback, Action _canelCallback, Action _closeCallback)
    {
        loading.SetActive(false);
        notifyList.ForEach(x => x.gameObject.SetActive(notifyList.IndexOf(x) == (int)_type));
        notifyList[(int)_type].SetMessage(_titleMess, _contentMess, _notifyMess, _confirmCallback, _canelCallback, _closeCallback);
    }

    public void ClearAllNotify()
    {
        if (notifyList.Count > 0)
        {
            for (int i = 0; i < notifyList.Count; i++)
            {
                notifyList[i].gameObject.SetActive(false);
            }
        }
        ShowLoading(false);
    }
    
    public IEnumerator ShowLoadingDelay()
    {
        yield return new WaitForSecondsRealtime(20f);
        ShowLoading(false);
    }
}

