using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class FunNode : BaseInterpreterNode {
        protected List<string> paraList = new List<string>();
        public void InterpretPart(ScriptSentenceContext context) {
            context.SkipToken("(");
            while(true) {
                if (context.CurrentToken.Equals(")"))
                    break;
                string currentToken = context.CurrentToken;
                paraList.Add(currentToken);
            }
        }


        public override void Execute() {
            OnUpdateStageContext();
            OnUpdateStageRender();
            OnUpdateEngineState();
            OnLateUpdate();
        }

        protected abstract void OnUpdateEngineState();
        protected abstract void OnUpdateStageContext();
        protected abstract void OnUpdateStageRender();
        protected abstract void OnLateUpdate();
    }
}