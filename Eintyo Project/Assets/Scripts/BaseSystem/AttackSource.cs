using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FormulePurser;

namespace AttackMata
{

    public class AttackSource
    {

        private StatusData A;  //server
        private StatusData B;  //receiver
        int index;

        SkillDataManager dataManager; //データベース
        Skill_DB skill;

        public Skill_DB Skill { get { return skill; } }

        public AttackSource(StatusData A, StatusData B, int index)
        {
            this.A = A;
            this.B = B;
            this.index = index;

            //扱うスキルを取り出す
            //Debug.Log(GameManager.instance.SkillList[index]);
            skill = GameManager.instance.SkillList[index];
            
        }

        //ダメージを算出
        public int DamageEval()
        {
            Purser p = new Purser(A, B, skill.Formula);
            return p.Eval();
        }

        
        //ステート
        public Dictionary<int, int> StatusJudge()
        {
            var outDict = new Dictionary<int, int>();
            foreach(StateParam state in skill.States)
            {
                outDict.Add(state.stateId, state.probability);
            }
            return outDict;
        }


            //Purser p = new Purser(A, A, "((1 + a.def)* 2) * (b.atk + 12)");
            //Purser p = new Purser(A, A, "34+67*78");

            //Debug.Log(p.Eval());
            //string s = "a.atk+56*34+(2 * a.hun)";
            //Debug.Log(s.IndexOf("a.atk"));
            //Debug.Log(p.Conversion_Status(s));
    }
}