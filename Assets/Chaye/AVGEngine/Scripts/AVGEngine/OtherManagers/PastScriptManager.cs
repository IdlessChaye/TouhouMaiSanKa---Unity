using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class PastScriptManager {
        public Dictionary<string, int> PastScriptDict => pastScriptDict; // SaveData 用

        private Dictionary<string, int> pastScriptDict = new Dictionary<string,int>();


        public void UpdatePastScript(string scriptName, int endLineNumber) {
            if (pastScriptDict.ContainsKey(scriptName)) {
                int lastEndLineNumber = pastScriptDict[scriptName];
                if(lastEndLineNumber < endLineNumber) { 
                    pastScriptDict[scriptName] = endLineNumber;
                }
            } else {
                pastScriptDict.Add(scriptName, endLineNumber);
            }
        }

        public bool HasRead(string scriptName,int lineNumber) {
            bool hasRead = false;
            if(pastScriptDict.ContainsKey(scriptName)) {
                int endLineNumber = pastScriptDict[scriptName];
                if(lineNumber <= endLineNumber) {
                    hasRead = true;
                }
            }
            return hasRead;
        }

        public void LoadPlayerRecord(List<string> pastScriptNameList, List<int> pastScriptLineNumberList) {
            if (pastScriptNameList != null) {
                pastScriptDict.Clear();
                for (int i = 0; i < pastScriptNameList.Count; i++) {
                    pastScriptDict.Add(pastScriptNameList[i], pastScriptLineNumberList[i]);
                }
            } else {
                pastScriptDict.Clear();
            }
        }
    }
}