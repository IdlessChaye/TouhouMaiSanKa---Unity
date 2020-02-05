using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {

    [CreateAssetMenu(fileName = "ConstData", menuName = "AVGEngine/ConstData")]
    public class ConstData : ScriptableObject {
        public string AssetBundlePathInStreamingAssets = "AVGEngine/AssetBundle";
        public string AssetBundleVariant = ".unity3d";

        public string ScriptsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Scripts";
        public string ScriptAssetBundleName => "script" + AssetBundleVariant;
        public string ScriptIndexPrefix = "SC";
        public int ScriptBufferMaxCount = 3;
        public string MainScriptFileName = "Main.txt";

        public string BgImagesSubFolderPathInReadonlyDataFolderName = "AVGEngine/BgImages";
        public string BgImageAssetBundleName => "bgimage" + AssetBundleVariant;
        public int BgImageBufferMaxCount = 4;
        public string BgImageIndexPrefix = "BI";

        public string FgImagesSubFolderPathInReadonlyDataFolderName = "AVGEngine/FgImages";
        public string FgImageAssetBundleName => "fgimage" + AssetBundleVariant;
        public int FgImageBufferMaxCount = 20;
        public string FgImageIndexPrefix = "FI";

        public string BGMsSubFolderPathInReadonlyDataFolderName = "AVGEngine/BGMs";
        public string BGMAssetBundleName => "bgm" + AssetBundleVariant;
        public int BGMBufferMaxCount = 3;
        public string BGMIndexPrefix = "BM";

        public string SoundsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Sounds";
        public string SoundAssetBundleName => "sound" + AssetBundleVariant;
        public int SoundBufferMaxCount = 5;
        public string SoundIndexPrefix = "SD";

        public string VoicesSubFolderPathInReadonlyDataFolderName = "AVGEngine/Voices";
        public string VoicetAssetBundleName => "voiceChapterName" + AssetBundleVariant;
        public int VoiceBufferMaxCount = 100;
        public string VoiceIndexPrefix = "VI";
        public string VoiceAssetBundleConfigSubFolderPathInReadonly = "AVGEngine/DefaultData";
        public string VoiceAssetBundleConfigFileName = "VoiceAssetBundleConfig.txt";
        public int VoiceAssetBundleBufferMaxCount = 2;
      
        public string VideosSubFolderPathInReadonlyDataFolderName = "AVGEngine/Videos";
        public int VideoBufferMaxCount = 1;
        public string VideoIndexPrefix = "VD";

        public string DialogsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Dialogs";
        public string DialogAssetBundleName => "Languagedialog" + AssetBundleVariant;
        public int DialogBufferMaxCount = 3;
        public string DialogIndexPrefix = "DL";
        public string ChineseDialogFolderName = "Chinese";



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
    }
}