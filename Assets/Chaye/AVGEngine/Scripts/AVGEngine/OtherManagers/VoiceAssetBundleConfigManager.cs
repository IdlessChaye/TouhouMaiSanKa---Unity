using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class VoiceAssetBundleConfigManager {
        private Dictionary<string, string> scriptToChapterDict = new Dictionary<string, string>();
        private Dictionary<string, List<string>> chapterToScriptsDict = new Dictionary<string, List<string>>();

        public bool LoadVoiceAssetBundleConfigContext(string context) {
            if (context == null)
                return false;
            context.Replace("\r\n", "\n");
            context.Replace("  ", " ");
            string[] lines = context.Split('\n');
            for(int i = 0; i < lines.Length;i+= 2) {
                string chapter = lines[i];
                string scriptsLine = lines[i + 1];
                string[] scripts = scriptsLine.Split(' ');
                for(int j = 0; j < scripts.Length; j++) {
                    string script = scripts[j];
                    scriptToChapterDict.Add(script, chapter);
                }
                chapterToScriptsDict.Add(chapter, new List<string>(scripts));
            }
            return true;
        }

        public string GetChapter(string scriptName) {
            if(scriptName == null || scriptName.Equals(string.Empty)) {
                return string.Empty;
            }
            string chapter = null;
            if(scriptToChapterDict.ContainsKey(scriptName)) {
                chapter = scriptToChapterDict[scriptName];
            }
            return chapter;
        }
        

    }

}