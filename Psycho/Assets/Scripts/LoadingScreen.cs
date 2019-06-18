using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public bool active = false;

    public List<WebRequestHandler> tiedWebRequestHandlers;

    public float closeTimeOutLeft = 0;
    public float closeTimeOut = 0.75f;
    public bool pendingClose = false;

    private AnimatableUI animationController;

    public void TieTo(WebRequestHandler webRequsetHandler)
    {
        if (tiedWebRequestHandlers.FindIndex((req) => req == webRequsetHandler) != -1)
        {
            return;
        }

        tiedWebRequestHandlers.Add(webRequsetHandler);
    
        if (tiedWebRequestHandlers.Count == 1)
        {
            InitiateStart();
        }
    }

    void Awake()
    {
        animationController = GetComponent<AnimatableUI>();

        //tiedWebRequestHandlers = new List<WebRequestHandler>();

        closeTimeOutLeft = closeTimeOut;
    }

    void InitiateStart()
    {
        if (!pendingClose)
        {
            animationController.Play("Show");
        }

        active = true;
        pendingClose = false;
        closeTimeOutLeft = closeTimeOut;
    }

    void InitiateClose()
    {
        active = false;
        pendingClose = true;
    }

    void Update()
    {
        if (pendingClose)
        {
            closeTimeOutLeft -= Time.deltaTime;

            if (closeTimeOutLeft <= 0)
            {
                closeTimeOutLeft = closeTimeOut;
                pendingClose = false;
                animationController.Play("Hide");
            }
        }

        if (!active)
        {
            return;
        }

        List<int> indeciesToRemoveLoad = new List<int>();
        List<int> indeciesToRemoveRequest = new List<int>();

        for (int i = 0; i < tiedWebRequestHandlers.Count; ++i)
        {
            if (tiedWebRequestHandlers[i].gotResponce)
            {
                indeciesToRemoveRequest.Add(i);
            }
        }

        if (indeciesToRemoveLoad.Count + indeciesToRemoveRequest.Count == 0)
        {
            return;
        }
        else
        {
            foreach (var indx in indeciesToRemoveRequest)
            {
                if (indx >= 0 && indx < tiedWebRequestHandlers.Count)
                {
                    tiedWebRequestHandlers.RemoveAt(indx);
                }
            }
        
            if (tiedWebRequestHandlers.Count == 0)
            {
                InitiateClose();
            }
        }
    }
}
