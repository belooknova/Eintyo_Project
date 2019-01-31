using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
[CreateAssetMenu(fileName = "Type", menuName = "DataBase/CreateType")]
public class type_DB : ScriptableObject
{
    //属性名
    [SerializeField]
    private string[] attributes;

    public string[] Attributes_Array
    {
        get { return attributes; }
    }
}


#if UNITY_EDITOR

/*-----inspector拡張コード-----*/
[CustomEditor(typeof(Skill_DB))]
public class DB_Skill_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
}

#endif