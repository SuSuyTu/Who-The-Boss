using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Manages the UI canvases in the application.
/// </summary>
public class CanvasManager : FastSingleton<CanvasManager>
{
    // Dictionary for quick query of UI prefabs
    private Dictionary<System.Type, UICanvas> uiCanvasPrefab = new Dictionary<System.Type, UICanvas>();

    // List of UI resources
    private UICanvas[] uiResources;

    // Dictionary for active UIs
    private Dictionary<System.Type, UICanvas> uiCanvas = new Dictionary<System.Type, UICanvas>();

    public Transform CanvasParentTF;
    public bool isMobile = false;

    #region Canvas

    /// <summary>
    /// Opens the UI of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas to open.</typeparam>
    /// <returns>The opened UI canvas.</returns>
    public T OpenUI<T>() where T : UICanvas
    {
        UICanvas canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();
        return canvas as T;
    }

    /// <summary>
    /// Opens the UI of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas to open.</typeparam>
    /// <returns>The opened UI canvas.</returns>
    public T OpenUI<T>(object canvasData) where T : UICanvas
    {
        UICanvas canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open(canvasData);
        return canvas as T;
    }

    /// <summary>
    /// Closes the UI of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas to close.</typeparam>
    public void CloseUI<T>() where T : UICanvas
    {
        if (IsOpened<T>())
        {
            GetUI<T>().Close();
        }
    }

    /// <summary>
    /// Checks if the UI of the specified type is opened.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas to check.</typeparam>
    /// <returns>True if the UI is opened, false otherwise.</returns>
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && uiCanvas[typeof(T)].gameObject.activeInHierarchy;
    }

    /// <summary>
    /// Checks if the UI of the specified type is loaded.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas to check.</typeparam>
    /// <returns>True if the UI is loaded, false otherwise.</returns>
    public bool IsLoaded<T>() where T : UICanvas
    {
        System.Type type = typeof(T);
        return uiCanvas.ContainsKey(type) && uiCanvas[type] != null;
    }

    /// <summary>
    /// Gets the UI of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas to get.</typeparam>
    /// <returns>The UI canvas of the specified type.</returns>
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            UICanvas canvas = Instantiate(GetUIPrefab<T>(), CanvasParentTF);
            uiCanvas[typeof(T)] = canvas;
        }

        return uiCanvas[typeof(T)] as T;
    }

    /// <summary>
    /// Gets the UI prefab of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the UI canvas prefab to get.</typeparam>
    /// <returns>The UI canvas prefab of the specified type.</returns>
    private T GetUIPrefab<T>() where T : UICanvas
    {
        if (!uiCanvasPrefab.ContainsKey(typeof(T)))
        {
            if (uiResources == null)
            {
                if (isMobile)
                {
                    #if TABLET
                        uiResources = Resources.LoadAll<UICanvas>("UITablet/");
                    #else 
                        uiResources = Resources.LoadAll<UICanvas>("UIMobile/");
                    #endif                    
                    
                }
                else
                {
                    uiResources = Resources.LoadAll<UICanvas>("UI/");
                }
            }

            for (int i = 0; i < uiResources.Length; i++)
            {
                if (uiResources[i] is T)
                {
                    uiCanvasPrefab[typeof(T)] = uiResources[i];
                    break;
                }
            }
        }

        return uiCanvasPrefab[typeof(T)] as T;
    }

    #endregion

    #region Back Button

    private Dictionary<UICanvas, UnityAction> BackActionEvents = new Dictionary<UICanvas, UnityAction>();
    private List<UICanvas> backCanvas = new List<UICanvas>();

    /// <summary>
    /// Gets the top UI canvas in the back stack.
    /// </summary>
    private UICanvas BackTopUI
    {
        get
        {
            UICanvas canvas = null;
            if (backCanvas.Count > 0)
            {
                canvas = backCanvas[backCanvas.Count - 1];
            }

            return canvas;
        }
    }

    /// <summary>
    /// Handles the back button press event.
    /// </summary>
    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
        {
            BackActionEvents[BackTopUI]?.Invoke();
        }
    }

    /// <summary>
    /// Pushes a back action for the specified UI canvas.
    /// </summary>
    /// <param name="canvas">The UI canvas.</param>
    /// <param name="action">The action to perform on back press.</param>
    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (!BackActionEvents.ContainsKey(canvas))
        {
            BackActionEvents.Add(canvas, action);
        }
    }

    /// <summary>
    /// Adds the specified UI canvas to the back stack.
    /// </summary>
    /// <param name="canvas">The UI canvas to add.</param>
    public void AddBackUI(UICanvas canvas)
    {
        if (!backCanvas.Contains(canvas))
        {
            backCanvas.Add(canvas);
        }
    }

    /// <summary>
    /// Removes the specified UI canvas from the back stack.
    /// </summary>
    /// <param name="canvas">The UI canvas to remove.</param>
    public void RemoveBackUI(UICanvas canvas)
    {
        backCanvas.Remove(canvas);
    }

    /// <summary>
    /// Clears the back stack.
    /// </summary>
    public void ClearBackKey()
    {
        backCanvas.Clear();
    }

    #endregion
}
