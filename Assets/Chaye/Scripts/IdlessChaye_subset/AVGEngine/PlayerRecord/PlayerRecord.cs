using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    [Serializable]
    public class PlayerRecord {
        public List<string> markList = new List<string>();
        public List<string> varNameList = new List<string>();
        public List<int> varValueList = new List<int>();
    }
}
