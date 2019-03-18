using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[SerializeField]
[CreateAssetMenu(fileName = "AbilityValue", menuName = "DataBase/Create_AbilityValue")]
public class AbilityValue_DB : ScriptableObject
{
    private int index;
    [SerializeField]
    private string[] AbyNameJ;
    [SerializeField]
    private string[] AbyName;

    public string[] GetAbyNameJ()
    {
        return AbyNameJ;
    }

    public string[] GetAbyName()
    {
        return AbyName;
    }

    //名前からIndexを出す
    public int OfIndex(string name)
    {
        int index = 0 ;
        for (int i=0; i < AbyName.Length; i++)
        {
            if (name == AbyName[i])
            {
                index = i;
            }
        }
        return index;
    }


#if UNITY_EDITOR

    /*-----inspector拡張コード-----*/
    [CustomEditor(typeof(AbilityValue_DB))]
    public class DB_AbilityValue_Editor : Editor
    {
        bool foldout = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }
    }
#endif
}
