using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System;

public class MusicItemController : MonoBehaviour
{
    public GameObject Dummy;

    public GameObject Content;

    public GridLayoutGroup ItemsGridLayoutGroup;
    public ContentSizeFitter SizeFitter;

    public List<GameObject> children = new List<GameObject>();

    public string ListPath;
    public GameObject DummyMusicItem;

    WebRequestHandler MusicListRequester;

    void Start()
    {
    }

    public void DownloadMusicList()
    {
        ClearChildren();

        MusicListRequester = gameObject.AddComponent<WebRequestHandler>();

        MusicListRequester.OnRequestSuccessful.AddListener(DownloadMusicFromExistingList);

        MusicListRequester.TextRequest(ListPath);
    }

    void DownloadMusicFromExistingList(WebRequestHandler context)
    {
        string listText = MusicListRequester.downloadedText;

        string[] musicLinks = listText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string link in musicLinks)
        {
            WebRequestHandler temp = gameObject.AddComponent<WebRequestHandler>();
            temp.OnRequestSuccessful.AddListener(LoadMusicItem);
            temp.AudioRequest(link);
        }

        Destroy(context);
    }

    // Убирает SizeFitter и делает по центру детей
    void RemoveAutoSize()
    {
        SizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    // Из загруженной музяки делает предметы
    void LoadMusicItem(WebRequestHandler context)
    {
        AddPhraseItem(context.downloadedClip);

        Destroy(context);
    }

    public void ClearChildren()
    {
        foreach (GameObject obj in children)
        {
            RemovePhraseItem(obj);
        }

        children.Clear();
    }

    public void RemovePhraseItem(GameObject item)
    {
        item.gameObject.SetActive(false);
        Destroy(item);

        float childrenWidth = ItemsGridLayoutGroup.padding.right + ItemsGridLayoutGroup.padding.left;
        foreach (var child in children)
        {
            // Перед удалением ставлю active на false чтобы понять что объекта больше нет
            if (child.gameObject.activeSelf)
            {
                childrenWidth += child.GetComponent<RectTransform>().rect.width + ItemsGridLayoutGroup.spacing.x;
            }
        }

        Debug.Log(childrenWidth);
        Debug.Log(Content.transform.parent.GetComponent<RectTransform>().rect.width);

        if (childrenWidth < Content.transform.parent.GetComponent<RectTransform>().rect.width)
        {
            RemoveAutoSize();
        }
    }
    public void AddPhraseItem(AudioClip audio)
    {
        GameObject objAdded = Instantiate(Dummy, Content.transform);
        objAdded.GetComponent<AudioSource>().clip = audio;

        children.Add(objAdded);

        if (SizeFitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
        {
            float childrenWidth = ItemsGridLayoutGroup.padding.right + ItemsGridLayoutGroup.padding.left;
            foreach (var child in children)
            {
                childrenWidth += child.GetComponent<RectTransform>().rect.width + ItemsGridLayoutGroup.spacing.x;
            }

            if (childrenWidth > Content.transform.parent.GetComponent<RectTransform>().rect.width)
            {
                SizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }
    }
}
