using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExcelData))]
public class MapColParameterEditor : CustomComponentBase
{
    private enum ButtonAction
    {
        None,
        SetupColId,
        AddColName,
        AddColId,
        Remove
    }

    public override void OnInspectorGUI()
    {
        InitStyles();

        var mapInfo = (ExcelData) target;
        var action = ButtonAction.None;

        GUILayout.BeginVertical("box");
        
        GUILayout.BeginHorizontal();
        
        GUILayout.Label($"ExcelData List ({mapInfo.list.Count})", Styles.header);
        if (GUILayout.Button("-", new GUIStyle("button") {fixedWidth = 20f})) 
            action = ButtonAction.Remove;

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Setup map_col_id", new GUIStyle("button") )) 
            action = ButtonAction.SetupColId;
        
        if (GUILayout.Button("Add map_col_id", new GUIStyle("button") )) 
            action = ButtonAction.AddColId;
        
        GUILayout.Label("");
        
        if (GUILayout.Button("Add map_col_name", new GUIStyle("button") )) 
            action = ButtonAction.AddColName;

        GUILayout.EndVertical();
        
        mapInfo.list.ForEach(info =>
        {
            GUILayout.BeginVertical("box");

            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();

            for (var i = 0; i < info.list.Count; i++)
                info.list[i] = EditorGUILayout.TextField(info.list[i]);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        });

        switch (action)
        {
            case ButtonAction.Remove:
            {
                if (!mapInfo.list.IsNullOrEmpty()) 
                    mapInfo.list.RemoveAt(mapInfo.list.Count - 1);
                break;
            }
            case ButtonAction.SetupColId:
                mapInfo.list.Add(new ExcelData.Param {list = new List<string> {"map_col_id"}});
                mapInfo.list.Add(new ExcelData.Param {list = new List<string> {"当たり判定名", "消えるフレーム"}});
                break;
            case ButtonAction.AddColId:
                mapInfo.list.Add(new ExcelData.Param {list = new List<string> {"object_col", "object"}});
                break;
            case ButtonAction.AddColName:
                mapInfo.list.Add(new ExcelData.Param {list = new List<string> {"map_col_id", "id", "name"}});
                break;
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}