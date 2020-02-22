using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ScriptManager {
        private PastScriptManager pastScriptManager;

        public string ScriptPointerScriptName { get; private set; }
        public int ScriptPointerLineNumber { get; private set; }
        public List<string> ScriptReplaceKeys { get; private set; }
        public List<string> ScriptReplaceValues { get; private set; }
        public Stack<string> PointerScriptNameStack => pointerScriptNameStack;
        public Stack<int> PointerLineNumberStack => pointerLineNumberStack;


        private Stack<char> charStack = new Stack<char>();
        private bool isSecondGear = false;

        private List<ScriptSentenceContext> scriptSentenceList;
        private Stack<string> pointerScriptNameStack;
        private Stack<int> pointerLineNumberStack;

        private List<ScriptSentenceContext> secondScriptSentenceList;
        private int secondScriptPointerLineNumber; // 第二档的脚本无名
        private bool isAllTrue;
        public bool IsAllTrue {
            get { return isAllTrue; }
            set { isAllTrue = isAllTrue && value; }
        }

        public ScriptManager(PastScriptManager pastScriptManager) {
            this.pastScriptManager = pastScriptManager;
        }



        public bool NextSentence() {
            if (isSecondGear)
                return NextSecondSentence();
            else
                return NextFirstSentence();
        }

        public void LoadScriptFile(string scriptName, string scriptContext) {
            if (string.IsNullOrEmpty(scriptContext) || string.IsNullOrEmpty(scriptName))
                throw new System.Exception("LoadScriptFile");
            Debug.Log("加载脚本 :" + scriptName);
            isSecondGear = false;
            if (!IsOver()) { // 保存主翻译器函数返回地址
                pointerScriptNameStack.Push(ScriptPointerScriptName);
                pointerLineNumberStack.Push(ScriptPointerLineNumber);
            }
            scriptSentenceList = ProcessScriptContext(scriptContext);
            foreach(var s in scriptSentenceList) {
                s.ShowSelfAll();
            }
            ScriptPointerScriptName = scriptName;
            ScriptPointerLineNumber = 0;
        }

        public void LoadScriptContext(string scriptContext) {
            isSecondGear = true;
            secondScriptSentenceList = ProcessScriptContext(scriptContext);
            secondScriptPointerLineNumber = 0;
        }

        public void UnloadSecondSentence() {
            secondScriptSentenceList = null;
            secondScriptPointerLineNumber = -1;
            isSecondGear = false;
        }


        public void ScriptIfThenElse(string ifStr, string thenStr, string elseStr) {
            isAllTrue = true;
            LoadScriptContext(ifStr);
            while (isAllTrue && NextSentence()) ; // isAllTrue会在Execute中改变
            UnloadSecondSentence();
            if (isAllTrue) {
                LoadScriptContext(thenStr);
            } else {
                LoadScriptContext(elseStr);
            }
            while (NextSentence()) ;
        }

        public void ScriptReplaceAdd(string key, string value) { // 后来居上
            ScriptReplaceKeys.Add(key);
            ScriptReplaceValues.Add(value);
        }




        private List<ScriptSentenceContext> ProcessScriptContext(string scriptContext) {
            if (scriptContext == null || scriptContext.Length == 0) {
                throw new System.Exception("传了个啥?");
            }
            Debug.Log("Process Script Context.");

            List<ScriptSentenceContext> sentenceList = new List<ScriptSentenceContext>();

            // 1. "\r\n" -> "\n"
            scriptContext.Replace("\r\n", "\n");
            scriptContext.Replace("\r", "");
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
                for (int j = ScriptReplaceKeys.Count - 1; j >= 0; j--) {
                    string key = ScriptReplaceKeys[j];
                    if (str.Contains(key)) {
                        string value = ScriptReplaceValues[j];
                        str = str.Replace(key, value);
                    }
                }
                // 4. 空格杠二消除
                str = str.Replace(" ", "");
                str = str.Replace("\r", "");
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
                        case '\r':
                            throw new System.Exception("有内鬼！");
                        case '/': // 实现注释功能
                            length = j - lastTailIndex - 1;
                            int strLength = str.Length;
                            if(length == 0 && j+1 < strLength && str[j+1] == '/') { // 第一个就是/后边又接了一个/
                                // 到\n都扔
                                int findHuicheIndex = j + 2;
                                while(findHuicheIndex < strLength && str[findHuicheIndex] != '\n') {
                                    findHuicheIndex++;
                                }
                                if(findHuicheIndex == strLength) {
                                    throw new System.Exception("有注释，但没找到回车！实现注释功能");
                                } else { // 跳过注释所在行
                                    Debug.Log("注释跳了！ 内容 :" + str.Substring(j, findHuicheIndex - j + 1));
                                    lastTailIndex = findHuicheIndex; 
                                    j = findHuicheIndex;
                                }
                            }
                            break;
                        case '\n':
                            length = j - lastTailIndex - 1;
                            if (length != 0) {
                                Debug.LogWarning("Error, str:" + str);
                                Debug.LogWarning("Error, fragmentList:");
                                foreach (var s in fragmentList)
                                    Debug.Log(s);
                                Debug.LogWarning("ErrorEnd, fragmentList.");
                                throw new System.Exception("语法出错,\n前一定是),但事实不是!");

                            }
                            lastTailIndex = j;
                            ScriptSentenceContext scriptSentenceContext = new ScriptSentenceContext(fragmentList.ToArray());
                            if (scriptSentenceContext.IsCorrect == false) { // 解析错误防止功能
                                throw new System.Exception("ScriptSentence is not right!");
                            }
                            string currentToken = scriptSentenceContext.CurrentToken;
                            if (currentToken != null && !currentToken.Substring(0, 2).Equals("//")) { // 跳过空行功能以及跳过注释辅助功能
                                sentenceList.Add(scriptSentenceContext);
                            } else {
                                Debug.Log("放弃ScriptSentenceContext currentToken :" + currentToken);
                            }
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
            if (fragmentList.Count != 0) {
                foreach (string s in fragmentList)
                    Debug.LogWarning(s);
                throw new System.Exception("怎么还有剩余的?");
            }

            return sentenceList;
        }

        private bool IsOver() {
            if (scriptSentenceList == null)
                return true;
            int length = scriptSentenceList.Count;
            if (ScriptPointerLineNumber < length)
                return false;
            else
                return true;
        }

        private bool IsSecondOver() {
            if (secondScriptSentenceList == null)
                return true;
            int length = secondScriptSentenceList.Count;
            if (secondScriptPointerLineNumber < length)
                return false;
            else
                return true;
        }

        private bool NextFirstSentence() {
            if (IsOver()) {
                if (pointerScriptNameStack.Count == 0) {
                    return false;
                } else {
                    // 恢复脚本上下文
                    ScriptPointerScriptName = pointerScriptNameStack.Pop();
                    ScriptPointerLineNumber = pointerLineNumberStack.Pop();
                    string scriptIndex = PachiGrimoire.I.constData.ScriptIndexPrefix + "_" + ScriptPointerScriptName;
                    string scriptContext = PachiGrimoire.I.ResourceManager.Get<string>(scriptIndex);
                    scriptSentenceList = ProcessScriptContext(scriptContext);
                }
            }
            // 得到当前语句
            ScriptSentenceContext context = scriptSentenceList[ScriptPointerLineNumber];
            // 更新已读文本
            pastScriptManager.UpdatePastScript(ScriptPointerScriptName, ScriptPointerLineNumber);
            // 更新指针
            ScriptPointerLineNumber = ScriptPointerLineNumber + 1;
            // 开始翻译
            ExpressionRootNode rootNode = new ExpressionRootNode();
            Debug.Log("主翻译器 开始翻译 :");
            context.ShowSelfAll();
            Debug.Log("主翻译器 开始翻译结束");
            rootNode.Interpret(context);
            Debug.Log("主翻译器 开始执行");
            rootNode.Execute();
            Debug.Log("主翻译器 开始执行结束");
            return true;
        }

        private bool NextSecondSentence() {
            if (IsSecondOver()) {
                UnloadSecondSentence();
                return false;
            }
            ScriptSentenceContext context = secondScriptSentenceList[secondScriptPointerLineNumber];
            secondScriptPointerLineNumber = secondScriptPointerLineNumber + 1;
            ExpressionRootNode rootNode = new ExpressionRootNode();
            Debug.Log("从翻译器 开始翻译 :");
            context.ShowSelfAll();
            Debug.Log("从翻译器 开始翻译结束");
            rootNode.Interpret(context);
            Debug.Log("从翻译器 开始执行");
            rootNode.Execute();
            Debug.Log("从翻译器 开始执行结束");
            return true;
        }


        public void KILLERQUEEN() {
            if (ScriptPointerLineNumber <= 0)
                return;
            ScriptPointerLineNumber--;
            NextFirstSentence();
        }


        public void LoadStoryRecord(string name, int number,
            List<string> keys, List<string> values,
            List<string> nameStack, List<int> numberStack) {
            ScriptPointerScriptName = name;
            string scriptIndex = PachiGrimoire.I.constData.ScriptIndexPrefix + "_" + name;
            string scriptContext = PachiGrimoire.I.ResourceManager.Get<string>(scriptIndex);
            scriptSentenceList = ProcessScriptContext(scriptContext);
            ScriptPointerLineNumber = number;
            if (keys != null)
                ScriptReplaceKeys = new List<string>(keys);
            else
                ScriptReplaceKeys.Clear();
            if (values != null)
                ScriptReplaceValues = new List<string>(values);
            else
                ScriptReplaceValues.Clear();
            if (nameStack != null)
                pointerScriptNameStack = new Stack<string>(nameStack);
            else
                pointerScriptNameStack.Clear();
            if (numberStack != null)
                pointerLineNumberStack = new Stack<int>(numberStack);
            else
                pointerLineNumberStack.Clear();

        }

        public void InitializeStory() {
            ScriptReplaceKeys = new List<string>();
            ScriptReplaceValues = new List<string>();
            pointerScriptNameStack = new Stack<string>();
            pointerLineNumberStack = new Stack<int>();
            isSecondGear = false;
            secondScriptSentenceList = null;
            secondScriptPointerLineNumber = -1;

            ConstData constData = PachiGrimoire.I.constData;
            string initScriptContext = PachiGrimoire.I.ResourceManager.Get<string>(constData.ScriptIndexPrefix + "_" + constData.InitScriptFileNameWithoutTXT);
            Debug.Log(initScriptContext);
            LoadScriptFile(constData.InitScriptFileNameWithoutTXT, initScriptContext);
            while (NextSentence()) ;
            Debug.Log("执行完毕InitScript in InitializeStory()");
        }

        public void FinalizeStory() {
            ScriptPointerScriptName = null;
            ScriptPointerLineNumber = -1;
            ScriptReplaceKeys = null;
            ScriptReplaceValues = null;
            scriptSentenceList = null;
            pointerScriptNameStack = null;
            pointerLineNumberStack = null;
            
            isSecondGear = false;
            secondScriptSentenceList = null;
            secondScriptPointerLineNumber = -1;
        }
    }

}