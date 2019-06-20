using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhraseGridController : MonoBehaviour
{
    public GameObject Dummy;

    public GameObject Content;

    public GridLayoutGroup ItemsGridLayoutGroup;

    public int amount = 10;

    public List<GameObject> children = new List<GameObject>();

    public Vector2 MinSize = new Vector2(35, 35);

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
    }

    public void ChangeAmount(Slider slider)
    {
        Amount = (int)slider.value;
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

        ItemsGridLayoutGroup.constraintCount = (int)Mathf.CeilToInt((Mathf.Sqrt(Amount)));

        ItemsGridLayoutGroup.cellSize = new Vector2((rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount,
            (rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount);

        while (ItemsGridLayoutGroup.cellSize.magnitude < MinSize.magnitude)
        {
            --ItemsGridLayoutGroup.constraintCount;

            ItemsGridLayoutGroup.cellSize = new Vector2((rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount,
                (rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount);
        }
    }

    public void AddPhraseItem()
    {
        GameObject objAdded = Instantiate(Dummy, Content.transform);

        children.Add(objAdded);
    }
}
