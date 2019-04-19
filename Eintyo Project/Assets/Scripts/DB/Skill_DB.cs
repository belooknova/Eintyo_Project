using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[Serializable]
public class StateParam
{
    //状態識別番号
    [SerializeField]
    public int stateId;
    //確率
    [SerializeField]
    public int probability;

}

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "DataBase/Create_Skill")]
public class Skill_DB : ScriptableObject
{
    //
    [SerializeField]
    SkillDataManager skillDataManager;
    
    //スキルの名前
    [SerializeField]
    private string SkillName;

    //消費HP
    [SerializeField]
    private int Consum_hp = 0;

    //消費MP
    [SerializeField]
    private int Consum_sp = 0;

    //計算式
    [SerializeField]
    [Tooltip("ダメージの計算式\na.~:攻撃を与える側のstatus　b.~:攻撃を受ける側のstatus" +
                "\natk:攻撃力　def:防御力")]
    [Multiline(5)]
    private string formula;

    //命中率
    [SerializeField]
    [Range(0, 100)]
    private int accuracy = 100;

    //状態
    [SerializeField]
    private List<StateParam> status = new List<StateParam>();


    //属性
    [SerializeField]
    private int attribution;

    //対象
    [SerializeField]
    private int targetId;

    //--------------------
    public string Name { get { return SkillName; } }
    public int Consum_HP { get { return Consum_HP; } }
    public int Consum_SP { get { return Consum_SP; } }
    public string Formula { get { return formula; } }
    public int Accuracy { get { return accuracy; } }
    public int Attribute { get { return attribution; } }
    public List<StateParam> States { get { return status; } }


#if UNITY_EDITOR

    /*-----inspector拡張コード-----*/

    //リスト内表示のコード
    [CustomPropertyDrawer(typeof(StateParam))]
    public class StateParamDrawer : PropertyDrawer
    {
        StateDataManager SDM;


        void OnEnable()
        {
            SDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //元は 1 つのプロパティーであることを示すために PropertyScope で囲む
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                StateDataManager SDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;


                EditorGUIUtility.labelWidth = 30;
                position.height = EditorGUIUtility.singleLineHeight;
                var halfWidth = position.width * 0.5f;


                var idRect = new Rect(position)
                {
                    width = position.width / 4
                };

                var prRect = new Rect(idRect)
                {
                    width = idRect.width * 3,
                    x = idRect.x + 2 + idRect.width
                };


                //各プロパティーの SerializedProperty を求める
                var idProperty = property.FindPropertyRelative("stateId");
                var prProperty = property.FindPropertyRelative("probability");

                //ラベルとかの設定
                //GUIContent idContent = new GUIContent{text="状態ID" };
                GUIContent prContent = new GUIContent { text = "確率" };
                //状態名を取得
                var NameArray = SDM.GetNameArray();
                //表示
                idProperty.intValue = EditorGUI.Popup(idRect,idProperty.intValue, NameArray);
                EditorGUI.IntSlider(prRect, prProperty, 0, 100, prContent);

            }
        }
    }

    //スキルinspectorのコード
    [CustomEditor(typeof(Skill_DB))]
    public class DB_Skill_Editor : Editor
    {
        bool foldout = true;
        //StateDataManager SDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;
        ReorderableList reorderableList;
        Skill_DB _DB;
        StateDataManager SDM;
        type_DB TypeDB;

        void OnEnable()
        {
            _DB = target as Skill_DB;
            SDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;
            TypeDB = Resources.Load("DataBase/Type", typeof(type_DB)) as type_DB;

            var prop = serializedObject.FindProperty("status");

            reorderableList = new ReorderableList(serializedObject, prop);
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = prop.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(rect, element);
            };

            reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, prop.displayName);


        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //base.OnInspectorGUI();

            //情報を読み込む
            StateDataManager SDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;
            type_DB TypeDB = Resources.Load("DataBase/Type", typeof(type_DB)) as type_DB;
            
            //ここから始まる
            //名前
            EditorGUILayout.LabelField("【スキル名】");
            _DB.SkillName = EditorGUILayout.TextField( _DB.SkillName);

            //消費
            //EditorGUILayout.BeginHorizontal();
            _DB.Consum_hp = EditorGUILayout.IntField("消費HP", _DB.Consum_hp);
            _DB.Consum_sp = EditorGUILayout.IntField("消費SP", _DB.Consum_sp);
            //EditorGUILayout.EndHorizontal();

            //属性
            _DB.attribution = EditorGUILayout.Popup("属性", _DB.attribution, TypeDB.Attributes_Array);

            //計算式
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("【計算式】");
            _DB.formula = EditorGUILayout.TextField(_DB.formula);

            //命中率
            _DB.accuracy = EditorGUILayout.IntSlider("【命中率】", _DB.accuracy,0,100);

            //デバフ
            if ( foldout = EditorGUILayout.Foldout(foldout, "デバフ/バフの確率"))
            {
                reorderableList.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }



    }

#endif
}