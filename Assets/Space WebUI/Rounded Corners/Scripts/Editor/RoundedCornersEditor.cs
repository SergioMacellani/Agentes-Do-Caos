using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(RoundedCorners))]
public class RoundedCornersEditor : Editor
{
    private SerializedProperty Sprite;
    private SerializedProperty TopLeft;
    private SerializedProperty BottomLeft;
    private SerializedProperty TopRight;
    private SerializedProperty BottomRight;

    private void OnEnable()
    {
        Sprite = serializedObject.FindProperty("m_Sprite");
        TopLeft = serializedObject.FindProperty("TopLeft");
        BottomLeft = serializedObject.FindProperty("BottomLeft");
        TopRight = serializedObject.FindProperty("TopRight");
        BottomRight = serializedObject.FindProperty("BottomRight");
    }

    public override void OnInspectorGUI()
    {
        RoundedCorners roundedCorners = (RoundedCorners)target;
        
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Image", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(Sprite, new GUIContent("Sprite",Texture2D.blackTexture));
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Corner Radius", EditorStyles.boldLabel);
        
        EditorGUILayout.PropertyField(TopLeft, new GUIContent("Top Left",Texture2D.whiteTexture));
        EditorGUILayout.PropertyField(TopRight, new GUIContent("Top Right",Texture2D.whiteTexture));
        EditorGUILayout.PropertyField(BottomLeft, new GUIContent("Bottom Left",Texture2D.whiteTexture));
        EditorGUILayout.PropertyField(BottomRight, new GUIContent("Bottom Right",Texture2D.whiteTexture));

        serializedObject.ApplyModifiedProperties();
    }
}