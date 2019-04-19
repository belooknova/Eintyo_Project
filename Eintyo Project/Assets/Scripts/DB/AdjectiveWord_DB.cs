using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
[CreateAssetMenu(fileName = "AdjectiveWord", menuName = "DataBase/Create_AdjectiveWord")]
public class AdjectiveWord_DB : ScriptableObject
{
    //形容詞収納変数
    [SerializeField]
    private string[] adjectiveWord;
    
    public string[] AdjectiveWord { get { return adjectiveWord; } }

    public int Length { get { return adjectiveWord.Length; } }


}
