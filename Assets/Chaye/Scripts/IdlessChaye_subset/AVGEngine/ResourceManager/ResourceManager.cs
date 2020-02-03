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
                    bgImageResManager = new BgImageResManager<Texture2D>(constData.BgImageBufferMaxCount);
                }
                return bgImageResManager;
            }
        }
        #endregion

        public Object Get(string index) {
            string subName;
            string typeName = GetTypeNameOfIndex(index, out subName);
            if (index.Equals(ConstData.ScriptIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetScript(subName);
            } else if (index.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetBgImage(subName);
            } else if (index.Equals(ConstData.FgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetFgImage(subName); 
            } else if (index.Equals(ConstData.BGMIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetBGM(subName);
            } else if (index.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetSound(subName); 
            } else if (index.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetVoice(subName); 
            } else if (index.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetVideo(subName); 
            } else if (index.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                return GetDialog(subName); 
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


        private String GetScript(string subName) {
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
        private String GetDialog(string subName) {
            return null;
        }
    }

    public class String : Object{
        public string str { get; set; }
    }
}
