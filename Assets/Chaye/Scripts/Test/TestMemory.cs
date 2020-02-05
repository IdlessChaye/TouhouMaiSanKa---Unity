using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMemory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestM1());
    }

    IEnumerator TestM1() {
        yield return new WaitForSeconds(1);
        float time = 0.2f;
        yield return new WaitForSeconds(time);
        string path = Application.streamingAssetsPath + "/AVGEngine/AssetBundle/avgengine.unity3d";
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path); // 这里只有引导文件被加载了
        yield return new WaitForSeconds(time);
        assetBundle.LoadAsset<Texture2D>("0");  // 这里加载AB的内存镜像，并复制资源
        yield return new WaitForSeconds(time);

        int size = 100;
        List<Texture2D> list = new List<Texture2D>(size); 
        for (int i = 0;i < 30; i++) {
            string name = i.ToString();
            Texture2D t = assetBundle.LoadAsset<Texture2D>(name); // 这里开始从内存镜像中复制出来资源
            list.Add(t);
        }
        yield return new WaitForSeconds(time);
        for (int i = 30; i < 60; i++) {
            string name = i.ToString();
            Texture2D t = assetBundle.LoadAsset<Texture2D>(name);
            list.Add(t);
        }
        yield return new WaitForSeconds(time);
        for (int i = 60; i < size; i++) {
            string name = i.ToString();
            Texture2D t = assetBundle.LoadAsset<Texture2D>(name);
            list.Add(t);
        }
        yield return new WaitForSeconds(time);
        assetBundle.Unload(false); // 把AB内存镜像释放
        yield return new WaitForSeconds(time);
        Resources.UnloadUnusedAssets(); // 不知道释放了啥，反正内存下降了
        yield return new WaitForSeconds(time);
        list.Clear();
        Resources.UnloadUnusedAssets(); // 把所有没用到的资源释放
        yield return new WaitForSeconds(time);
        //assetBundle.Unload(true);
        yield return new WaitForSeconds(time);

        // 建议就是，AB也要分开打包，要不一起打包就要加载所有资源，没有必要

    }


    IEnumerator TestM2() {
        yield return new WaitForSeconds(1);
        float time = 0.2f;
        yield return new WaitForSeconds(time);
        string path = Application.streamingAssetsPath + "/AVGEngine/AssetBundle/avgengine.unity3d";
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path); // 这里只有引导文件被加载了
        
        

        int size = 100;
        List<Texture2D> list = new List<Texture2D>(size);
        for (int i = 0; i < 30; i++) {
            string name = i.ToString();
            Texture2D t = assetBundle.LoadAsset<Texture2D>(name); // 这里开始从内存镜像中复制出来资源
            list.Add(t);
        }
        
        for (int i = 30; i < 60; i++) {
            string name = i.ToString();
            Texture2D t = assetBundle.LoadAsset<Texture2D>(name);
            list.Add(t);
        }
        for (int i = 60; i < size; i++) {
            string name = i.ToString();
            Texture2D t = assetBundle.LoadAsset<Texture2D>(name);
            list.Add(t);
        }
        assetBundle.Unload(false); // 把AB内存镜像释放
        yield return new WaitForSeconds(time);
        Resources.UnloadUnusedAssets(); // 不知道释放了啥，反正内存下降了
        yield return new WaitForSeconds(time);
        list.Clear();
        Resources.UnloadUnusedAssets(); // 把所有没用到的资源释放
        yield return new WaitForSeconds(time);
        //assetBundle.Unload(true);
        yield return new WaitForSeconds(time);

        // 建议就是，AB也要分开打包，要不一起打包就要加载所有资源，没有必要

    }
}
