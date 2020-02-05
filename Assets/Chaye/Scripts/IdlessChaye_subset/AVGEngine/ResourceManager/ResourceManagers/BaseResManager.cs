using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class BaseResManager<TValue> where TValue : class {
        protected Dictionary<string, string> pathDict = new Dictionary<string, string>();
        public Dictionary<string, string> PathDict => pathDict;

        public abstract bool LoadFileInfoList(List<System.IO.FileInfo> fileInfoList);
        public abstract TValue Get(string subIndex);
    }

}