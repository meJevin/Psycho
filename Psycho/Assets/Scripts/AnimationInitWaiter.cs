using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class AnimationInitWaiter : MonoBehaviour
{
    bool done = false;

    public UnityEvent OnInitDone = new UnityEvent();

    [SerializeField]
    public List<AnimatableUI> animatableUIs;

    void Start()
    {
        animatableUIs = FindObjectsOfType<AnimatableUI>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        while (!done)
        {
            foreach (var AUI in animatableUIs)
            {
                if (!AUI.Initialized)
                {
                    return;
                }
            }

            done = true;
            OnInitDone.Invoke();
        }
    }
}
