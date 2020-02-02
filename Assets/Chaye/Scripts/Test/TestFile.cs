using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestFile : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.dataPath);
        Debug.Log(Application.persistentDataPath);
        //Debug.Log(ReadTextFileInStreamingAssets("/AVGEngine/Scripts/Chinese", "Main.txt"));
        //WriteTextFileInStreamingAssets("/AVGEngine/Scripts/Chinese", "Main.txt", "鸡鸣狗跳\n神龙野马");

        List<FileInfo> result = new List<FileInfo>();
        GetAllFilesOfFolderPath(Application.streamingAssetsPath, result, "txt");
        for (int i = 0; i < result.Count; i++) {
            Debug.Log(result[i].Name);
            Debug.Log(result[i].FullName);
            Debug.Log(result[i].DirectoryName);
        }
    }


    public string ReadTextFileInFolderPath(string folderPath, string fileName) {
        string path = folderPath + "/" + fileName;
        return File.ReadAllText(path, System.Text.Encoding.UTF8);
    }

    public void WriteTextFileInFolderPath(string folderPath, string fileName, string contexts) {
        string fullPath = folderPath + "/" + fileName;
        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
        File.WriteAllText(fullPath, contexts);
    }

    public void GetAllFilesOfFolderPath(string folderPath, List<FileInfo> result, string fileFormat) {
        if (!Directory.Exists(folderPath)) {
            return;
        }
        foreach (string subdir in Directory.GetDirectories(folderPath)) {
            GetAllFilesOfFolderPath(subdir, result, fileFormat);
        }
        DirectoryInfo folder = new DirectoryInfo(folderPath);
        FileInfo[] files = folder.GetFiles("*." + fileFormat);
        result.AddRange(files);
    }
}
