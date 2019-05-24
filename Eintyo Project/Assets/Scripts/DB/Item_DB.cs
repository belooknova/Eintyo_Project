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
[CreateAssetMenu(fileName = "ItemData", menuName = "DataBase/Create_ItemData")]
public class Item_DB : ScriptableObject
{
    [SerializeField]
    public string mataName; //内部的な名前 

    [SerializeField]
    public string unknownName; //未鑑定時の名前

    [SerializeField]
    public string description; //アイテムの説明

    [SerializeField]
    public int cost = 1;//アイテムの収納コスト

    [SerializeField]
    public int itemType; //アイテムの種類
    //0: 使用アイテム
    //1: 不使用アイテム

    [SerializeField]
    public List<AddBuffSource> addBuffs = new List<AddBuffSource>();

    [SerializeField]
    public List<AddBuffSource> passive = new List<AddBuffSource>();

    [SerializeField]
    public int usingType; //使用タイプ

    //--- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
    public string MataName { get { return mataName; } }
    public string UnknownName { get { return unknownName; } }
    public string Description { get { return description; } }
    public int Cost { get { return cost; } }
    public int ItemType { get { return itemType; } }
    public List<AddBuffSource> AddBuffs { get { return addBuffs; } }
    public List<AddBuffSource> Passive { get { return passive; } }



#if UNITY_EDITOR
    /*-----inspector拡張コード-----*/


   
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
            //return;

            EditorGUILayout.LabelField("【アイテム名】");
            _DB.mataName = EditorGUILayout.TextField(_DB.mataName);
            string[] useTypeLabel = { "いつでも使用可能", "戦闘時のみ使用可能", "非戦闘時のみ使用可能","使用不可" };
            _DB.usingType = EditorGUILayout.Popup(_DB.usingType, useTypeLabel);

            _DB.cost = EditorGUILayout.IntField("所持コスト", _DB.cost);
            _DB.unknownName = EditorGUILayout.TextField("未鑑定時の名前(名詞)", _DB.unknownName);

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
