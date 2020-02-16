using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageBackgroundImageChangeNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("BackgroundImageChange");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("ImageBackgroundImageChangeNode");
            string index = paraList[0];

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if(stateBuff == StateBuff.Next) {
                StageRenderManager.I.BackgroundImageChange(index, false);
            } else { 
                StageRenderManager.I.BackgroundImageChange(index);
            }

        }

    }
}