using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class BgImageResManager<TValue> where TValue : class {
        private BgImageResBufferIndexer<TValue> bufferIndexer;

        public BgImageResManager(int bufferMaxCount) {
            bufferIndexer = new BgImageResBufferIndexer<TValue>(bufferMaxCount);
        }

        public TValue Get(string subName) {
            string key = subName;
            string path = "???";
            return bufferIndexer.GetValue(key,path);
        }
    }
}