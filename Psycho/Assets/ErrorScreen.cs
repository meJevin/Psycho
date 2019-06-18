using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ErrorScreen : MonoBehaviour
{
    [Serializable]
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            this.message = message;

            showTimeLeft = showTime;
        }

        public string message;
        public float showTimeLeft;

        float showTime = 2.0f;
    }

    public List<ErrorMessage> messageQueue = new List<ErrorMessage>();
    ErrorMessage currentMessage = null;

    private AnimatableUI animController;

    public Text errorText;

    public float messageCoolDown = 0.4f;
    public float messageCoolDownLeft = 0.0f;

    void Awake()
    {
        animController = GetComponent<AnimatableUI>();
    }

    public void ShowMessage(string message)
    {
        messageQueue.Add(new ErrorMessage(message));
    }

    void Update()
    {
        if (messageCoolDownLeft >= 0)
        {
            messageCoolDownLeft -= Time.deltaTime;
            return;
        }

        if (currentMessage != null)
        {
            // Мы уже показываем сообщение, надо работать с ним!
            currentMessage.showTimeLeft -= Time.deltaTime;

            if (currentMessage.showTimeLeft <= 0)
            {
                animController.Play("Hide");
                currentMessage = null;
                messageCoolDownLeft = messageCoolDown;
            }
        }

        if (messageQueue.Count == 0)
        {
            return;
        }

        // В очереди сообщений че-то есть! Давай первого вынем и будем показывать
        currentMessage = messageQueue[0];
        messageQueue.RemoveAt(0);
        errorText.text = currentMessage.message;
        animController.Play("Show");
    }
}
