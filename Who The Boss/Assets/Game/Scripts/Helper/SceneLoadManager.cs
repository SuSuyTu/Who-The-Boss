using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : FastSingleton<SceneLoadManager>
{
    protected virtual void OnLogInButtonClick()
    {
        LoadScene("MainMenu");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName, mode);
    }

    public AsyncOperation LoadSceneAsync(string sceneName)
    {
        return SceneManager.LoadSceneAsync(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}