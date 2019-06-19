using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhraseGridController : MonoBehaviour
{
    public GameObject Dummy;

    public GameObject Content;

    public GridLayoutGroup GridLayoutGroup;

    public int amount = 10;

    void Start()
    {
        for (int i = 0; i < amount; ++i)
        {
            AddPhraseItem();
        }

        ResizeChildren();
    }

    public void ResizeChildren()
    {
        Rect rect = GetComponent<RectTransform>().rect;

        GridLayoutGroup.constraintCount = Mathf.CeilToInt((Mathf.Sqrt(amount)));

        GridLayoutGroup.cellSize = new Vector2(rect.width / 2 / GridLayoutGroup.constraintCount, rect.width / 2 / GridLayoutGroup.constraintCount);
    }

    public void AddPhraseItem()
    {
        GameObject objAdded = GameObject.Instantiate(Dummy, Content.transform);
    }
}
