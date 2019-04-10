using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using EintyoSystem;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif


[Serializable]
[CreateAssetMenu(fileName = "ItemDaata", menuName = "DataBase/CreateItemData")]
public class Item_DB : ScriptableObject
{
    [SerializeField]
    private string mataName; //内部的な名前 

    [SerializeField]
    private string unknownName; //未鑑定時の名前

    [SerializeField]
    private string description; //アイテムの説明

    [SerializeField]
    private int cost = 1;//アイテムの収納コスト

    [SerializeField]
    private int itemType; //アイテムの種類
    //0: 使用アイテム
    //1: 不使用アイテム

    [SerializeField]
    private List<AddBuffSource> addBuffs = new List<AddBuffSource>();

    [SerializeField]
    private List<AddBuffSource> passive = new List<AddBuffSource>();

#if UNITY_EDITOR
    /*-----inspector拡張コード-----*/


    //リスト内表示のコード
    [CustomPropertyDrawer(typeof(AddBuffSource))]
    public class BuffParamDrawer : PropertyDrawer
    {

        void OnEnable()
        {

        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                AbilityValue_DB AVD = Resources.Load("DataBase/AbilityValue", typeof(AbilityValue_DB)) as AbilityValue_DB;


                EditorGUIUtility.labelWidth = 30;
                position.height = EditorGUIUtility.singleLineHeight;

                var typeRect = new Rect(position) {y=position.y+4};
                var indexpRect = new Rect(position)
                {
                    y = typeRect.y + EditorGUIUtility.singleLineHeight + 16,
                    width = position.width * 0.5f 
                };
                var labelRect = new Rect(indexpRect)
                {
                    y = indexpRect.y + EditorGUIUtility.singleLineHeight + 2,
                    width = position.width
                };
                var indexRect = new Rect(indexpRect)
                {
                    x = indexpRect.width +48,
                    width = indexpRect.width - 32
                };
                var aperRect = new Rect(labelRect)
                {
                    width = 32,
                };
                var aconRect = new Rect(aperRect)
                {
                    x = 92
                };

                //------各プロパティーの SerializedProperty を求める------
                var typeProperty  = property.FindPropertyRelative("type");
                var indexProperty = property.FindPropertyRelative("index");
                var aperProperty  = property.FindPropertyRelative("addPercent");
                var aconProperty  = property.FindPropertyRelative("addConstant");
                var percProperty  = property.FindPropertyRelative("percent");

                //------各プロパティーの GUI を描画------

                //バーに表示する内容
                string[] BarLabel = { "能力値", "状態付与", "状態解除" };
                typeProperty.intValue = EditorGUI.Popup(typeRect, typeProperty.intValue, BarLabel);

                if (typeProperty.intValue == 0)
                {
                    indexProperty.intValue = EditorGUI.Popup(indexpRect, indexProperty.intValue, AVD.GetAbyNameJ());
                    EditorGUIUtility.labelWidth = 40;
                    indexProperty.intValue = EditorGUI.IntField(indexRect, "能力ID", indexProperty.intValue);
                    EditorGUIUtility.labelWidth = 30;
                    EditorGUI.LabelField(labelRect, "           % +             増加させる");
                    aperProperty.intValue = EditorGUI.IntField(aperRect, aperProperty.intValue);
                    aconProperty.intValue = EditorGUI.IntField(aconRect, aconProperty.intValue);
                }

            }
        }
    }


    //スキルinspectorのコード
    [CustomEditor(typeof(Item_DB))]
    public class DB_Item_Editor : Editor
    {
        Item_DB _DB;
        ReorderableList reorderableList;
        ReorderableList reorderableList2;
        bool foldout = true;
        bool foldout2 = true;

        private void OnEnable()
        {
            _DB = target as Item_DB;


            //--- --- --- --- --- --- --- --- --- --- --- ---
            var prop = serializedObject.FindProperty("addBuffs");
            reorderableList = new ReorderableList(serializedObject, prop);
            reorderableList.elementHeight = 80;

            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = prop.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            };
            reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "使用時の効果");
            //--- --- --- --- --- --- --- --- --- --- --- ---

            var prop2 = serializedObject.FindProperty("passive");
            reorderableList2 = new ReorderableList(serializedObject, prop2);
            reorderableList2.elementHeight = 80;
            reorderableList2.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = prop2.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element);
            };
            reorderableList2.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "所持時の効果(パッシブ)");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //base.OnInspectorGUI();
            

            EditorGUILayout.LabelField("【アイテム名】");
            _DB.mataName = EditorGUILayout.TextField(_DB.mataName);
            _DB.cost = EditorGUILayout.IntField("所持コスト", _DB.cost);
            _DB.unknownName = EditorGUILayout.TextField("未鑑定時の名前", _DB.unknownName);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("説明");
            _DB.description = EditorGUILayout.TextArea(_DB.description, GUILayout.Height(50));

            //ReorderableListの表示
            if (foldout = EditorGUILayout.Foldout(foldout, "使用時の効果"))
            {
                reorderableList.DoLayoutList();
            }

            if (foldout = EditorGUILayout.Foldout(foldout, "所持時の効果"))
            {
                reorderableList2.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

    }


#endif


}
