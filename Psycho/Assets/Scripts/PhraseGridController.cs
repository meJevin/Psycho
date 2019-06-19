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

    public List<GameObject> children = new List<GameObject>();

    public int Amount
    {
        get
        {
            return amount;
        }
        
        set
        {
            amount = value;

            Init();
        }
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        ClearChildren();

        for (int i = 0; i < Amount; ++i)
        {
            AddPhraseItem();
        }

        ResizeChildren();
    }

    public void ClearChildren()
    {
        foreach (GameObject obj in children)
        {
            DestroyImmediate(obj);
        }

        children.Clear();
    }

    public void ResizeChildren()
    {
        Rect rect = GetComponent<RectTransform>().rect;

        GridLayoutGroup.constraintCount = (int)(Mathf.Sqrt(Amount));

        GridLayoutGroup.cellSize = new Vector2((rect.width - GridLayoutGroup.spacing.x * GridLayoutGroup.constraintCount) / GridLayoutGroup.constraintCount,
            (rect.width - GridLayoutGroup.spacing.x * GridLayoutGroup.constraintCount) / GridLayoutGroup.constraintCount);
    }

    public void AddPhraseItem()
    {
        GameObject objAdded = Instantiate(Dummy, Content.transform);

        children.Add(objAdded);
    }
}
