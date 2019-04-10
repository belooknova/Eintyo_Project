using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulePurser
{
    public class Purser
    {
        //数式
        public string formula;
        private StatusData Attacker;
        private StatusData Receiver;

        private  bool ErrerCode = false;

        public Purser(StatusData A, StatusData B, string s)
        {
            this.formula = s;
            this.Attacker = A;
            this.Receiver = B;
        }

        public int Eval()
        {
            All_Remove_Char(); //スペースを削除
            formula.ToLower(); //小文字に変換

            Node root = new Node(); //最初のノードを生成する
            this.formula = Conversion_Status(this.formula); //ステータス文字列を変換

            Debug.Log(this.formula);
            Branch_Node(root); //分木を生成する

            //エラーコードフラグが立っている時はエラーを吐く
            int resultNum = Num_Convertor(root);

            if (ErrerCode)
            { return -1; }
            else
            { return resultNum; }
        }

        //数値に変換する
        int Num_Convertor(Node node)
        {
            List<string> num = new List<string>();
            List<char> vs = new List<char>();
            StrAnalysis(node.formula, out num, out vs);

            var numbers = new List<int>();
            int child = 0;
            for(int i=0; i < num.Count; i++)
            {
                int currntNum = 0;

                if (num[i] == "#")
                {
                    currntNum = Num_Convertor(node.childs[child++]);
                    
                }
                else
                {
                    //何かの手違いで数値以外が混じっていた場合の処理
                    try
                    { currntNum = int.Parse(num[i]); }
                    catch
                    { ErrerCode = true; }

                }
                numbers.Add(currntNum);

            }

            //積商計算
            for (int i = 0; i < vs.Count; i++)
            {
                switch (vs[i])
                {
                    case '*':
                        {
                            int left = numbers[i];
                            int right = numbers[i + 1];
                            numbers[i] = left * right;
                            numbers.RemoveAt(i + 1);
                            vs.RemoveAt(i);
                        }
                        break;
                    case '/':
                        {
                            int left = numbers[i];
                            int right = numbers[i + 1];

                            //０で割った時はエラー
                            try
                            { numbers[i] = left / right; }
                            catch
                            {
                                numbers[i] = 0;
                                ErrerCode = true;
                            }
                            numbers.RemoveAt(i + 1);
                            vs.RemoveAt(i); 
                        }
                        break;
                }

            }

            //加減算
            int total = numbers[0];
            for (int i=0; i < vs.Count; i++)
            {
                switch (vs[i])
                {
                    case '+':
                        {
                            total += numbers[i + 1];
                        }
                        break;
                    case '-':
                        {
                            total -= numbers[i + 1];
                        }
                        break;
                }
            }

            return total;
        }


        //数式を解析する
        void StrAnalysis(string s, out List<string> num, out List<char> vs)
        {
            num = new List<string>();
            vs = new List<char>();

            string text = "";
            for (int i=0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        {
                            num.Add(text);
                            vs.Add(s[i]);
                            text = "";
                        }
                        break;

                    default:
                        {
                            text += s[i];
                            if (i == s.Length - 1)
                            {
                                num.Add(text);
                                text = "";
                            }
                        }
                        break;
                }
            }
        }

        //空白を消去する
        private void All_Remove_Char()
        {
            string outStr = "";
            int strRange = formula.Length;

            for (int i=0; i<strRange; i++)
            {
                if (formula[i]!=' ')
                {
                    outStr += formula[i];
                }
            }
            formula = outStr;

        }

        //statusを数値に書き換える
        private string Conversion_Status(string s)
        {
            StatusData[] target = { Attacker, Receiver };
            string[] head = { "a.", "b." };

            for (int i = 0; i < target.Length; i++)
            {
                Dictionary<string, int> keyValues = target[i].GetDict();

                foreach (string key in keyValues.Keys)
                {
                    while (s.IndexOf(head[i]+key) != -1)
                    {
                        int index = s.IndexOf(head[i] + key);

                        s = s.Remove(index, (head[i] + key).Length);
                        s = s.Insert(index, keyValues[key].ToString());   
                    }
                }
            }
            return s;     
        }

       


        //カッコ内を別の文字に置き換えて分岐させる
        private int Branch_Node(Node target)
        {
            int range = this.formula.Length;
            for (int i=0; i < range; i++)
            {
                switch (this.formula[i])
                {
                    case '(':
                        {
                            target.formula += "#";

                            Node node = new Node();
                            target.Add(node);
                            target = node;

                        }
                        break;

                    case ')':
                        {
                            target = target.parent;
                        }
                        break;

                    default:
                        {
                            target.formula += this.formula[i];
                        }
                        break;
                }
            }
            return 0;
        }




    }

    public class Node
    {
        // 数式
        public string formula = "";

        // 子ノード
        public List<Node> childs = new List<Node>();

        // 親ノード
        public Node parent { get; private set; }

        public void Add(Node node)
        {
            node.parent = this;
            this.childs.Add(node);
        }

        public void Log()
        {
            Debug.Log(this.formula);
            foreach (var child in this.childs)
            {
                child.Log();
            }
        }
    }

}
