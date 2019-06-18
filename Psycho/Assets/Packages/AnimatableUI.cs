using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(CanvasGroup))]

public class AnimatableUI : MonoBehaviour
{
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEndInit = new UnityEvent();

    [SerializeField]
    private List<TweenSequence> UIElementSequences;

    public bool Initialized = false;

    public virtual void Start()
    {
        OnStart.Invoke();

        Init();
    }

    public virtual void Init()
    {
        UIElementSequences = new List<TweenSequence>();

        foreach (var seq in GetComponents<TweenBase>())
        {
            TweenSequence addedSeq = gameObject.AddComponent<TweenSequence>();

            addedSeq.tweenSequence = new TweenBase[] { seq };
            addedSeq.sequenceName = seq.MyTweenName;

            UIElementSequences.Add(addedSeq);
        }

        Initialized = true;

        OnEndInit.Invoke();
    }

    public virtual void Play(string sequenceName)
    {
        gameObject.SetActive(true);

        try
        {
            UIElementSequences.Find((seq) => seq.sequenceName == sequenceName).BeginSequence();
        }
        catch (NullReferenceException nullException)
        {
            Debug.Log("Could not play sequence with name: " + sequenceName + " on object " + name);
        }
    }
}