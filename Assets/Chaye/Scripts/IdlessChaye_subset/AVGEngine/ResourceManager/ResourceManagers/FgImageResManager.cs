using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class FgImageResManager<TValue> : BaseResManager<TValue> where TValue:class  {

        private FgImageResBufferIndexer<TValue> bufferIndexer;

        public FgImageResManager(int bufferMaxCount) {
            bufferIndexer = new FgImageResBufferIndexer<TValue>(bufferMaxCount);
        }

        public override TValue Get(string subIndex) {
            string key = subIndex;
            string finalIndex = subIndex;
            if (PathDict.ContainsKey(key)) {
                finalIndex = PathDict[key];
            }
            return bufferIndexer.Get(key, finalIndex);
        }

        public override bool LoadFileInfoList(List<FileInfo> fileInfoList) {
            for (int i = 0; i < fileInfoList.Count; i++) {
                FileInfo fileInfo = fileInfoList[i];
                string name = fileInfo.Name.Split('.')[0];
                string fullPath = fileInfo.FullName;
                Debug.Log($"LoadFIList,name:{name}, fullPath:{fullPath}");
                if (pathDict.ContainsKey(name)) {
                    pathDict[name] = fullPath;
                } else {
                    pathDict.Add(name, fullPath);
                }
            }
            return true;
        }

        public bool ForceAdd(string key, TValue value) {
            return bufferIndexer.ForceAdd(key, value);
        }
    }
}