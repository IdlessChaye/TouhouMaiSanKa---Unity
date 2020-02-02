//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace IdlessChaye.IdleToolkit.AVGEngine {
//    public class CommandEngineNode : BaseInterpreterNode {
//        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

//        public override void Interpret(ScriptSentenceContext context) {
//            context.SkipToken("Engine");
//            while (true) {
//                string funcToken = context.GetCurrentToken();
//                if (funcToken == null)
//                    break;

//                if (!CanParse(funcToken, context))
//                    break;
//            }
//        }

//        public override void Execute() {
//            foreach (BaseInterpreterNode node in nodeList)
//                node.Execute();
//        }

//        private bool CanParse(string token, ScriptSentenceContext scriptSentenceContext) {
//            bool canParse = true;
//            BaseInterpreterNode node = null;
//            if (token.Equals("ScriptReplace")) {
//                node = new EngineScriptReplaceNode();
//            } else {
//                canParse = false;
//            }

//            if (node != null) {
//                nodeList.Add(node);
//                node.Interpret(scriptSentenceContext);
//            }

//            return canParse;
//        }

//    }
//}