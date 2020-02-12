using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineBacklogAddNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("BacklogAdd");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("EngineBacklogAddNode");
            string voiceIndex = PachiGrimoire.I.MusicManager.VoiceIndex;
            string contextIndex = StageRenderManager.I.DialogContextIndex;
            //string imageIndex = ;
            string name = StageRenderManager.I.CharacterName;
            PachiGrimoire.I.BacklogManager.Push(voiceIndex, contextIndex, null, name);
        }

    }
}