using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    [Serializable]
    public class PlayerRecord {
        public List<string> markPlayerList = new List<string>();
        public List<string> varPlayerNameList = new List<string>();
        public List<float> varPlayerValueList = new List<float>();

        public List<string> pastScriptNameList;
        public List<int> pastScriptLineNumberList; // SaveData 用
    }
}
