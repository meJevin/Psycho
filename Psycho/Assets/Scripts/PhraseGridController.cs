using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhraseGridController : MonoBehaviour
{
    public GameObject Dummy;

    public GameObject Content;

    public int amount = 10;

    void Start()
    {
        for (int i = 0; i < amount; ++i)
        {
            AddPhraseItem();
        }
    }

    public void AddPhraseItem()
    {
        GameObject objAdded = GameObject.Instantiate(Dummy, Content.transform);
    }
}
