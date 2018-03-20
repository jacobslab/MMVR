using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Phrase))]
public class PhraseContainerDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), /*new GUIContent("aa")*/label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        Rect textRect = new Rect(position.x, position.y, 100, position.height);
        Rect clipRect = new Rect(position.x + 105, position.y, 100, position.height);
        EditorGUI.PropertyField(textRect, property.FindPropertyRelative("text"), GUIContent.none);
        EditorGUI.PropertyField(clipRect, property.FindPropertyRelative("audioClip"), GUIContent.none);
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    [MenuItem("Assets/Create/PhraseContainer")]
    static void CreateTileSet()
    {
        PhraseContainer asset = ScriptableObject.CreateInstance<PhraseContainer>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path)) { path = "Assets"; }
        else if (Path.GetExtension(path) != "") { path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), ""); }
        else { path += "/"; }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/PhraseContainer.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;
    }
}
