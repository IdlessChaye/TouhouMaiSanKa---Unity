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
            string dialogContextIndex = paraList[0];
            string characterName;

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                if (paraList.Count == 1) {
                    StageRenderManager.I.TextChange(dialogContextIndex, null, false);
                } else if (paraList.Count == 2) {
                    characterName = paraList[1];
                    StageRenderManager.I.TextChange(dialogContextIndex, characterName, false);
                }
            } else {
                if (paraList.Count == 1) {
                    StageRenderManager.I.TextChange(dialogContextIndex);
                } else if (paraList.Count == 2) {
                    characterName = paraList[1];
                    StageRenderManager.I.TextChange(dialogContextIndex, characterName);
                }
            }
        }


    }
}