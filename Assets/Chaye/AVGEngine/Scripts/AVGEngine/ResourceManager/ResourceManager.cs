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
        private ScriptResManager<string> scriptResManager;
        public ScriptResManager<string> ScriptResManager {
            get {
                if(scriptResManager == null) {
                    scriptResManager = new ScriptResManager<string>(ConstData.ScriptBufferMaxCount);
                }
                return scriptResManager;
            }
        }

        private BgImageResManager<Texture2D> bgImageResManager;
        public BgImageResManager<Texture2D> BgImageResManager {
            get {
                if (bgImageResManager == null) {
                    bgImageResManager = new BgImageResManager<Texture2D>(ConstData.BgImageBufferMaxCount);
                }
                return bgImageResManager;
            }
        }

        private FgImageResManager<Texture2D> fgImageResManager;
        public FgImageResManager<Texture2D> FgImageResManager {
            get {
                if(fgImageResManager == null) {
                    fgImageResManager = new FgImageResManager<Texture2D>(ConstData.FgImageBufferMaxCount);
                }
                return fgImageResManager;
            }
        }

        private BGMResManager<AudioClip> bgmResManager;
        public BGMResManager<AudioClip> BGMResManager {
            get {
                if(bgmResManager == null) {
                    bgmResManager = new BGMResManager<AudioClip>(ConstData.BGMBufferMaxCount);
                }
                return bgmResManager;
            }
        }

        private SoundResManager<AudioClip> soundResManager;
        public SoundResManager<AudioClip> SoundResManager {
            get {
                if (soundResManager == null) {
                    soundResManager = new SoundResManager<AudioClip>(ConstData.SoundBufferMaxCount);
                }
                return soundResManager;
            }
        }

        private VoiceResManager<AudioClip> voiceResManager;
        public VoiceResManager<AudioClip> VoiceResManager {
            get {
                if (voiceResManager == null) {
                    voiceResManager = new VoiceResManager<AudioClip>(ConstData.VoiceBufferMaxCount);
                }
                return voiceResManager;
            }
        }

        private VideoResManager<UnityEngine.Video.VideoClip> videoResManager;
        public VideoResManager<UnityEngine.Video.VideoClip> VideoResManager {
            get {
                if (videoResManager == null) {
                    videoResManager = new VideoResManager<UnityEngine.Video.VideoClip>(ConstData.VideoBufferMaxCount);
                }
                return videoResManager;
            }
        }

        private DialogResManager<List<string>> dialogResManager;
        public DialogResManager<List<string>> DialogResManager {
            get {
                if (dialogResManager == null) {
                    dialogResManager = new DialogResManager<List<string>>(ConstData.DialogBufferMaxCount);
                }
                return dialogResManager;
            }
        }
        #endregion


        public TValue Get<TValue>(string index) where TValue : class {
            if(string.IsNullOrEmpty(index)) {
                throw new System.Exception("ResourceManager Get :null index.");
            }
            string subName;
            string typeName = GetTypeNameOfIndex(index, out subName);
            TValue value = null;
            if (typeName.Equals(ConstData.ScriptIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetScript(subName) as TValue;
            } else if (typeName.Equals(ConstData.BgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetBgImage(subName) as TValue;
            } else if (typeName.Equals(ConstData.FgImageIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetFgImage(subName) as TValue; 
            } else if (typeName.Equals(ConstData.BGMIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetBGM(subName) as TValue;
            } else if (typeName.Equals(ConstData.SoundIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetSound(subName) as TValue; 
            } else if (typeName.Equals(ConstData.VoiceIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetVoice(subName) as TValue; 
            } else if (typeName.Equals(ConstData.VideoIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetVideo(subName) as TValue; 
            } else if (typeName.Equals(ConstData.DialogIndexPrefix, System.StringComparison.OrdinalIgnoreCase)) {
                value = GetDialog(subName) as TValue; 
            }
            if(value == null) {
                throw new System.Exception($"ResourceManager Get. index: {index}, subName: {subName}, typeName: {typeName}");
            }
            return value;
        }



        private string GetTypeNameOfIndex(string index, out string subName) {
            subName = null;
            if (string.IsNullOrEmpty(index))
                return null;
            int _index = index.IndexOf('_');
            if (_index == -1)
                return null;
            string typeName = index.Substring(0, _index);
            subName = index.Substring(_index + 1);
            return typeName;
        }

        private string GetScript(string subName) {
            return ScriptResManager.Get(subName);
        }
        private Texture2D GetBgImage(string subName){
            return BgImageResManager.Get(subName);
        }
        private Texture2D GetFgImage(string subName) {
            return FgImageResManager.Get(subName);
        }
        private AudioClip GetBGM(string subName) {
            return BGMResManager.Get(subName);
        }
        private AudioClip GetSound(string subName) {
            return SoundResManager.Get(subName); ;
        }
        private AudioClip GetVoice(string subName) {
            return VoiceResManager.Get(subName);
        }
        private UnityEngine.Video.VideoClip GetVideo(string subName) {
            return VideoResManager.Get(subName);
        }
        private string GetDialog(string subName) {
            //"ScriptName_0"
            int _index = subName.IndexOf('_');
            string subIndex = subName.Substring(0, _index);
            int number = int.Parse(subName.Substring(_index + 1));
            List<string> list  = DialogResManager.Get(subIndex);
            return list[number];
        }
    }
}
