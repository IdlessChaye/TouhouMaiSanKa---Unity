using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class PlayerRecordManager {
        public PlayerRecord PlayerRecord { get; private set; }
        public Dictionary<int, StoryRecord> StoryRecordDict { get; private set; } = new Dictionary<int, StoryRecord>();


        public void SavePlayerRecord(PlayerRecord newPlayerRecord) {
            if(newPlayerRecord == null) {
                throw new System.Exception("PlayerRecordManager SavePlayerRecord");
            }
            string json = JsonUtility.ToJson(newPlayerRecord);
            PlayerRecord = new PlayerRecord();
            JsonUtility.FromJsonOverwrite(json, PlayerRecord);
            PachiGrimoire.I.FileManager.SavePlayerRecord(json);
        }
        public void SaveStoryRecord(int indexOfRecord, StoryRecord newStoryRecord) {
            if(newStoryRecord == null) {
                throw new System.Exception("PlayerRecordManager SaveStoryRecord");
            }
            if (indexOfRecord >= 0 && indexOfRecord < PachiGrimoire.I.constData.SaveDataMaxCount) {
                string json = JsonUtility.ToJson(newStoryRecord);
                StoryRecord storyRecord = new StoryRecord();
                JsonUtility.FromJsonOverwrite(json, storyRecord);
                if(StoryRecordDict.ContainsKey(indexOfRecord)) {
                    StoryRecordDict[indexOfRecord] = storyRecord;
                } else {
                    StoryRecordDict.Add(indexOfRecord, storyRecord);
                }
                PachiGrimoire.I.FileManager.SaveStoryRecord(json, indexOfRecord);
            } else {
                throw new System.Exception("保存storyRecord时，index不得劲!" + indexOfRecord);
            }
        }

        public bool LoadPlayerRecordData(string playerRecordJSON) {
            Debug.Log("playerRecordJSON");
            Debug.Log(playerRecordJSON);
            PlayerRecord = new PlayerRecord();
            if (playerRecordJSON == null) {
                // 保存一个默认的玩家记录数据
                SavePlayerRecord(PlayerRecord);
                return false;
            }

            JsonUtility.FromJsonOverwrite(playerRecordJSON, PlayerRecord);
            return true;
        }

        public bool LoadStoryRecordData(List<FileInfo> fileInfoList) { // 规定保存记录的文件命名格式是固定的   
            if (fileInfoList == null || fileInfoList.Count == 0) {
                return false;
            }

            StoryRecordDict.Clear();
            StoryRecordDict = new Dictionary<int, StoryRecord>(PachiGrimoire.I.constData.SaveDataMaxCount);
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

    }
}
