using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System;

[Serializable]
public class AudioButtonItem
{
   public string audio;
   public string bg;
   public string caption;
}

public class MusicItemController : MonoBehaviour
{
    [Header("Dummy Object")]
    public GameObject Dummy;

    [Header("Content Control")]
    public GameObject Content;
    public GridLayoutGroup ItemsGridLayoutGroup;
    public ContentSizeFitter SizeFitter;

    public List<GameObject> Children = new List<GameObject>();

    [Header("Web Request")]
    public string ListPath;

    WebRequestHandler MusicListRequester;

    public void DownloadMusicList()
    {
        ClearChildren();

        MusicListRequester = gameObject.AddComponent<WebRequestHandler>();

        MusicListRequester.OnRequestSuccessful.AddListener(MusicListDownloadSuccess);
        MusicListRequester.OnRequestFailed.AddListener(MusicListDownloadFail);

        MusicListRequester.TextRequest(ListPath);
    }

    void MusicListDownloadSuccess(WebRequestHandler context)
    {
        AudioButtonItem[] audioButtons = JsonHelper.FromJson<AudioButtonItem>(MusicListRequester.downloadedText);

        foreach (AudioButtonItem audioButton in audioButtons)
        {
            WebRequestHandler temp = gameObject.AddComponent<WebRequestHandler>();

            temp.requesterName = audioButton.caption;

            temp.OnRequestSuccessful.AddListener(MusicItemDownloadSuccess);
            temp.OnRequestFailed.AddListener(MusicItemDownloadFail);

            temp.AudioRequest(audioButton.audio);
            temp.ImageRequest(audioButton.bg);
        }

        Destroy(context);
    }

    void MusicListDownloadFail(WebRequestHandler context)
    {
        Destroy(context);
    }


    // Из загруженной музяки делает предметы
    void MusicItemDownloadSuccess(WebRequestHandler context)
    {
        AddMusicItem(context.downloadedClip, context.downloadedTexture, context.requesterName);

        Destroy(context);
    }

    // Из загруженной музяки делает предметы
    void MusicItemDownloadFail(WebRequestHandler context)
    {
        Destroy(context);
    }

    // Убирает SizeFitter и делает по центру детей
    void RemoveAutoSize()
    {
        SizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    public void ClearChildren()
    {
        foreach (GameObject obj in Children)
        {
            RemovePhraseItem(obj);
        }

        Children.Clear();
    }

    public void RemovePhraseItem(GameObject item)
    {
        item.gameObject.SetActive(false);
        Destroy(item);

        float childrenWidth = ItemsGridLayoutGroup.padding.right + ItemsGridLayoutGroup.padding.left;
        foreach (var child in Children)
        {
            // Перед удалением ставлю active на false чтобы понять что объекта больше нет
            if (child.gameObject.activeSelf)
            {
                childrenWidth += child.GetComponent<RectTransform>().rect.width + ItemsGridLayoutGroup.spacing.x;
            }
        }

        if (childrenWidth < Content.transform.parent.GetComponent<RectTransform>().rect.width)
        {
            RemoveAutoSize();
        }
    }
    public void AddMusicItem(AudioClip audio, Texture2D image, string caption)
    {
        GameObject objAdded = Instantiate(Dummy, Content.transform);
        objAdded.GetComponent<AudioSource>().clip = audio;
        objAdded.GetComponent<Image>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0,0));
        objAdded.name = caption;

        Children.Add(objAdded);

        if (SizeFitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
        {
            float childrenWidth = ItemsGridLayoutGroup.padding.right + ItemsGridLayoutGroup.padding.left;
            foreach (var child in Children)
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
