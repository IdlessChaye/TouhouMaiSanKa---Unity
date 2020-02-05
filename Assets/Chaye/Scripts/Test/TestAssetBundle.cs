using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssetBundle : MonoBehaviour {
    // Start is called before the first frame update
    public UITexture texture;
    void Start() {
        LoadPrefabs();
    }

    void LoadPrefabs()//第二种直接从本地加载
    {
        string path =
#if UNITY_ANDROID
             Application.dataPath + "!assets"+ "/";
#else
             Application.streamingAssetsPath + "/";
#endif
        ;
        path = path + "AVGEngine/AssetBundle/avgengine.unity3d";
        print(path);
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        print(ab);
        Object[] obj = ab.LoadAllAssets<GameObject>();
        Texture2D t = ab.LoadAsset<Texture2D>("0");
        texture.mainTexture = t;
        foreach (var o in obj) {
            print(o);
        }
    }
}
