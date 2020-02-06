using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class DialogResManager<TValue> : BaseResManager<TValue> where TValue : class {

        private DialogResBufferIndexer<TValue> bufferIndexer;

        public DialogResManager(int bufferMaxCount) {
            bufferIndexer = new DialogResBufferIndexer<TValue>(bufferMaxCount);
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
                //"Chinese_ScriptName.txt"
                string name = fileInfo.Name.Split('.')[0];
                string key = name.Substring(name.IndexOf('_') + 1);
                string fullPath = fileInfo.FullName;

                if (pathDict.ContainsKey(key)) {
                    pathDict[key] = fullPath;
                } else {
                    pathDict.Add(key, fullPath);
                }
            }
            return true;
        }

        public void ForceAdd(string key, TValue value) {
            bufferIndexer.ForceAdd(key, value);
        }
    }

}