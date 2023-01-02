using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ImageDownloader
{
    const string Saved_Sprite_List_Key = "SavedSpriteListKey";
    static ImageDownloader instance;
    public static ImageDownloader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ImageDownloader();
                instance.Init();
            }
            return instance;
        }
    }

    List<string> savedSpriteList;

    void Init()
    {
        string temp = PlayerPrefs.GetString(Saved_Sprite_List_Key, "[]");
        savedSpriteList = JsonConvert.DeserializeObject<List<string>>(temp);
    }

    public void GetSprite(string url, Action<Sprite> callback)
    {
        if (savedSpriteList.Contains(url))
        {
            Texture2D texture = LoadTexture(url);
            callback.Invoke(Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one * 0.5f));
        }
        else
        {
            LOAGameManager.Instance.StartCoroutine(GetTexture(url, callback));
        }
    }

    IEnumerator GetTexture(string url, Action<Sprite> callback)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
            SaveTexture(texture, url);
            callback.Invoke(Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one * 0.5f));
        }
    }

    void SaveTexture(Texture2D texture, string url)
    {
        byte[] texAsByte = texture.EncodeToPNG();
        string texAsString = Convert.ToBase64String(texAsByte);

        PlayerPrefs.SetString(url, texAsString);
        savedSpriteList.Add(url);
        PlayerPrefs.SetString(Saved_Sprite_List_Key, JsonConvert.SerializeObject(savedSpriteList));
        PlayerPrefs.Save();
    }

    Texture2D LoadTexture(string url)
    {
        string temp = PlayerPrefs.GetString(url);
        byte[] texAsByte = Convert.FromBase64String(temp);
        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        texture.LoadImage(texAsByte);
        texture.Apply();

        return texture;
    }

#if UNITY_EDITOR
[MenuItem("Tools/Clear saved textures")]
    static void ClearSavedTextures()
    {
        string temp = PlayerPrefs.GetString(Saved_Sprite_List_Key, "[]");
        List<string> tempSavedSpriteList = JsonConvert.DeserializeObject<List<string>>(temp);

        foreach (var item in tempSavedSpriteList)
            PlayerPrefs.DeleteKey(item);

        PlayerPrefs.DeleteKey(Saved_Sprite_List_Key);
        PlayerPrefs.Save();

        Debug.Log("ImageDownloader:: All textures have been cleared.");
    }
#endif
}
