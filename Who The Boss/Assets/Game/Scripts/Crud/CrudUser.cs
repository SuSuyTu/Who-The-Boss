using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class CrudUser
{
    public static IEnumerator CheckAccount(Users users, Action onSuccess = null, Action onError = null)
    {
        string json = JsonUtility.ToJson(users);


        using (UnityWebRequest unityWebRequest = new UnityWebRequest(APIUrlConstants.CHECK_USER_URL, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

            yield return unityWebRequest.SendWebRequest();
            CanvasNotify.instance.ShowLoading(false);
            string response = unityWebRequest.downloadHandler.text;
            LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                if (unityWebRequest.responseCode == 400) 
                {
                    Debug.LogError($"Login failed - Bad Request: {loginResponse.error}");
                }
                else if (unityWebRequest.responseCode == 401) 
                {
                    Debug.LogError($"Login failed - Unauthorized: {loginResponse.error}");
                }
                else 
                {
                    Debug.LogError($"Login failed: {loginResponse.error}");
                }
                onError?.Invoke();
            }
            else
            {
                Debug.Log("Login success: " + unityWebRequest.downloadHandler.text);
                onSuccess?.Invoke();
            }
        }
    }

    public static IEnumerator GetAllUser(Action<Users[]> onSuccess = null, Action<string> onError = null)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(APIUrlConstants.GET_ALL_USER))
        {
            yield return unityWebRequest.SendWebRequest();
            CanvasNotify.instance.ShowLoading(false);

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || 
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string error = unityWebRequest.error;
                Debug.LogError($"GetAccount Error: {error}");
                onError?.Invoke(error);
            }
            else
            {
                string jsonResponse = unityWebRequest.downloadHandler.text;
                Debug.Log($"GetAccount Success: {jsonResponse}");
                
                try
                {
                    Users[] users = JsonHelper.FromJson<Users>(jsonResponse);
                    onSuccess?.Invoke(users);
                }
                catch (Exception ex)
                {
                    string errorMsg = $"JSON Parse Error: {ex.Message}";
                    Debug.LogError(errorMsg);
                    onError?.Invoke(errorMsg);
                }
            }
        }
    }
}
