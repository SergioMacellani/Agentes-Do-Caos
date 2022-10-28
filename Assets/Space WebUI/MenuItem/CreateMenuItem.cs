using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateMenuItem
{
    #if UNITY_EDITOR
    private static void SafeInstantiate(GameObject prefab)
    {
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, Selection.activeTransform);
        
        PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
        Selection.activeObject = instance;
    }
    
    [MenuItem("GameObject/UI/Space WebUI/Mobile Toggle", false, 0)]
    private static void CreateButton()
    {
        SafeInstantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Space WebUI/Mobile Toggle/Resources/Mobile Toggle.prefab"));
    }
    
    [MenuItem("GameObject/UI/Space WebUI/Rounded Container", false, 0)]
    private static void CreateRoundedContainer()
    {
        SafeInstantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Space WebUI/Rounded Corners/Resources/Rounded Container.prefab"));
    }
    
    #endif
}
