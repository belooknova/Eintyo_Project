using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "DataBase/CreateSkill")]
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
    private List<int> status;

    //属性
    [SerializeField]
    private int attribution;



    private void OnEnable()
    {

    }

    


    public string GetName()
    {
        return SkillName;
    }

    public int GetConsumHp()
    {
        return Consum_hp;
    }

    public int GetConsumMp()
    {
        return Consum_sp;
    }


#if UNITY_EDITOR

    /*-----inspector拡張コード-----*/
    [CustomEditor(typeof(Skill_DB))]
    public class DB_Skill_Editor : Editor
    {
        bool foldout = false;
        //StateDataManager SDM = Resources.Load("DataBase/StateDataManager", typeof(StateDataManager)) as StateDataManager;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //base.OnInspectorGUI();

            //情報を読み込む
            Skill_DB _DB = target as Skill_DB;
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
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("【計算式】");
            _DB.formula = EditorGUILayout.TextField(_DB.formula);

            //命中率
            _DB.accuracy = EditorGUILayout.IntSlider("【命中率】", _DB.accuracy,0,100);

            //デバフ
            if ( foldout = EditorGUILayout.Foldout(foldout, "デバフ/バフの確率"))
            {
                
                for(int i=0; i < SDM.GetListLenght(); i++)
                {
                    _DB.status[i] = EditorGUILayout.IntSlider(SDM.GetStateLists()[i].StateName , _DB.status[i],0,100);
                }
                
            }
            

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}