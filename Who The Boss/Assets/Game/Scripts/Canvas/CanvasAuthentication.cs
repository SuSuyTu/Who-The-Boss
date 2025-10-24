using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAuthentication : UICanvas
{
    [SerializeField] private TMP_InputField inputUserName;
    [SerializeField] private TMP_InputField inputPassword;

    [SerializeField] private GameObject errorInlineMessage;

    [SerializeField] private Button showPasswordButton;
    [SerializeField] private Button hidePasswordButton;
    [SerializeField] private Button logInButton;
    [SerializeField] private Button exitButton;
    protected virtual void OnEnable()
    {
        logInButton.onClick.AddListener(OnLogInButtonClick);
        showPasswordButton.onClick.AddListener(OnShowHidePasswordButtonClick);
        hidePasswordButton.onClick.AddListener(OnShowHidePasswordButtonClick);
    }

    protected virtual void OnDisable()
    {
        logInButton.onClick.RemoveListener(OnLogInButtonClick);
        showPasswordButton.onClick.RemoveListener(OnShowHidePasswordButtonClick);
        hidePasswordButton.onClick.RemoveListener(OnShowHidePasswordButtonClick);
    }

    protected virtual void OnLogInButtonClick()
    {
        errorInlineMessage.SetActive(false);
        CanvasNotify.instance.ShowLoading(true);
        string userName = inputUserName.text;
        string password = inputPassword.text;

        CheckUser(userName, password);
    }

    protected virtual void OnShowHidePasswordButtonClick()
    {
        int caretPos = inputPassword.caretPosition;
        int selectionAnchor = inputPassword.selectionAnchorPosition;
        int selectionFocus = inputPassword.selectionFocusPosition;

        if (inputPassword.contentType == TMP_InputField.ContentType.Password)
        {
            inputPassword.contentType = TMP_InputField.ContentType.Standard;
            showPasswordButton.gameObject.SetActive(true);
            hidePasswordButton.gameObject.SetActive(false);
        }
        else
        {
            inputPassword.contentType = TMP_InputField.ContentType.Password;
            showPasswordButton.gameObject.SetActive(false);
            hidePasswordButton.gameObject.SetActive(true);
        }

        inputPassword.ForceLabelUpdate();
        inputPassword.caretPosition = caretPos;
        inputPassword.selectionAnchorPosition = selectionAnchor;
        inputPassword.selectionFocusPosition = selectionFocus;
        inputPassword.textComponent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    protected virtual void CheckUser(string userName, string password)
    {
        StartCoroutine(CrudUser.CheckAccount(new Users(userName, password),
                                            () =>
                                            {
                                                SceneLoadManager.instance.LoadScene(Constants.MAIN_MENU_SCENE);
                                            },
                                            () =>
                                            {
                                                errorInlineMessage.SetActive(true);
                                            }));
    }
}
