using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ResourceManager {
        #region ConstData
        private ConstData constData;
        private ConstData ConstData {
            get {
                if (constData == null)
                    constData = PachiGrimoire.I.constData;
                return constData;
            }
        }
        #endregion

        #region ResManagers
        private BgImageResManager<Texture2D> bgImageResManager;
        private BgImageResManager<Texture2D> BgImageResManager {
            get {
                if (bgImageResManager == null) {
                    bgImageResManager = new BgImageResManager<Texture2D>(ConstData.BgImageBufferMaxCount);
                }
                return bgImageResManager;
            }
        }

        #endregion

        public bool LoadBgImage(List<System.IO.FileInfo> fileInfoList) { // 规定保存记录的文件命名格式是固定的   
            return BgImageResManager.LoadFileInfoList(fileInfoList);
        }

        public TValue Get<TValue>(string index) where TValue : class {
            string subName;
            string typeName = GetTypeNameOfIndex(index, out subName);
            if (typeName.Equals(ConstData.ScriptIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetScript(subName) as TValue;
            } else if (typeName.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetBgImage(subName) as TValue;
            } else if (typeName.Equals(ConstData.FgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetFgImage(subName) as TValue; 
            } else if (typeName.Equals(ConstData.BGMIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetBGM(subName) as TValue;
            } else if (typeName.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetSound(subName) as TValue; 
            } else if (typeName.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetVoice(subName) as TValue; 
            } else if (typeName.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetVideo(subName) as TValue; 
            } else if (typeName.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetDialog(subName) as TValue; 
            }
            return null;
        }

        private string GetTypeNameOfIndex(string index, out string subName) {
            subName = null;
            if (index == null)
                return null;
            int _index = index.IndexOf('_');
            if (_index == -1)
                return null;
            string typeName = index.Substring(0, _index);
            subName = index.Substring(_index + 1);
            return typeName;
        }


        private string GetScript(string subName) {
            return subName;
            return null;
        }
        private Texture2D GetBgImage(string subName){
            return BgImageResManager.Get(subName);
        }
        private Texture2D GetFgImage(string subName) {
            return null;
        }
        private AudioClip GetBGM(string subName) {
            return null;
        }
        private AudioClip GetSound(string subName) {
            return null;
        }
        private AudioClip GetVoice(string subName) {
            return null;
        }
        private UnityEngine.Video.VideoClip GetVideo(string subName) {
            return null;
        }
        private string GetDialog(string subName) {
            return null;
        }
    }
}
