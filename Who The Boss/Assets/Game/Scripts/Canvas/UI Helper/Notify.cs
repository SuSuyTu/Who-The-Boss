using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notify : MonoBehaviour
{
    [SerializeField] CanvasNotify canvasNotify;
    [SerializeField] private TMP_Text titleTxt;
    [SerializeField] private TMP_Text contentTxt;
    [SerializeField] private TMP_Text notifyText;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button closeBtn;


    private void OnEnable()
    {
        CanvasNotify.instance.showing = true;
        if (cancelBtn != null)
        {
            cancelBtn.onClick.AddListener(CloseNotify);
        }
        if (acceptBtn != null)
        {
            acceptBtn.onClick.AddListener(CloseNotify);
        }
        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(CloseNotify);
        }
    }

    private void OnDisable()
    {
        CanvasNotify.instance.showing = false;
        if (cancelBtn != null)
        {
            cancelBtn.onClick.RemoveAllListeners();
        }
        if (acceptBtn != null)
        {
            acceptBtn.onClick.RemoveAllListeners();
        }
        if (closeBtn != null)
        {
            closeBtn.onClick.RemoveAllListeners();
        }
    }

    void CloseNotify()
    {
        gameObject.SetActive(false);
    }
    public void SetMessage(string _title, string _content, string _notify, Action _confirmCallback)
    {
        if (titleTxt != null)
        {
            titleTxt.text = _title;
        }
        if (contentTxt != null)
        {
            contentTxt.text = _content;
        }
        if (notifyText != null)
        {
            notifyText.text = _notify;
        }

        if (confirmBtn != null)
        {
            confirmBtn.onClick.AddListener(() =>
            {
                _confirmCallback?.Invoke();
            });
        }

    }

    public void SetMessage(string _title, string _content, string _notify, Action _confirmCallback, Action _canelCallback, Action _closeCallback)
    {
        if (titleTxt != null)
        {
            titleTxt.text = _title;
        }
        if (contentTxt != null)
        {
            contentTxt.text = _content;
        }
        if (notifyText != null)
        {
            notifyText.text = _notify;
        }

        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(() =>
            {
                _closeCallback?.Invoke();
            });
        }

        if (confirmBtn != null)
        {
            confirmBtn.onClick.AddListener(() =>
            {
                _confirmCallback?.Invoke();
            });
        }
        
        if (cancelBtn != null)
        {
            cancelBtn.onClick.AddListener(() =>
            {
                _canelCallback?.Invoke();
            });
        }
    }
}
