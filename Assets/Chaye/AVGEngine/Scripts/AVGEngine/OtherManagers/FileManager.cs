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
        private AssetBundle scriptAssetBundle;
        private AssetBundle bgImageAssetBundle;
        private AssetBundle fgImageAssetBundle;
        private AssetBundle bgmAssetBundle;
        private AssetBundle soundAssetBundle;
        private AssetBundle videoAssetBundle;
        private AssetBundle dialogAssetBundle;

        private string language;




        public FileManager(ConfigManager configManager, PlayerRecordManager playerRecordManager, ResourceManager resourceManager, ConstData constData) {
            this.configManager = configManager;
            this.playerRecordManager = playerRecordManager;
            this.resourceManager = resourceManager;
            this.constData = constData;
        }



        #region Public Functions
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
        public string LoadDefaultConfig() {
            return ReadSingleTXTFileInBothReadonlyFolder(constData.DefaultDataSubFolderPathInReadonlyDataFolderName, constData.DefaultConfigFileName);
        }

        public void LoadPlayerRecord() {
            string playerRecordContext = null;
            playerRecordContext = ReadSingleTXTFileInPersistentFolder(constData.DataSubFolderPathInPersistentFolderName, constData.PlayerRecordFileName);
            bool isSuccess = playerRecordManager.LoadPlayerRecordData(playerRecordContext); // 后续处理由PlayerRecordManager实现
        }

        public void LoadStoryRecords() {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            string folderPath = Application.persistentDataPath + "/" + constData.SaveDataSubFolderPathInPersistentFolderName;
            GetAllFilesOfFolderPath(folderPath, fileInfoList, new string[] { "txt" });
            bool isSuccess = playerRecordManager.LoadStoryRecordData(fileInfoList); // 后续处理由PlayerRecordManager实现
        }

        public string LoadStoryRecordContext(string path) {
            return ReadTXTFile(path);
        }

        public void LoadResource() {
            LoadScripts();
            LoadBgImages();
            LoadFgImages();
            LoadBGMs();
            LoadSounds();
            LoadVoices();
            LoadVideos();
            LoadDialogs();
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
            fileName = fileName + ".txt";
            Debug.Log("SaveStoryRecord " + fileName);
            WriteSingleTXTFileInPersistentFolder(constData.SaveDataSubFolderPathInPersistentFolderName, fileName, storyRecordContext);
        }
#endregion


        #region Buffer Load Asset
        public AssetBundle GetAssetBundleByName(string name) {
            string folderName = Application.streamingAssetsPath + "/" + constData.AssetBundlePathInStreamingAssets;
            string path = folderName + "/" + name;
            AssetBundle ab = null;
            if (File.Exists(path)) {
                ab = AssetBundle.LoadFromFile(path);
            }
            return ab;
        }


        public string LoadScriptAsset(string assetName) {
            return scriptAssetBundle.LoadAsset<TextAsset>(assetName).text as string;
        }

        public Texture2D LoadBgImageAsset(string assetName) {
            return bgImageAssetBundle.LoadAsset<Texture2D>(assetName);
        }

        public Texture2D LoadFgImageAsset(string assetName) {
            return fgImageAssetBundle.LoadAsset<Texture2D>(assetName);
        }

        public AudioClip LoadBGMAsset(string assetName) {
            AudioClip clip = bgmAssetBundle.LoadAsset<AudioClip>(assetName); //加载音效文件
            if (clip.LoadAudioData()) {
                Debug.Log("音频已成功加载");
                return clip;
            } else {
                Debug.LogError("音效加载失败");
                return null;
            }
        }

        public AudioClip LoadSoundAsset(string assetName) {
            AudioClip clip = soundAssetBundle.LoadAsset<AudioClip>(assetName);
            if (clip.LoadAudioData()) {
                Debug.Log("音频已成功加载");
                return clip;
            } else {
                Debug.LogError("音效加载失败");
                return null;
            }
        }

        public AudioClip LoadVoiceAsset(string assetName) {
            int _index = assetName.IndexOf('_');
            string script = assetName.Substring(0, _index);
            string number = assetName.Substring(_index + 1);
            Debug.Log($"LoadVoiceAsset. scriptName: {script}, number: {number}");
            AssetBundle ab = voiceAssetBundleResManager.Get(script);
            AudioClip clip = ab?.LoadAsset<AudioClip>(assetName);
            if (clip != null && clip.LoadAudioData()) {
                Debug.Log("音频已成功加载");
                return clip;
            } else {
                Debug.LogError("音效加载失败");
                return null;
            }
        }

        public UnityEngine.Video.VideoClip LoadVideoAsset(string assetName) {
            return videoAssetBundle?.LoadAsset<UnityEngine.Video.VideoClip>(assetName);
        }

        public List<string> LoadDialogAsset(string assetName) {
            assetName = language + "_" + assetName;
            return TXT2List(dialogAssetBundle.LoadAsset<TextAsset>(assetName).text);
        }
