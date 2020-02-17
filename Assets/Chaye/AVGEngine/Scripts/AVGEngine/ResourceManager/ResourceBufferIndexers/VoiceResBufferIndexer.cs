using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class VoiceResBufferIndexer<TValue> : BaseResBufferIndexer<string, TValue> where TValue : class {
        private int destroyValueCount = 0;
        public VoiceResBufferIndexer(int bufferMaxCount) : base(bufferMaxCount) { }

        protected override void DestroyValue(TValue value) {
            if (destroyValueCount++ >= 20) {
                destroyValueCount = 0;
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
        }

        protected override TValue LoadValue(string finalIndex) {
            if (finalIndex.Contains("StreamingAssets"))
                return FileManager.GetAudioClip(finalIndex) as TValue;
            else
                return FileManager.LoadVoiceAsset(finalIndex) as TValue;
        }
    }
}
