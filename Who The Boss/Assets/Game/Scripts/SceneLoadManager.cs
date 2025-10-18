using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] protected Button logInBuutton;
    void Start()
    {
        logInBuutton.onClick.AddListener(OnLogInButtonClick);
    }
    
    protected virtual void OnLogInButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
