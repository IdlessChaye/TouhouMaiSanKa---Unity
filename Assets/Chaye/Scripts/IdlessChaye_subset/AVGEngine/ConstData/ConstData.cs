using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {

    [CreateAssetMenu(fileName = "ConstData", menuName = "AVGEngine/ConstData")]
    public class ConstData : ScriptableObject {
        #region FileSystem
        public string AssetBundlePathInStreamingAssets = "AVGEngine/AssetBundle";
        public string AssetBundleVariant = ".unity3d";


        public string ScriptsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Scripts";
        public string ScriptAssetBundleName => "script" + AssetBundleVariant;
        public bool IsScriptABLoadedAllOnce = true;
        public string ScriptIndexPrefix = "SC";
        public int ScriptBufferMaxCount = 150;
        public string MainScriptFileName = "Main.txt";


        public string BgImagesSubFolderPathInReadonlyDataFolderName = "AVGEngine/BgImages";
        public string BgImageAssetBundleName => "bgimage" + AssetBundleVariant;
        public bool IsBgImageABLoadedAllOnce = false;
        public int BgImageBufferMaxCount = 10;
        public string BgImageIndexPrefix = "BI";


        public string FgImagesSubFolderPathInReadonlyDataFolderName = "AVGEngine/FgImages";
        public string FgImageAssetBundleName => "fgimage" + AssetBundleVariant;
        public bool IsFgImageABLoadedAllOnce = false;
        public int FgImageBufferMaxCount = 30;
        public string FgImageIndexPrefix = "FI";

        public string BGMsSubFolderPathInReadonlyDataFolderName = "AVGEngine/BGMs";
        public string BGMAssetBundleName => "bgm" + AssetBundleVariant;
        public bool IsBGMABLoadedAllOnce = false;
        public int BGMBufferMaxCount = 5;
        public string BGMIndexPrefix = "BM";


        public string SoundsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Sounds";
        public string SoundAssetBundleName => "sound" + AssetBundleVariant;
        public bool IsSoundABLoadedAllOnce = true;
        public int SoundBufferMaxCount = 15;
        public string SoundIndexPrefix = "SD";


        public string VoicesSubFolderPathInReadonlyDataFolderName = "AVGEngine/Voices";
        //public string VoicetAssetBundleName => "voiceChapterName" + AssetBundleVariant;
        public string VoicetAssetBundleSubName => "voice" + AssetBundleVariant;
        public int VoiceBufferMaxCount = 120;
        public string VoiceIndexPrefix = "VI";
        public string VoiceAssetBundleConfigSubFolderPathInReadonly = "AVGEngine/DefaultData";
        public string VoiceAssetBundleConfigFileName = "VoiceAssetBundleConfig.txt";
        public int VoiceAssetBundleBufferMaxCount = 2;
      

        public string VideosSubFolderPathInReadonlyDataFolderName = "AVGEngine/Videos";
        public string VideoAssetBundleName => "video" + AssetBundleVariant;
        public int VideoBufferMaxCount = 1;
        public string VideoIndexPrefix = "VD";


        public string DialogsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Dialogs";
        //public string DialogAssetBundleName => "languagedialog" + AssetBundleVariant;
        public string DialogAssetBundleSubName => "dialog" + AssetBundleVariant;
        public bool IsDialogABLoadedAllOnce = true;
        public int DialogBufferMaxCount = 3;
        public string DialogIndexPrefix = "DL";
        public string ChineseDialogName = "chinese";



        public string DefaultDataSubFolderPathInReadonlyDataFolderName = "AVGEngine/DefaultData";
        public string DefaultConfigFileName = "DefaultConfig.txt";

        public string DataSubFolderPathInPersistentFolderName = "AVGEngine/Data";
        public string PlayerConfigFileName = "Config.txt";
        public string PlayerRecordFileName = "PlayerRecord.txt";
        
        public string SaveDataSubFolderPathInPersistentFolderName = "AVGEngine/Data/SaveData";
        public string SaveDataCommonPrefixName = "SaveData";
        public string SaveDataQuickPrefixName = "Quick";
        public int SaveDataNormalMaxCount = 99;
        public int SaveDataQuickMaxCount = 1;
        public int SaveDataMaxCount => SaveDataNormalMaxCount + SaveDataQuickMaxCount;


        public static int TEXTURE2D_WIDTH = 1280;
        public static int TEXTURE2D_HEIGHT = 720;
        #endregion


        public KeyCode KeyConfirm = KeyCode.Z;

        public int BacklogCapacity = 100;

        public float MessageSpeedLowest = 4;
        public float MessageSpeedHighest = 30;
    }
}