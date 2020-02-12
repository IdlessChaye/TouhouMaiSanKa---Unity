using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class PlayerRecordManager {
        public PlayerRecord PlayerRecord { get; private set; } = new PlayerRecord();
        public Dictionary<int, StoryRecord> StoryRecordDict { get; private set; } = new Dictionary<int, StoryRecord>(100);

        public bool LoadPlayerRecordContext(string playerRecordJSON) {
            if (playerRecordJSON == null) {
                // 保存一个默认的玩家记录数据
                SavePlayerRecord(PlayerRecord);
                return false;
            }
            JsonUtility.FromJsonOverwrite(playerRecordJSON, PlayerRecord);
            return true;
        }

        public void SavePlayerRecord(PlayerRecord newPlayerRecord) {
            if (newPlayerRecord != PlayerRecord) {
                PlayerRecord = newPlayerRecord;
            }
            string configJSON = JsonUtility.ToJson(newPlayerRecord);
            PachiGrimoire.I.FileManager.SavePlayerRecord(configJSON);
        }

        public bool LoadStoryRecordContext(List<FileInfo> fileInfoList) { // 规定保存记录的文件命名格式是固定的   
            if (fileInfoList == null || fileInfoList.Count == 0) {
                return false;
            }

            StoryRecordDict.Clear();
            string saveDataCommonPrefixName = PachiGrimoire.I.constData.SaveDataCommonPrefixName;
            for (int i = 0; i < fileInfoList.Count; i++) {
                FileInfo fileInfo = fileInfoList[i];
                string fileName = fileInfo.Name;
                if (fileName.Contains(".txt") || fileName.Contains(".TXT")) {
                    fileName = fileName.Split('.')[0];
                }
                if (fileName.Contains(saveDataCommonPrefixName)) {
                    int indexOfSaveData = -1;
                    if (fileName.Contains(PachiGrimoire.I.constData.SaveDataQuickPrefixName)) {
                        indexOfSaveData = 0;
                    } else {
                        indexOfSaveData = int.Parse(fileName.Substring(saveDataCommonPrefixName.Length));
                    }
                    if (indexOfSaveData >= 0 && indexOfSaveData < PachiGrimoire.I.constData.SaveDataMaxCount) {
                        string filePath = fileInfo.FullName;
                        string json = PachiGrimoire.I.FileManager.LoadStoryRecordContext(filePath);

                        // Json可以Overwrite
                        StoryRecord storyRecord = new StoryRecord();
                        JsonUtility.FromJsonOverwrite(json, storyRecord);
                        StoryRecordDict.Add(indexOfSaveData, storyRecord);
                    }
                }
            }
            return true;
        }

        public void SaveStoryRecord(int indexOfRecord, StoryRecord newStoryRecord) {
            if (indexOfRecord >= 0 && indexOfRecord < PachiGrimoire.I.constData.SaveDataMaxCount) {
                if (StoryRecordDict.ContainsKey(indexOfRecord)) {
                    StoryRecordDict[indexOfRecord] = newStoryRecord;
                } else {
                    StoryRecordDict.Add(indexOfRecord, newStoryRecord);
                }
                string json = JsonUtility.ToJson(newStoryRecord);
                PachiGrimoire.I.FileManager.SaveStoryRecord(json, indexOfRecord);
            } else {
                throw new System.Exception("保存storyRecord时，index不得劲!" + indexOfRecord);
            }
        }
    }
}
