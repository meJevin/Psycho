using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleLoadsDoneHandler : MonoBehaviour
{

    public List<WebRequestHandler> requsetHandlers = new List<WebRequestHandler>();

    public UnityEvent OnMultipleLoadsDone = new UnityEvent();

    public bool active = false;

    public void Activate()
    {
        active = true;
    }

    public void DeActivate()
    {
        active = false;
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        foreach (var handler in requsetHandlers)
        {
            if (!handler.gotResponce)
            {
                //Debug.Log(gameObject.name + ": " + handler.gameObject.name + " hasn't downloaded yet!");
                return;
            }
        }

        DeActivate();
        OnMultipleLoadsDone.Invoke();
    }
}
