using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InputFieldHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Image inputImage;
    [SerializeField] private List<Sprite> inputSprites = new List<Sprite>();
    [SerializeField] private bool useErrorState = false;
    private bool isError = false;
    private bool isHovering = false;
    private bool isFocused = false;

    private void OnEnable()
    {
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
        }

        // Initialize with default sprite
        if (inputImage != null && inputSprites.Count > 0)
        {
            inputImage.sprite = inputSprites[0];
        }

        // Add listeners for all platforms
        if (inputField != null)
        {
            inputField.onSelect.AddListener(_ => OnInputSelected());
            inputField.onDeselect.AddListener(_ => OnInputDeselected());
        }
    }

    private void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onSelect.RemoveListener(_ => OnInputSelected());
            inputField.onDeselect.RemoveListener(_ => OnInputDeselected());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == inputField.gameObject)
        {
            isHovering = true;
            UpdateInputSprite();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == inputField.gameObject)
        {
            isHovering = false;
            UpdateInputSprite();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPress == inputField.gameObject)
        {
            UpdateInputSprite();
        }
    }

    private void OnInputSelected()
    {
        isFocused = true;
        UpdateInputSprite();
    }

    private void OnInputDeselected()
    {
        isFocused = false;
        isHovering = false;
        UpdateInputSprite();
    }

    private void UpdateInputSprite()
    {
        if (inputImage == null || inputSprites.Count < 3) return;

        if (useErrorState && isError)
        {
            inputImage.sprite = inputSprites[1];
        }
        else if (isFocused)
        {
            inputImage.sprite = inputSprites[2];
        }
        else if (isHovering && !Application.isMobilePlatform)
        {
            inputImage.sprite = inputSprites[2];
        }
        else
        {
            inputImage.sprite = inputSprites[0];
        }
    }

    public void SetErrorState(bool isError)
    {
        if (useErrorState)
        {
            this.isError = isError;
            UpdateInputSprite();
        }
    }
    
    public void SetFocusedState()
    {
        if (!useErrorState || !isError)
        {
            inputImage.sprite = inputSprites[2];
        }
    }
    
    public void ResetState()
    {
        isHovering = false;
        isFocused = false;
        UpdateInputSprite();
    }
}