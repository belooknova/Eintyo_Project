using System.Collections;
using System.Collections.Generic;
using System; 

using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif


public class testList : MonoBehaviour {

    [SerializeField]
    Character[] characters;


    [Serializable]
    public class Character
    {
        [SerializeField]
        Texture icon;

        [SerializeField]
        string name;

        [SerializeField]
        int hp;

        [SerializeField]
        int power;
    }





#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(Character))]
    public class CharacterDrawer : PropertyDrawer
    {
        private Character character;


        public override void OnGUI(Rect position,
          SerializedProperty property, GUIContent label)
        {
            //元は 1 つのプロパティーであることを示すために PropertyScope で囲む
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                
                //サムネの領域を確保するためにラベル領域の幅を小さくする
                //*(ラベルと入力フィールドの幅)
                EditorGUIUtility.labelWidth = 50;

                //一つ分の縦幅にする(？)
                //*消すと文字列のフィールドが大きくなる
                position.height = EditorGUIUtility.singleLineHeight;

                var halfWidth = position.width * 0.5f;

                

                //各プロパティーの Rect を求める
                var iconRect = new Rect(position)
                {
                    width = 64,
                    height = 64
                };

                var nameRect = new Rect(position)
                {
                    width = position.width - 64,
                    x = position.x + 64
                };

                var hpRect = new Rect(nameRect)
                {
                    y = nameRect.y + EditorGUIUtility.singleLineHeight + 2
                };

                var powerRect = new Rect(hpRect)
                {
                    y = hpRect.y + EditorGUIUtility.singleLineHeight + 2
                };

                //各プロパティーの SerializedProperty を求める
                var iconProperty = property.FindPropertyRelative("icon");
                var nameProperty = property.FindPropertyRelative("name");
                var hpProperty = property.FindPropertyRelative("hp");
                var powerProperty = property.FindPropertyRelative("power");

                Debug.Log(iconProperty);

                //各プロパティーの GUI を描画
                iconProperty.objectReferenceValue =
                  EditorGUI.ObjectField(iconRect,
                    iconProperty.objectReferenceValue, typeof(Texture), false);

                nameProperty.stringValue =
                  EditorGUI.TextField(nameRect,
                    nameProperty.displayName, nameProperty.stringValue);

                EditorGUI.IntSlider(hpRect, hpProperty, 0, 100);
                EditorGUI.IntSlider(powerRect, powerProperty, 0, 10);

            }
        }
    }


    /*-----inspector拡張コード-----*/
    [CustomEditor(typeof(testList))]
    public class ExampleInspector : Editor
    {
        ReorderableList reorderableList;

        void OnEnable()
        {
            var prop = serializedObject.FindProperty("characters");

            reorderableList = new ReorderableList(serializedObject, prop);

            //縦の大きさを変更
            reorderableList.elementHeight = 68;
            //属性の変更
            reorderableList.drawElementCallback =
              (rect, index, isActive, isFocused) => {
                  //?
                  var element = prop.GetArrayElementAtIndex(index); 
                  //矩形の調整？
                  rect.height -= 4; 
                  //矩形の調整？
                  rect.y += 2; 
                  //propertyを表示する範囲の設定？
                  EditorGUI.PropertyField(rect, element);
              };
            
            //どこかの色の変更？
            var defaultColor = GUI.backgroundColor;

            //ヘッダーの設定
            reorderableList.drawHeaderCallback = (rect) =>
              EditorGUI.LabelField(rect, prop.displayName);

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //ReorderableListの表示
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
