using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class VideoResBufferIndexer<TValue> : BaseResBufferIndexer<string, TValue> where TValue : class {
        public VideoResBufferIndexer(int bufferMaxCount): base(bufferMaxCount) { }

        protected override void DestroyValue(TValue value) {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        protected override TValue LoadValue(string finalIndex) {
            if (finalIndex.Contains("StreamingAssets"))
                return FileManager.GetVideoClip(finalIndex) as TValue;
            else
                return FileManager.LoadVideoAsset(finalIndex) as TValue;
        }
    }
}