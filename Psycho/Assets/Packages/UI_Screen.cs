using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]

public class UI_Screen : MonoBehaviour
{
    [Header("Scrren Events")]
    public UnityEvent OnStart = new UnityEvent();

    private List<TweenSequence> UIElementSequences;

    private void DisableScreen()
    {
        gameObject.SetActive(false);
    }

    private void EnableScreen()
    {
        gameObject.SetActive(true);
    }

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
    }

    public virtual void Play(string sequenceName)
    {
        UIElementSequences.Find((seq) => seq.sequenceName == sequenceName).BeginSequence();
    }
}