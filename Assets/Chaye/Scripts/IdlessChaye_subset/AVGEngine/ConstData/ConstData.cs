using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {

    [CreateAssetMenu(fileName = "ConstData", menuName = "AVGEngine/ConstData")]
    public class ConstData : ScriptableObject {
        public string ScriptsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Scripts";
        public string BgImagesSubFolderPathInReadonlyDataFolderName = "AVGEngine/BgImages";
        public string FgImagesSubFolderPathInReadonlyDataFolderName = "AVGEngine/FgImages";
        public string BGMsSubFolderPathInReadonlyDataFolderName = "AVGEngine/BGMs";
        public string SoundsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Sounds";
        public string VoicesSubFolderPathInReadonlyDataFolderName = "AVGEngine/Voices";
        public string DialogsSubFolderPathInReadonlyDataFolderName = "AVGEngine/Dialogs";
        public string DefaultDataSubFolderPathInReadonlyDataFolderName = "AVGEngine/DefaultData";
        public string VideosSubFolderPathInReadonlyDataFolderName = "AVGEngine/Videos";

        public string DefaultDataSubFolderPathInPersistentFolderName = "AVGEngine/DefaultData";
        public string PlayerConfigFileName = "Config.txt";

        public string DefaultConfigFileName = "DefaultConfig.txt";
        public string ResourcesFolderName = "Resources";
        public string MainScriptFileName = "Main.txt";
        public string ChineseDialogFolderName = "Chinese";
    }
}