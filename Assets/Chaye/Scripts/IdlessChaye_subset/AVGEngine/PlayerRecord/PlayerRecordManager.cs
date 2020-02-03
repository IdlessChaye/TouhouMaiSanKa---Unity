using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class PlayerRecordManager {
        private PlayerRecord playerRecord = new PlayerRecord();
        private List<StoryRecord> storyRecordList = new List<StoryRecord>();
        public PlayerRecord PlayerRecord => playerRecord;
        public List<StoryRecord> StoryRecordList => storyRecordList;

        public bool LoadPlayerRecordContext(string playerRecordJSON) {
            if (playerRecordJSON == null) {
                // 保存一个默认的玩家记录数据
                SavePlayerRecord();
                return false;
            }
            JsonUtility.FromJsonOverwrite(playerRecordJSON, playerRecord);
            return true;
        }

        public void SavePlayerRecord() {
            string configJSON = JsonUtility.ToJson(playerRecord);
            PachiGrimoire.I.FileManager.SavePlayerRecord(configJSON);
        }

        public bool LoadStoryRecordContext(List<FileInfo> fileInfoList) { // 规定保存记录的文件命名格式是固定的   
            if (fileInfoList == null || fileInfoList.Count == 0) {
                return false;
            }
            string saveDataCommonPrefixName = PachiGrimoire.I.constData.SaveDataCommonPrefixName;
            for (int i = 0; i < fileInfoList.Count; i++) {
                FileInfo fileInfo = fileInfoList[i];
                string fileName = fileInfo.Name;
                if (fileName.Contains(saveDataCommonPrefixName)) {
                    int indexOfSaveData = int.Parse(fileName.Substring(saveDataCommonPrefixName.Length));
                    if (indexOfSaveData >= 0 && indexOfSaveData < PachiGrimoire.I.constData.SaveDataMaxCount) {
                        string filePath = fileInfo.FullName;
                        string json = PachiGrimoire.I.FileManager.LoadStoryRecordContext(filePath);

                        // Json可以Overwrite
                        if (storyRecordList[indexOfSaveData] == null)
                            storyRecordList[indexOfSaveData] = new StoryRecord();

                        JsonUtility.FromJsonOverwrite(json, storyRecordList[indexOfSaveData]);
                    }
                }
            }
            return true;
        }

        public void SaveStoryRecord(int indexOfRecord) {
            if (indexOfRecord >= 0 && indexOfRecord < PachiGrimoire.I.constData.SaveDataMaxCount) {
                StoryRecord storyRecord = storyRecordList[indexOfRecord];
                string json = JsonUtility.ToJson(storyRecord);
                PachiGrimoire.I.FileManager.SaveStoryRecord(json, indexOfRecord);
            } else {
                throw new System.Exception("保存storyRecord时，index不得劲!");
            }
        }
    }
}
