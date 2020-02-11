using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicBGMPlayNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("BGMPlay");
            InterpretPart(context);
        }



        protected override void OnUpdateEngineState() {

        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("MusicBGMPlayNode");
            string bgmIndex = paraList[0]; // "SC_ScriptName"
            AudioClip bgmClip = PachiGrimoire.I.ResourceManager.Get<AudioClip>(bgmIndex);
            PachiGrimoire.I.MusicManager.BGMPlay(bgmClip,bgmIndex);
        }

        protected override void OnUpdateStageRender() {

        }

        protected override void OnLateUpdate() {

        }

    }
}