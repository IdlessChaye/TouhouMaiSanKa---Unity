using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class CommandEngineNode : BaseInterpreterNode {
        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Engine");
            while (true) {
                string funcToken = context.CurrentToken;
                if (funcToken == null)
                    break;

                if (!CanParse(funcToken, context))
                    break;
            }
        }

        public override void Execute() {
            for (int i = 0; i < nodeList.Count; i++) {
                BaseInterpreterNode node = nodeList[i];
                node.Execute();
            }
        }

        private bool CanParse(string token, ScriptSentenceContext scriptSentenceContext) {
            bool canParse = true;
            
            BaseInterpreterNode node = null;
            if (token.Equals("ScriptReplace")) {
                node = new EngineScriptReplaceAddNode();
            } else if (token.Equals("ScriptIfThenElse")) {
                node = new EngineScriptIfThenElseNode();
            } else if (token.Equals("ScriptLoadFile")) {
                node = new EngineScriptLoadFileNode();
            } else if (token.Equals("BacklogAdd")) {
                node = new EngineBacklogAddNode();
            } else if (token.Equals("SystemChangeStateToAnimate")) {
                node = new EngineSystemChangeStateToAnimate();
            } else if (token.Equals("SystemChangeStateToWait")) {
                node = new EngineSystemChangeStateToWait();
            } else if (token.Equals("MarkPlayerGet")) {
                node = new EngineMarkPlayerGetNode();
            } else if (token.Equals("MarkPlayerSet")) {
                node = new EngineMarkPlayerSetNode();
            } else if (token.Equals("MarkStoryGet")) {
                node = new EngineMarkStoryGetNode();
            } else if (token.Equals("MarkStorySet")) {
                node = new EngineMarkStorySetNode();
            } else if (token.Equals("ValuePlayerGet")) {
                node = new EngineValuePlayerGetNode();
            } else if (token.Equals("ValuePlayerSet")) {
                node = new EngineValuePlayerSetNode();
            } else if (token.Equals("ValueStoryGet")) {
                node = new EngineValueStoryGetNode();
            } else if (token.Equals("ValueStorySet")) {
                node = new EngineValueStorySetNode();
            } else if (token.Equals("ChoiceCreate")) {
                node = new EngineChoiceCreateNode();
            } else if (token.Equals("SystemDebugLog")) {
                node = new EngineSystemDebugLogNode();
            } else if (token.Equals("ChapterNameSet")) {
                node = new EngineChapterNameSetNode();
            } else {
                canParse = false;
            }

            if (canParse) {
                nodeList.Add(node);
                node.Interpret(scriptSentenceContext);
            } else {
                Debug.Log("CommandEngineNode FALSE canParse! token :" + token);
            }

            return canParse;
        }

    }
}