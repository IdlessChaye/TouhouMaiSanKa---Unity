using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class FileManager {
        private ConfigManager configManager;

        private PlayerRecordManager playerRecordManager;

        private ResourceManager resourceManager;

        private ConstData constData;

        private VoiceAssetBundleConfigManager voiceAssetBundleConfigManager;

        private VoiceAssetBundleResManager<AssetBundle> voiceAssetBundleResManager;

        

        private string language;

        public FileManager(ConfigManager configManager, PlayerRecordManager playerRecordManager, ResourceManager resourceManager, ConstData constData) {
            this.configManager = configManager;
            this.playerRecordManager = playerRecordManager;
            this.resourceManager = resourceManager;
            this.constData = constData;
        }


        public void LoadConfig() {
            string configContext = null;
            configContext = ReadSingleTXTFileInPersistentFolder(constData.DataSubFolderPathInPersistentFolderName, constData.PlayerConfigFileName);
            bool isSuccess = configManager.LoadConfigContext(configContext);

            if (isSuccess == false) { // Persistent文件夹内的文件不存在或者损坏
                configContext = ReadSingleTXTFileInBothReadonlyFolder(constData.DefaultDataSubFolderPathInReadonlyDataFolderName, constData.DefaultConfigFileName);
                isSuccess = configManager.LoadConfigContext(configContext);
                if (isSuccess == false) {
                    throw new System.Exception("Config文件读取失败!");
                }
                // 写入DefaultConfig到Persistent文件夹
                WriteSingleTXTFileInPersistentFolder(constData.DataSubFolderPathInPersistentFolderName, constData.PlayerConfigFileName, configContext);
            }

            language = configManager.Config.Language;
        }

        public void LoadPlayerRecord() {
            string playerRecordContext = null;
            playerRecordContext = ReadSingleTXTFileInPersistentFolder(constData.DataSubFolderPathInPersistentFolderName, constData.PlayerRecordFileName);
            bool isSuccess = playerRecordManager.LoadPlayerRecordContext(playerRecordContext); // 后续处理由PlayerRecordManager实现
        }

        public void LoadStoryRecords() {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            string folderPath = Application.persistentDataPath + "/" + constData.SaveDataSubFolderPathInPersistentFolderName;
            GetAllFilesOfFolderPath(folderPath, fileInfoList, new string[] { "txt" });
            bool isSuccess = playerRecordManager.LoadStoryRecordContext(fileInfoList); // 后续处理由PlayerRecordManager实现
        }

        public string LoadStoryRecordContext(string path) {
            return ReadTXTFile(path);
        }

        public void LoadVoiceAssetBundleConfig() {
            voiceAssetBundleConfigManager = new VoiceAssetBundleConfigManager();
            string folderPath = constData.VoiceAssetBundleConfigSubFolderPathInReadonly;
            string vabConfigContext = ReadSingleTXTFileInBothReadonlyFolder(folderPath, constData.VoiceAssetBundleConfigFileName);
            voiceAssetBundleConfigManager.LoadVoiceAssetBundleConfigContext(vabConfigContext);
            voiceAssetBundleResManager = new VoiceAssetBundleResManager<AssetBundle>(constData.VoiceAssetBundleBufferMaxCount);
            voiceAssetBundleResManager.LoadConfigManager(voiceAssetBundleConfigManager);
        }

        public void LoadResource() {
            // 
            resourceManager.LoadBgImage(GetBgImages());
        }

        public void SaveConfig(string configContext) {
            WriteSingleTXTFileInPersistentFolder(constData.DataSubFolderPathInPersistentFolderName, constData.PlayerConfigFileName, configContext);
        }

        public void SavePlayerRecord(string playerRecordContext) {
            WriteSingleTXTFileInPersistentFolder(constData.DataSubFolderPathInPersistentFolderName, constData.PlayerRecordFileName, playerRecordContext);
        }

        public void SaveStoryRecord(string storyRecordContext, int indexOfRecord) {
            string fileName = indexOfRecord == 0 ?
                constData.SaveDataQuickPrefixName + constData.SaveDataCommonPrefixName + indexOfRecord :
                constData.SaveDataCommonPrefixName + indexOfRecord;
            WriteSingleTXTFileInPersistentFolder(constData.SaveDataSubFolderPathInPersistentFolderName, fileName, storyRecordContext);
        }

        public AssetBundle GetAssetBundleByName(string name) {
            string folderName = Application.streamingAssetsPath + "/" + constData.AssetBundlePathInStreamingAssets;
            string assetbundleName = name + constData.AssetBundleVariant;
            string path = folderName + "/" + assetbundleName;
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            return ab;
        }





        private string ReadSingleTXTFileInPersistentFolder(string folderPath, string fileName) {
            string context = null;
            string folderPathOfStreamingAssets = Application.persistentDataPath + "/" + folderPath;
            context = ReadTXTFileInFolderPath(folderPathOfStreamingAssets, fileName);
            return context;
        }

        private string ReadSingleTXTFileInBothReadonlyFolder(string folderPath, string fileName) {
            string context = null;

            string subPathOfResources = folderPath + "/" + fileName.Substring(0, fileName.Length - 4);
            context = Resources.Load<TextAsset>(subPathOfResources)?.text as string;
            if (context != null)
                return context;

            string folderPathOfStreamingAssets = Application.streamingAssetsPath + "/" + folderPath;
            context = ReadTXTFileInFolderPath(folderPathOfStreamingAssets, fileName);

            return context;
        }

        private string ReadTXTFileInFolderPath(string folderPath, string fileName) {
            string path = folderPath + "/" + fileName;
            return ReadTXTFile(path);
        }

        private string ReadTXTFile(string path) {
            if (File.Exists(path)) {
                return File.ReadAllText(path, System.Text.Encoding.UTF8);
            } else {
                return null;
            }
        }

        private void GetAllFilesOfFolderPath(string folderPath, List<FileInfo> result, string[] fileFormatList) {
            if (!Directory.Exists(folderPath)) {
                return;
            }
            foreach (string subdir in Directory.GetDirectories(folderPath)) {
                GetAllFilesOfFolderPath(subdir, result, fileFormatList);
            }
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            for (int i = 0; i < fileFormatList.Length; i++) {
                string fileFormat = fileFormatList[i];
                FileInfo[] files = folder.GetFiles("*." + fileFormat);
                result.AddRange(files);
            }
        }

        private void WriteSingleTXTFileInPersistentFolder(string folderPath, string fileName, string contexts) {
            string folderPathOfStreamingAssets = Application.persistentDataPath + "/" + folderPath;
            WriteTextFileInFolderPath(folderPathOfStreamingAssets, fileName, contexts);
        }

        private void WriteTextFileInFolderPath(string folderPath, string fileName, string contexts) {
            string fullPath = folderPath + "/" + fileName;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(fullPath, contexts);
        }


        private List<FileInfo> GetBgImages() {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            string folderPathOfStreaming = Application.streamingAssetsPath + "/" + constData.BgImagesSubFolderPathInReadonlyDataFolderName;
            GetAllFilesOfFolderPath(folderPathOfStreaming, fileInfoList, new string[] { "png","jpg" });
            //string folderPathOfResources = Application.dataPath
            return fileInfoList;
        }


    }
}



/* 全平台异步读取StreamingAssets文件夹内的资源代码
void Awake()
    {
        string path =
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.streamingAssetsPath + "/Josn/modelname.json";
#elif UNITY_IPHONE && !UNITY_EDITOR
        "file://" + Application.streamingAssetsPath + "/Josn/modelname.json";
#elif UNITY_STANDLONE_WIN||UNITY_EDITOR
        "file://" + Application.streamingAssetsPath + "/Josn/modelname.json";
#else
        string.Empty;
#endif
        StartCoroutine(ReadData(path));
    }
 
    IEnumerator ReadData(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        while (www.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        string data = www.text;
        yield return new WaitForEndOfFrame();
    }
 */
