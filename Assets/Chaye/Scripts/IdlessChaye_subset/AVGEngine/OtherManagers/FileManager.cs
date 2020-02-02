using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class FileManager {
        private ConfigManager configManager;

        private PlayerRecordManager playerRecordManager;

        private ResourceManager resourceManager;

        private ConstData constData;

        public FileManager(ConfigManager configManager, PlayerRecordManager playerRecordManager, ResourceManager resourceManager, ConstData constData) {
            this.configManager = configManager;
            this.playerRecordManager = playerRecordManager;
            this.resourceManager = resourceManager;
            this.constData = constData;
        }


        public void LoadConfig() {
            string configContext = null;
            configContext = ReadSingleTXTFileInPersistentFolder(constData.DefaultDataSubFolderPathInPersistentFolderName, constData.PlayerConfigFileName);
            bool isSuccess = configManager.CanProcessConfigContext(configContext);

            if (isSuccess == false) { // Persistent文件夹内的文件不存在或者损坏
                configContext = ReadSingleTXTFileInBothReadonlyFolder(constData.DefaultDataSubFolderPathInReadonlyDataFolderName, constData.DefaultConfigFileName);
                isSuccess = configManager.CanProcessConfigContext(configContext);
                if (isSuccess == false) {
                    throw new System.Exception("Config文件读取失败!");
                }
                // 写入DefaultConfig到Persistent文件夹
                WriteSingleTXTFileInPersistentFolder(constData.DefaultDataSubFolderPathInPersistentFolderName, constData.PlayerConfigFileName, configContext);
            }
        }

        public void LoadPlayerRecord() {

        }

        public void LoadResource() {

        }





        private string ReadSingleTXTFileInPersistentFolder(string folderPath, string fileName) {
            string context = null;
            string folderPathOfStreamingAssets = Application.persistentDataPath + "/" + folderPath;
            context = ReadTextFileInFolderPath(folderPathOfStreamingAssets, fileName);
            return context;
        }

        private string ReadSingleTXTFileInBothReadonlyFolder(string folderPath, string fileName) {
            string context = null;

            string subPathOfResources = folderPath + "/" + fileName.Substring(0, fileName.Length - 4);
            context = Resources.Load<TextAsset>(subPathOfResources)?.text as string;
            if (context != null)
                return context;

            string folderPathOfStreamingAssets = Application.streamingAssetsPath + "/" + folderPath;
            context = ReadTextFileInFolderPath(folderPathOfStreamingAssets, fileName);

            return context;
        }

        private string ReadTextFileInFolderPath(string folderPath, string fileName) {
            string path = folderPath + "/" + fileName;

            if (File.Exists(path)) {
                return File.ReadAllText(path, System.Text.Encoding.UTF8);
            } else {
                return null;
            }
        }

        private void WriteSingleTXTFileInPersistentFolder(string folderPath, string fileName, string contexts) {
            string folderPathOfStreamingAssets = Application.persistentDataPath + "/" + folderPath;
            WriteTextFileInFolderPath(folderPathOfStreamingAssets, fileName, contexts);
        }

        private void WriteTextFileInFolderPath(string folderPath, string fileName, string contexts) {
            string fullPath = folderPath + "/" + fileName;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllText(fullPath, contexts);
        }
    }
}