#endregion


        #region Buffer LoadRes from FileSystem
        public string ReadTXTFile(string path) {
            if (File.Exists(path)) {
                return File.ReadAllText(path, System.Text.Encoding.UTF8);
            } else {
                return null;
            }
        }

        public List<string> ReadTXTFileAsList(string path) {
            string text = ReadTXTFile(path);
            return TXT2List(text);
        }

        public static Texture2D GetTexture2DFromPath(string imgPath) {
            //读取文件
            FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            int byteLength = (int)fs.Length;
            byte[] imgBytes = new byte[byteLength];
            fs.Read(imgBytes, 0, byteLength);
            fs.Close();
            fs.Dispose();
            fs = null;
            //转化为Texture2D
            Texture2D t2d = new Texture2D(ConstData.TEXTURE2D_WIDTH, ConstData.TEXTURE2D_HEIGHT);
            t2d.LoadImage(imgBytes);
            t2d.Apply();
            return t2d;
        }

        public AudioClip GetAudioClip(string path) {
            Debug.LogError("Not Completed!");
            return null;
        }

        public UnityEngine.Video.VideoClip GetVideoClip(string path) {
            Debug.LogError("Not Completed!");
            return null;
        }
#endregion


        #region Private Functions
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

        private List<string> TXT2List(string text) {
            text.Replace("\r\n", "\n");
            return new List<string>(text.Split('\n'));
        }


        private List<FileInfo> GetFileInfosInStreamingFolder(string subFolder, string[] types) {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            string folderPathOfStreaming = Application.streamingAssetsPath + "/" + subFolder;
            GetAllFilesOfFolderPath(folderPathOfStreaming, fileInfoList, types);
            return fileInfoList;
        }
        #endregion


        #region LoadRes First
        private void LoadScripts() {
            resourceManager.ScriptResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.ScriptsSubFolderPathInReadonlyDataFolderName, new string[] { "txt" }));

            scriptAssetBundle = GetAssetBundleByName(constData.ScriptAssetBundleName);
            if (scriptAssetBundle != null && constData.IsScriptABLoadedAllOnce) {
                string[] names = scriptAssetBundle.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) {
                    string name = names[i];
                    string key = Path.GetFileName(name).Split('.')[0];
                    string text = scriptAssetBundle.LoadAsset<TextAsset>(key).text as string;
                    resourceManager.ScriptResManager.ForceAdd(key, text);
                }
                scriptAssetBundle.Unload(false);
            }
        }

        private void LoadBgImages() {
            resourceManager.BgImageResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.BgImagesSubFolderPathInReadonlyDataFolderName, new string[] { "png", "jpg" }));

            bgImageAssetBundle = GetAssetBundleByName(constData.BgImageAssetBundleName);
            if (bgImageAssetBundle != null && constData.IsBgImageABLoadedAllOnce) {
                Debug.Log("IsBgImageABLoadedAllOnce");
                string[] names = bgImageAssetBundle.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) {
                    string name = names[i];
                    string key = Path.GetFileName(name).Split('.')[0];
                    Texture2D t = bgImageAssetBundle.LoadAsset<Texture2D>(key);
                    Debug.Log(t);
                    resourceManager.BgImageResManager.ForceAdd(key, t);
                }
                bgImageAssetBundle.Unload(false);
            }
        }

        private void LoadFgImages() {
            resourceManager.FgImageResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.FgImagesSubFolderPathInReadonlyDataFolderName, new string[] { "png", "jpg" }));

            fgImageAssetBundle = GetAssetBundleByName(constData.FgImageAssetBundleName);
            if (fgImageAssetBundle!=null && constData.IsFgImageABLoadedAllOnce) {
                string[] names = fgImageAssetBundle.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) {
                    string name = names[i];
                    string key = Path.GetFileName(name).Split('.')[0];
                    Texture2D t = fgImageAssetBundle.LoadAsset<Texture2D>(key);
                    resourceManager.FgImageResManager.ForceAdd(key, t);
                }
                fgImageAssetBundle.Unload(false);
            }
        }

        private void LoadBGMs() {
            resourceManager.BGMResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.BGMsSubFolderPathInReadonlyDataFolderName, new string[] { "mp3" }));

            bgmAssetBundle = GetAssetBundleByName(constData.BGMAssetBundleName);
            if (bgmAssetBundle!=null && constData.IsBGMABLoadedAllOnce) {
                string[] names = bgmAssetBundle.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) {
                    string name = names[i];
                    string key = Path.GetFileName(name).Split('.')[0];
                    AudioClip audio = bgmAssetBundle.LoadAsset<AudioClip>(key);
                    if (audio.LoadAudioData())
                        resourceManager.BGMResManager.ForceAdd(key, audio);
                    else
                        Debug.LogError($"AudioClip is not fully Loaded! name: {name}");
                }
                bgmAssetBundle.Unload(false);
            }
        }

        private void LoadSounds() {
            resourceManager.SoundResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.SoundsSubFolderPathInReadonlyDataFolderName, new string[] { "mp3" }));

            soundAssetBundle = GetAssetBundleByName(constData.SoundAssetBundleName);
            if (soundAssetBundle!= null && constData.IsSoundABLoadedAllOnce) {
                string[] names = soundAssetBundle.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) {
                    string name = names[i];
                    string key = Path.GetFileName(name).Split('.')[0];
                    AudioClip audio = soundAssetBundle.LoadAsset<AudioClip>(key);
                    if (audio.LoadAudioData())
                        resourceManager.SoundResManager.ForceAdd(key, audio);
                    else
                        Debug.LogError($"AudioClip is not fully Loaded! name: {name}");
                }
                soundAssetBundle.Unload(false);
            }
        }

        private void LoadVoices() {
            resourceManager.VoiceResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.VoicesSubFolderPathInReadonlyDataFolderName, new string[] { "mp3" }));

            voiceAssetBundleConfigManager = new VoiceAssetBundleConfigManager();
            string folderPath = constData.VoiceAssetBundleConfigSubFolderPathInReadonly;
            string vabConfigContext = ReadSingleTXTFileInBothReadonlyFolder(folderPath, constData.VoiceAssetBundleConfigFileName);
            voiceAssetBundleConfigManager.LoadVoiceAssetBundleConfigContext(vabConfigContext);
            voiceAssetBundleResManager = new VoiceAssetBundleResManager<AssetBundle>(constData.VoiceAssetBundleBufferMaxCount);
            voiceAssetBundleResManager.LoadConfigManager(voiceAssetBundleConfigManager);
        }

        private void LoadVideos() {
            resourceManager.VideoResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.VideosSubFolderPathInReadonlyDataFolderName, new string[] { "mp4" }));

            videoAssetBundle = GetAssetBundleByName(constData.VideoAssetBundleName);
        }

        private void LoadDialogs() {
            resourceManager.DialogResManager.LoadFileInfoList(GetFileInfosInStreamingFolder(constData.DialogsSubFolderPathInReadonlyDataFolderName, new string[] { "txt" }));

            string nameAssetBundle = constData.DialogAssetBundleSubName;
            if (language.Equals(constData.ChineseDialogName, System.StringComparison.OrdinalIgnoreCase)) {
                nameAssetBundle = constData.ChineseDialogName+ "_" + nameAssetBundle;
            }
            dialogAssetBundle = GetAssetBundleByName(nameAssetBundle);

            if (dialogAssetBundle != null &&  constData.IsDialogABLoadedAllOnce) {
                string[] names = dialogAssetBundle.GetAllAssetNames();
                for (int i = 0; i < names.Length; i++) {
                    string name = names[i];
                    name = Path.GetFileName(name).Split('.')[0];
                    string text = bgmAssetBundle.LoadAsset<TextAsset>(name).text;
                    List<string> list = TXT2List(text);
                    string key = name.Substring(name.IndexOf('_') + 1);
                    resourceManager.DialogResManager.ForceAdd(key, list);
                }
                dialogAssetBundle.Unload(false);
            }
        }
        #endregion

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
