using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class TextChangeNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Change");
            InterpretPart(context);
        }




        protected override void OnUpdateStageContext() {
            if (paraList.Count != 2 && paraList.Count != 1)
                throw new System.Exception("TextChangeNode");
            // 文本Index，角色名(可选)
            string scriptContextIndex = paraList[0];
            string characterName;

            if (paraList.Count == 1) {
                StageRenderManager.I.TextChange(scriptContextIndex);
            } else if (paraList.Count == 2) {
                characterName = paraList[1];
                StageRenderManager.I.TextChange(scriptContextIndex, characterName);
            } 
        }


    }
}