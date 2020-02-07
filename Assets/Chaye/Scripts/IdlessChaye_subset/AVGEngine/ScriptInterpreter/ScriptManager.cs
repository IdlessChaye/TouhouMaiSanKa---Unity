using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ScriptManager {
        private ResourceManager resourceManager;
        private StageContextManager stageContextManager;

        private string scriptPointerScriptName { set { stageContextManager.scriptPointerScriptName = value; } }
        private int scriptPointerLineNumber { set { stageContextManager.scriptPointerLineNumber = value; } }
        private List<string> scriptReplaceKeys { get { return stageContextManager.scriptReplaceKeys; } }
        private List<string> scriptReplaceValues { get { return stageContextManager.scriptReplaceValues; } }


        private List<ScriptSentenceContext> scriptSentenceList;
        private Stack<char> charStack = new Stack<char>();

        public ScriptManager(ResourceManager resourceManager, StageContextManager stageContextManager) {
            this.resourceManager = resourceManager;
            this.stageContextManager = stageContextManager;
        }


        public void LoadScriptFile(string scriptIndex) {
            string scriptContext = resourceManager.Get<string>(scriptIndex);
            if (scriptContext == null) {
                Debug.LogWarning($"Can't find script! scriptIndex: {scriptIndex}");
            }
            string scriptName = scriptIndex.Substring(scriptIndex.IndexOf('_') + 1);
            Debug.Log($"LoadScriptFile scriptName: {scriptName}");
            scriptPointerScriptName = scriptName;
            ProcessScriptContext(scriptContext);


        }

        public void LoadScriptContext(string scriptContext) {
            ProcessScriptContext(scriptContext);


        }

        public void ScriptReplaceAdd(string key, string value) {
            stageContextManager.scriptReplaceKeys.Add(key);
            stageContextManager.scriptReplaceValues.Add(value);
        }

        private void ProcessScriptContext(string scriptContext) {
            if (scriptContext == null || scriptContext.Length == 0) {
                throw new System.Exception("传了个啥?");
            }

            List<ScriptSentenceContext> sentenceList = new List<ScriptSentenceContext>();

            // 1. "\r\n" -> "\n"
            scriptContext.Replace("\r\n", "\n");
            int lastIndex = scriptContext.Length - 1; // 给最后补上'\n'
            if (scriptContext[lastIndex] != '\n') {
                scriptContext = scriptContext + "\n";
            }

            // 2. <>和**的绝对存在
            charStack.Clear();
            List<string> tempList = new List<string>(); // 宅出去<>**剩下的字符串
            List<bool> tempIsCompleteList = new List<bool>(); // 标记<>**字符串
            int lastTailIndex = -1;
            int leftIndex = -1;
            int i;
            for (i = 0; i < scriptContext.Length; i++) {
                char ch = scriptContext[i];
                switch (ch) {
                    case '<':
                        if (charStack.Count == 0) {
                            charStack.Push('<');
                            leftIndex = i;
                            if (i != lastTailIndex + 1) {
                                string str = scriptContext.Substring(lastTailIndex + 1, i - lastTailIndex - 1);
                                tempList.Add(str);
                                tempIsCompleteList.Add(false);
                            }
                        } else if (charStack.Peek() == '<') {
                            charStack.Push('<');
                        } else if (charStack.Peek() == '*') {
                            // pass;
                        } else {
                            throw new System.Exception($"语法有问题,Stack在<前遇到了 {charStack.Peek()}");
                        }
                        break;
                    case '>':
                        if (charStack.Count == 0) {
                            throw new System.Exception("语法有问题,遇到了 >");
                        } else if (charStack.Peek() == '<') {
                            charStack.Pop();
                            if (charStack.Count == 0) { // <>收束
                                lastTailIndex = i;
                                string str = scriptContext.Substring(leftIndex + 1, i - leftIndex - 1);
                                tempList.Add(str);
                                tempIsCompleteList.Add(true);
                            }
                        } else if (charStack.Peek() == '*') {
                            // pass
                        } else {
                            throw new System.Exception($"语法有问题,Stack在>前遇到了 {charStack.Peek()}");
                        }
                        break;
                    case '*':
                        if (charStack.Count == 0) {
                            charStack.Push('*');
                            leftIndex = i;
                            if (i != lastTailIndex + 1) {
                                string str = scriptContext.Substring(lastTailIndex + 1, i - lastTailIndex - 1);
                                tempList.Add(str);
                                tempIsCompleteList.Add(false);
                            }
                        } else if (charStack.Peek() == '*') {
                            if (charStack.Count != 1) {
                                throw new System.Exception($"语法有问题,好多*");
                            }
                            // **收束
                            charStack.Pop();
                            lastTailIndex = i;
                            string str = scriptContext.Substring(leftIndex + 1, i - leftIndex - 1);
                            tempList.Add(str);
                            tempIsCompleteList.Add(true);
                        } else if (charStack.Peek() == '<') {
                            // pass
                        } else {
                            throw new System.Exception($"语法有问题,Stack在*前遇到了 {charStack.Peek()}");
                        }
                        break;
                    default:
                        break;
                }
            }
            if (charStack.Count != 0) {
                throw new System.Exception("Stack isn't clear");
            }
            int lastStrLength = i - lastTailIndex - 1; // 把最后一段字符串加上
            if (lastStrLength > 0) {
                tempList.Add(scriptContext.Substring(lastTailIndex + 1, lastStrLength));
                tempIsCompleteList.Add(false);
            }

            // 3. ScriptReplace宏替换
            for (i = 0; i < tempList.Count; i++) {
                bool isComplete = tempIsCompleteList[i];
                if (isComplete) {
                    continue;
                }
                // 宏替换
                string str = tempList[i];
                for (int j = scriptReplaceKeys.Count - 1; j >= 0; j--) {
                    string key = scriptReplaceKeys[j];
                    if (str.Contains(key)) {
                        string value = scriptReplaceValues[j];
                        str = str.Replace(key, value);
                    }
                }
                // 4. 空格消除
                str = str.Replace(" ", "");
                tempList[i] = str;
            }

            // 5. '\n'脚本割解
            // 6. 全分割,得到ScriptSentenceContext
            List<string> fragmentList = new List<string>();
            for (i = 0; i < tempList.Count; i++) {
                string str = tempList[i];
                bool isComplete = tempIsCompleteList[i];
                if (isComplete) {
                    fragmentList.Add(str);
                    continue;
                }
                // 处理str,并在其中找'\n'
                int j;
                lastTailIndex = -1;
                int length = -1;
                string smallStr;
                for (j = 0; j < str.Length; j++) {
                    char ch = str[j];
                    switch (ch) {
                        case '\n':
                            length = j - lastTailIndex - 1;
                            if (length != 0) {
                                throw new System.Exception("语法出错,\n前一定是),但事实不是!");
                            }
                            lastTailIndex = j;
                            ScriptSentenceContext scriptSentenceContext = new ScriptSentenceContext(fragmentList.ToArray());
                            sentenceList.Add(scriptSentenceContext);
                            fragmentList.Clear();
                            break;
                        case '_':
                            length = j - lastTailIndex - 1;
                            if (length > 0) {
                                smallStr = str.Substring(lastTailIndex + 1, length);
                                fragmentList.Add(smallStr);
                            }
                            lastTailIndex = j;
                            break;
                        case '(':
                            length = j - lastTailIndex - 1;
                            if (length > 0) {
                                smallStr = str.Substring(lastTailIndex + 1, length);
                                fragmentList.Add(smallStr);
                            }
                            fragmentList.Add("(");
                            lastTailIndex = j;
                            break;
                        case ')':
                            length = j - lastTailIndex - 1;
                            if (length > 0) {
                                smallStr = str.Substring(lastTailIndex + 1, length);
                                fragmentList.Add(smallStr);
                            }
                            fragmentList.Add(")");
                            lastTailIndex = j;
                            break;
                        case ',':
                            length = j - lastTailIndex - 1;
                            if (length > 0) {
                                smallStr = str.Substring(lastTailIndex + 1, length);
                                fragmentList.Add(smallStr);
                            }
                            lastTailIndex = j;
                            break;
                        default:
                            break;
                    }
                }

            }
            if(fragmentList.Count!=0) {
                foreach (string s in fragmentList)
                    Debug.LogWarning(s);
                throw new System.Exception("怎么还有剩余的?");
            }

            this.scriptSentenceList = sentenceList;
        }
    }

}