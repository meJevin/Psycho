using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhraseGridController : MonoBehaviour
{
    [Header("Dummy Object")]
    public GameObject Dummy;

    [Header("Content Control")]
    public GameObject Content;
    public GridLayoutGroup ItemsGridLayoutGroup;

    public List<GameObject> Children = new List<GameObject>();

    public Vector2 MinSize = new Vector2(35, 35);

    [Header("Web Request")]
    public string ListPath;

    WebRequestHandler MusicListRequester;

    int DesiredAmount = 13;

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

        for (int i = 0; i < DesiredAmount; ++i)
        {
            AudioButtonItem audioButton = audioButtons[Random.Range(0, audioButtons.Length - 1)];

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
        AddPhraseItem(context.downloadedClip, context.downloadedTexture, context.requesterName);

        Destroy(context);
    }

    // Из загруженной музяки делает предметы
    void MusicItemDownloadFail(WebRequestHandler context)
    {
        Destroy(context);
    }

    public void ClearChildren()
    {
        foreach (GameObject obj in Children)
        {
            DestroyImmediate(obj);
        }

        Children.Clear();

        ResizeChildren();
    }

    public void ResizeChildren()
    {
        Rect rect = Content.GetComponent<RectTransform>().rect;

        ItemsGridLayoutGroup.constraintCount = (int)Mathf.CeilToInt((Mathf.Sqrt(Children.Count)));

        ItemsGridLayoutGroup.cellSize = new Vector2((rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount,
            (rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount);

        while (ItemsGridLayoutGroup.cellSize.magnitude < MinSize.magnitude)
        {
            --ItemsGridLayoutGroup.constraintCount;

            ItemsGridLayoutGroup.cellSize = new Vector2((rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount,
                (rect.height - ItemsGridLayoutGroup.spacing.x * (ItemsGridLayoutGroup.constraintCount + 1)) / ItemsGridLayoutGroup.constraintCount);
        }
    }

    public void AddPhraseItem(AudioClip audio, Texture2D image, string caption)
    {
        GameObject objAdded = Instantiate(Dummy, Content.transform);
        objAdded.GetComponent<AudioSource>().clip = audio;
        objAdded.GetComponent<Image>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0, 0));
        objAdded.name = caption;

        Children.Add(objAdded);

        ResizeChildren();
    }
}
