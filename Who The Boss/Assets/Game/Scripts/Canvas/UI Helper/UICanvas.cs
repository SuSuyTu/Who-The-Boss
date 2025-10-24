using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a UI canvas in the application.
/// </summary>
public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    public bool IsDestroyOnClose = false;

    protected RectTransform m_RectTransform;
    //private Animator m_Animator;
    //private bool m_IsInit = false;
    //private float m_OffsetY = 0;

    protected void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    /// <summary>
    /// Initializes the UI canvas.
    /// </summary>
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Initializes the RectTransform component.
    /// </summary>
    protected void Init()
    {
        m_RectTransform = GetComponent<RectTransform>();
        //m_Animator = GetComponent<Animator>();

        //float ratio = (float)Screen.height / (float)Screen.width;

        //// safe area
        //if (ratio > 2.1f)
        //{
        //    Vector2 leftBottom = m_RectTransform.offsetMin;
        //    Vector2 rightTop = m_RectTransform.offsetMax;
        //    rightTop.y = -100f;
        //    m_RectTransform.offsetMax = rightTop;
        //    leftBottom.y = 0f;
        //    m_RectTransform.offsetMin = leftBottom;
        //    m_OffsetY = 100f;
        //}
        //m_IsInit = true;
    }

    /// <summary>
    /// Sets up the UI canvas by adding it to the back stack and pushing the back action.
    /// </summary>
    public virtual void Setup()
    {
        CanvasManager.instance.AddBackUI(this);
        CanvasManager.instance.PushBackAction(this, BackKey);
    }

    /// <summary>
    /// Handles the back key action.
    /// </summary>
    public virtual void BackKey()
    {

    }

    /// <summary>
    /// Opens the UI canvas.
    /// </summary>
    public virtual void Open()
    {
        gameObject.SetActive(true);
        //anim
    }

    public virtual void Open(object canvasData = null)
    {
        gameObject.SetActive(true);
        //anim
    }

    /// <summary>
    /// Closes the UI canvas.
    /// </summary>
    public virtual void Close()
    {
        CanvasManager.instance.RemoveBackUI(this);
        //anim
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
    }
}
