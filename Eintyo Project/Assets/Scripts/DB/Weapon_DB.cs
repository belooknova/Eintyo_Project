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
[CreateAssetMenu(fileName = "WeaponData", menuName = "DataBase/Create_WeaponData")]
public class Weapon_DB : Item_DB {

    public int attribution; // 属性


}
#if UNITY_EDITOR

/*-----inspector拡張コード-----*/
//スキルinspectorのコード
[CustomEditor(typeof(Weapon_DB))]
public class DB_Weapon_Editor : Editor
{
    Weapon_DB _DB;
    type_DB TypeDB;
    ReorderableList reorderableList;
    ReorderableList reorderableList2;
    bool foldout = true;
    bool foldout2 = true;

    private void OnEnable()
    {
        _DB = target as Weapon_DB;

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

        TypeDB = Resources.Load("DataBase/Type", typeof(type_DB)) as type_DB;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();
        //return;

        EditorGUILayout.LabelField("【武器名】");
        _DB.mataName = EditorGUILayout.TextField(_DB.mataName);

        _DB.attribution = EditorGUILayout.Popup("属性", _DB.attribution, TypeDB.Attributes_Array);

        _DB.cost = EditorGUILayout.IntField("所持コスト", _DB.cost);
        _DB.unknownName = EditorGUILayout.TextField("未鑑定時の名前(名詞)", _DB.unknownName);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("説明");
        _DB.description = EditorGUILayout.TextArea(_DB.description, GUILayout.Height(50));

        if (foldout = EditorGUILayout.Foldout(foldout, "所持時の効果"))
        {
            reorderableList2.DoLayoutList();
        }

        serializedObject.ApplyModifiedProperties();
    }

}

#endif