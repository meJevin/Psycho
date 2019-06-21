using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System;
using System.Threading;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

[SerializeField]
public class RequestEvent : UnityEvent<WebRequestHandler>
{ }


public class WebRequestHandler : MonoBehaviour
{
    public ErrorScreen errorScreen;
    public LoadingScreen loadingScreen;

    [Header("Info")]
    public string requesterName = "";

    [Header("Data")]
    public string downloadedText;
    public AudioClip downloadedClip;
    public Texture2D downloadedTexture;

    [Header("Request")]
    public bool gotResponce = false;
    public long responceCode;

    [Header("Events")]
    public RequestEvent OnRequestSuccessful = new RequestEvent();
    public RequestEvent OnRequestFailed = new RequestEvent();

    public bool RequestSuccessful
    {
        get
        {
            if (responceCode >= 200 && responceCode < 300)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private set
        {

        }
    }

    public Dictionary<string, bool> requesters = new Dictionary<string, bool>();

    void Awake()
    {
        errorScreen = Resources.FindObjectsOfTypeAll<ErrorScreen>()[0];
        loadingScreen = Resources.FindObjectsOfTypeAll<LoadingScreen>()[0];
    }

    public void TextRequest(string path)
    {
        gameObject.SetActive(true);
        StartCoroutine(PerformTextRequest(path));
    }

    public void ImageRequest(string path)
    {
        gameObject.SetActive(true);
        StartCoroutine(PerformImageRequest(path));
    }

    public void AudioRequest(string path)
    {
        gameObject.SetActive(true);
        StartCoroutine(PerformAudioRequest(path));
    }

    public void Reset()
    {
        downloadedClip = null;
        downloadedText = "";
        downloadedTexture = null;
        gotResponce = false;
        responceCode = 0;
    }

    public IEnumerator PerformImageRequest(string path, Action<UnityWebRequest> requestCutomization = null)
    {
        Reset();

        requesters.Add("Texture", false);

        loadingScreen.gameObject.SetActive(true);
        loadingScreen.TieTo(this);

        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path);

        if (uwr.error != null)
        {
            errorScreen.ShowMessage(uwr.error);
            requesters.Remove("Texture");
            gotResponce = true;
            yield break;
        }

        if (requestCutomization != null)
        {
            requestCutomization.Invoke(uwr);
        }

        yield return uwr.SendWebRequest();

        gotResponce = true;
        downloadedTexture = DownloadHandlerTexture.GetContent(uwr);

        responceCode = uwr.responseCode;

        requesters.Remove("Texture");

        if (!(responceCode >= 200 && responceCode < 300))
        {
            if (responceCode == 0)
            {
                if (uwr.isNetworkError)
                {
                    errorScreen.ShowMessage(uwr.error);
                }
                else
                {
                    errorScreen.ShowMessage("Please check your internet connection");
                }
            }
            else
            {
                List<string> title = downloadedText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where((str) => (str.Contains("title"))).ToList();

                Debug.Log(title[0]);
                if (title.Count == 1)
                {
                    string errorTitle = title[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                    errorScreen.ShowMessage(errorTitle);
                }
            }

            OnRequestFailed.Invoke(this);
            yield break;
        }

        if (requesters.Count == 0 && RequestSuccessful)
        {
            OnRequestSuccessful.Invoke(this);
        }
    }

    public IEnumerator PerformTextRequest(string path, string method = "GET", string json = null, Action<UnityWebRequest> requestCutomization = null, WWWForm postParams = null)
    {
        Reset();

        requesters.Add("Text", false);

        loadingScreen.gameObject.SetActive(true);
        loadingScreen.TieTo(this);

        UnityWebRequest uwr = new UnityWebRequest(path, method);

        if (postParams != null && method == "POST")
        {
            uwr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            uwr = UnityWebRequest.Post(path, postParams);
        }

        if (uwr.error != null)
        {
            errorScreen.ShowMessage(uwr.error);
            gotResponce = true;

            requesters.Remove("Text");
            yield break;
        }

        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        if (json != null)
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.SetRequestHeader("Content-Type", "application/json");
        }

        if (requestCutomization != null)
        {
            requestCutomization.Invoke(uwr);
        }

        yield return uwr.SendWebRequest();

        gotResponce = true;

        downloadedText = uwr.downloadHandler.text;

        responceCode = uwr.responseCode;

        requesters.Remove("Text");

        if (!(responceCode >= 200 && responceCode < 300))
        {
            if (responceCode == 0)
            {
                if (uwr.isNetworkError)
                {
                    errorScreen.ShowMessage(uwr.error);
                }
                else
                {
                    errorScreen.ShowMessage("Please check your internet connection");
                }
            }
            else
            {
                List<string> title = downloadedText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where((str) => (str.Contains("title"))).ToList();

                if (title.Count == 1)
                {
                    Debug.Log(title[0]);
                    string errorTitle = title[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                    errorScreen.ShowMessage(errorTitle);
                    Debug.Log(errorTitle);
                }
            }

            OnRequestFailed.Invoke(this);
            yield break;
        }

        if (requesters.Count == 0 && RequestSuccessful)
        {
            OnRequestSuccessful.Invoke(this);
        }
    }

    public IEnumerator PerformAudioRequest(string path, Action<UnityWebRequest> requestCutomization = null)
    {
        Reset();

        requesters.Add("Audio", false);

        loadingScreen.gameObject.SetActive(true);
        loadingScreen.TieTo(this);

        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);

        if (uwr.error != null)
        {
            errorScreen.ShowMessage(uwr.error);
            gotResponce = true;

            requesters.Remove("Audio");
            yield break;
        }

        if (requestCutomization != null)
        {
            requestCutomization.Invoke(uwr);
        }

        yield return uwr.SendWebRequest();

        gotResponce = true;
        downloadedClip = DownloadHandlerAudioClip.GetContent(uwr);

        responceCode = uwr.responseCode;

        requesters.Remove("Audio");

        if (!(responceCode >= 200 && responceCode < 300))
        {
            if (responceCode == 0)
            {
                if (uwr.isNetworkError)
                {
                    errorScreen.ShowMessage(uwr.error);
                }
                else
                {
                    errorScreen.ShowMessage("Please check your internet connection");
                }
            }
            else
            {
                List<string> title = downloadedText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where((str) => (str.Contains("title"))).ToList();

                Debug.Log(title[0]);
                if (title.Count == 1)
                {
                    string errorTitle = title[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                    errorScreen.ShowMessage(errorTitle);
                }
            }

            OnRequestFailed.Invoke(this);
            yield break;
        }

        if (requesters.Count == 0 && RequestSuccessful)
        {
            OnRequestSuccessful.Invoke(this);
        }
    }
}
