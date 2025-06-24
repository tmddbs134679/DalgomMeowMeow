using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine.TextCore.Text;
using UnityEngine;

[CustomEditor(typeof(AICharacter))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기본 인스펙터 유지

        AICharacter character = (AICharacter)target;

        if (GUILayout.Button("레벨업"))
        {
            character.OnLevelUp();
        }

       
    }
}