using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicVoiceStopNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("VoiceStop");
            InterpretPart(context);
        }



        protected override void OnUpdateEngineState() {

        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("MusicVoicePlayNode");
            PachiGrimoire.I.MusicManager.VoiceStop();
        }

        protected override void OnUpdateStageRender() {

        }

        protected override void OnLateUpdate() {

        }

    }
}