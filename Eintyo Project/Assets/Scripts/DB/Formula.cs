using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Formula))]
public class FormulaInspector : Editor
{
    // Unity インスペクター拡張
    public override void OnInspectorGUI()
    {
        // ボタンを押したら数式パーサー実行
        if (GUILayout.Button("Parse", GUILayout.Width(100)))
        {
            Formula g = target as Formula;
            Eval(g.formula);
        }
        base.OnInspectorGUI();
    }

    // 数式を計算
    private static void Eval(string formula)
    {
        List<char> list = new List<char>(formula);

        // 空白全消去
        list.RemoveAll(x => x == ' ');

        char[] c = list.ToArray();

        // 構文解析
        Node node = Parse(c);

        // 計算
        double result = Eval(node);

        Debug.Log(formula + " = " + result);

        // 木構造を表示
        FormulaNodeWindow.RootNode = node;
        EditorWindow.GetWindow<FormulaNodeWindow>();
    }

    // cが数かどうかの判定
    private static bool IsNumber(char c)
    {
        return char.IsDigit(c) || c == 'x' || c == 'X' || c == '#';
    }

    // 数式を計算
    private static double Eval(Node node)
    {
        List<string> ns; // 数
        List<char> os; // 演算子

        // 字句解析
        LexicalAnalysis(node.formula, out ns, out os);

        // nsを元に数字を決定
        var numbers = new List<double>();
        {
            int child = 0;
            for (int i = 0; i < ns.Count; i++)
            {
                double num = 0.0;

                switch (ns[i])
                {
                    case "#":
                        num = Eval(node.childs[child++]);
                        break;
                    default:
                        double.TryParse(ns[i], out num);
                        break;
                }
                numbers.Add(num);
            }
        }

        // かけ算・わり算を行なう
        {
            for (int i = 0; i < os.Count;)
            {
                switch (os[i])
                {
                    case '*':
                        {
                            double left = numbers[i];
                            double right = numbers[i + 1];
                            numbers[i] = left * right;
                            numbers.RemoveAt(i + 1);
                            os.RemoveAt(i);
                        }
                        break;
                    case '/':
                        {
                            double left = numbers[i];
                            double right = numbers[i + 1];
                            numbers[i] = left / right;
                            numbers.RemoveAt(i + 1);
                            os.RemoveAt(i);
                        }
                        break;
                    default:
                        i++;
                        break;
                }
            }
        }

        // 足し算・引き算を行なう    
        double total = numbers[0];
        {
            for (int i = 0; i < os.Count; i++)
            {
                switch (os[i])
                {
                    case '+':
                        total += numbers[i + 1];
                        break;
                    case '-':
                        total -= numbers[i + 1];
                        break;
                }
            }
        }

        return total;
    }

    // 字句解析
    private static void LexicalAnalysis(string str, out List<string> ns, out List<char> os)
    {
        ns = new List<string>();
        os = new List<char>();

        string text = "";
        for (int i = 0; i < str.Length; i++)
        {
            switch (str[i])
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    ns.Add(text);
                    os.Add(str[i]);
                    text = "";
                    break;
                default:
                    if (IsNumber(str[i]))
                    {
                        text += str[i];
                        if (i == str.Length - 1)
                        {
                            ns.Add(text);
                            text = "";
                        }
                    }
                    break;
            }
        }

        // 字句解析結果をConsoleへ出力
        //string nt = "";
        //string ot = "";
        //foreach (var n in ns) { nt += n + ", "; }
        //foreach (var o in os) { ot += o + ", "; }
        //Debug.Log("数 : { " + nt + " }");
        //Debug.Log("演算子 : { " + ot + " }");
    }

    // 数式の構文解析
    private static Node Parse(char[] c)
    {
        Node root = new Node();
        Node target = root;
        string text = "";
        for (int i = 0; i < c.Length; i++)
        {
            switch (c[i])
            {
                case '(':
                    {
                        target.formula += "#";

                        // 子ノードを追加
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
                    target.formula += c[i];
                    break;
            }
        }

        // 構文解析結果表示
        //root.Log();

        return root;
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
#endif // UNITY_EDITOR

public class Formula : MonoBehaviour
{
    public string formula = "1+2*(3+4)+5*(6*(7+8)+(9+10))";
}

// 木構造をウィンドウで表示させるエディター拡張
public class FormulaNodeWindow : EditorWindow
{
    public static FormulaInspector.Node RootNode;

    protected void OnGUI()
    {
        BeginWindows();
        if (RootNode != null)
        {
            id = 0;
            this.Draw(RootNode, new Vector2(200, 200));
        }
        EndWindows();
    }

    static int id = 0;
    static Rect GetWindowRect(Vector2 pos)
    {
        const int SizeX = 120;
        const int SizeY = 45;
        Rect window = new Rect(pos, new Vector2(SizeX, SizeY));
        return window;
    }
    public void Draw(FormulaInspector.Node node, Vector2 position)
    {
        Rect window = GetWindowRect(position);
        GUI.Window(id++, window, DrawNodeWindow, node.formula);   // Updates the Rect's when these are dragged

        Vector2 left = position + new Vector2(-100, 100);
        Vector2 right = position + new Vector2(100, 100);
        Vector2 center = position + new Vector2(0, 100);
        int n = node.childs.Count;

        if (n == 1)
        {
            Vector2 childPos = center;
            Rect childWindow = GetWindowRect(childPos);

            DrawNodeLine(window, childWindow); // Here the curve is drawn under the windows
            Draw(node.childs[0], childPos);
        }
        else
        if (n > 1)
        {
            for (int i = 0; i < n; i++)
            {
                float t = i / (n - 1);
                Vector2 childPos = Vector2.Lerp(left, right, t);
                Rect childWindow = GetWindowRect(childPos);

                DrawNodeLine(window, childWindow); // Here the curve is drawn under the windows
                Draw(node.childs[i], childPos);
            }
        }
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
        //GUI.Label(new Rect(30, 22, 100, 100), "id = " + id, EditorStyles.label);
    }

    static void DrawNodeLine(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        Handles.DrawLine(startPos, endPos);
    }
}