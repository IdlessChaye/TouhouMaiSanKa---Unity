
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace IdlessChaye.IdleToolkit {
    public class CreateAssetbundles {
        [MenuItem("Assetbundles/BuildAssetBundles")]
        static void BuildAssetbundles() {
            string streamPath = Application.streamingAssetsPath;
            if (!Directory.Exists(streamPath)) {
                Directory.CreateDirectory(streamPath);
            }
            BuildPipeline.BuildAssetBundles(streamPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}
