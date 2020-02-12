using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class PastScriptManager : IRecordable{
        public Dictionary<string, KeyValuePair<int, int>> PastScriptDict => pastScriptDict; // SaveData 用

        private Dictionary<string, KeyValuePair<int, int>> pastScriptDict = new Dictionary<string, KeyValuePair<int, int>>();

        public void LoadData(List<string> scriptNameList,List<KeyValuePair<int,int>> keyValuePairList) {
            if((scriptNameList== null && keyValuePairList == null) || scriptNameList?.Count == keyValuePairList?.Count) {
                if (scriptNameList == null)
                    return;
                for(int i = 0;i<scriptNameList.Count;i++) {
                    pastScriptDict.Add(scriptNameList[i], keyValuePairList[i]);
                }
            } else {
                throw new System.Exception("PastScriptManager LoadData");
            }
        }


        public void UpdatePastScript(string scriptName, int endLineNumber, int startLineNumber = 0) {
            if (pastScriptDict.ContainsKey(scriptName)) {
                KeyValuePair<int, int> pair = new KeyValuePair<int, int>(startLineNumber, endLineNumber);
                pastScriptDict[scriptName] = pair;
            } else {
                pastScriptDict.Add(scriptName, new KeyValuePair<int, int>(startLineNumber, endLineNumber));
            }
        }

        public int GetStartLineNumber(string scriptName) { return pastScriptDict[scriptName].Key; }
        public int GetEndLineNumber(string scriptName) { return pastScriptDict[scriptName].Value; }

        public void LoadPlayerData() {
            throw new System.NotImplementedException();
        }

        public void LoadStoryData() {
            throw new System.NotImplementedException();
        }
    }
}