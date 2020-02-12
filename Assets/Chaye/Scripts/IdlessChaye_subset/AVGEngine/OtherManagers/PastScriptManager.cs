using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class PastScriptManager {
        public Dictionary<string, KeyValuePair<int, int>> PastScriptDict => pastScriptDict; // SaveData 用

        private Dictionary<string, KeyValuePair<int, int>> pastScriptDict = new Dictionary<string, KeyValuePair<int, int>>();


        public void UpdatePastScript(string scriptName, int endLineNumber, int startLineNumber = 0) {
            if (pastScriptDict.ContainsKey(scriptName)) {
                KeyValuePair<int, int> pair = new KeyValuePair<int, int>(startLineNumber, endLineNumber);
                pastScriptDict[scriptName] = pair;
            } else {
                pastScriptDict.Add(scriptName, new KeyValuePair<int, int>(startLineNumber, endLineNumber));
            }
        }

        public int GetStartLineNumber(string scriptName) => pastScriptDict[scriptName].Key;
        public int GetEndLineNumber(string scriptName) => pastScriptDict[scriptName].Value;


        public void LoadPlayerRecord(List<string> pastScriptNameList, List<KeyValuePair<int, int>> pastScriptRangeList) {
            if (pastScriptNameList != null) {
                pastScriptDict.Clear();
                for (int i = 0; i < pastScriptNameList.Count; i++) {
                    pastScriptDict.Add(pastScriptNameList[i], pastScriptRangeList[i]);
                }
            } else {
                pastScriptDict.Clear();
            }
        }
    }
}