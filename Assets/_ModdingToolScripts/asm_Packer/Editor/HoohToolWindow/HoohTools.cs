﻿#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Common;
using UnityEditor;
using UnityEngine;

public partial class HoohTools : EditorWindow
{
    // TODO: Make these moddable with windows.
    private static Dictionary<string, Texture2D> _icons;
    public static int Category;
    public static string SideloaderString = "";
    public static int CategorySmall;
    public static float LightScaleSize = 9f;
    public static string GameExportPath = "";
    public static string PrepostString = "";
    public static int Gap = 10;
    public static int Cols = 10;

    private readonly float maxSliderValue = 5.0f;
    private readonly float minSliderValue = 1.0f;
    private GUIStyle bigButton;
    private bool foldoutBundler = true;

    private bool foldoutElement = true;
    private bool foldoutMacros = true;
    private bool foldoutMod = true;
    private bool foldoutProbeset = true;
    private bool foldoutScaffolding = true;

    public GameObject fuckyouCunt;
    private GUIStyle headerStyle;
    private float inputNumber;

    private Vector2 scrollPos;
    private float sliderValue;
    private GUIStyle smallButton;
    private GUIStyle titleStyle;

    private Styles _styles;

    private void Awake()
    {
    }

    public void OnEnable()
    {
        _styles = new Styles();
        _icons = new Dictionary<string, Texture2D>
        {
            {"plus", Resources.Load("icons/plus") as Texture2D},
            {"minus", Resources.Load("icons/minus") as Texture2D},
            {"reset", Resources.Load("icons/arrow_repeat") as Texture2D},
            {"wrap", Resources.Load("icons/wrap") as Texture2D},
            {"wrapscale", Resources.Load("icons/wrapscale") as Texture2D}
        };
        
        smallButton = new GUIStyle
        {
            fixedWidth = 16,
            fixedHeight = 16,
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(5, 0, 0, 0)
        };
        bigButton = new GUIStyle
        {
            fixedWidth = 32,
            fixedHeight = 32,
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(5, 0, 0, 0)
        };
        headerStyle = new GUIStyle {fontSize = 12};
        titleStyle = new GUIStyle()
        {
            fontSize = 15,
            margin = new RectOffset(10, 10, 0, 10)
        };

        foldoutElement = EditorPrefs.GetBool("hoohTool_foldoutElement", true);
        foldoutMacros = EditorPrefs.GetBool("hoohTool_foldoutMacros", true);
        foldoutProbeset = EditorPrefs.GetBool("hoohTool_foldoutProbeset", true);
        foldoutScaffolding = EditorPrefs.GetBool("hoohTool_foldoutScaffolding", true);
        foldoutMod = EditorPrefs.GetBool("hoohTool_foldoutMod", true);
        foldoutBundler = EditorPrefs.GetBool("hoohTool_foldoutBundler", true);

        Category = EditorPrefs.GetInt("hoohTool_category"); // this is mine tho
        SideloaderString = EditorPrefs.GetString("hoohTool_sideloadString");
        CategorySmall = EditorPrefs.GetInt("hoohTool_categorysmall"); // this is mine tho
        GameExportPath = EditorPrefs.GetString("hoohTool_exportPath");
        sliderValue = EditorPrefs.GetFloat("hoohTool_probeStrength");
        LightScaleSize = 9f;
    }

    public static void DrawUILine(Color color, int thickness = 1, int padding = 15)
    {
        var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

    private bool CheckGoodSelection()
    {
        if (Selection.objects.Length <= 0)
        {
            EditorUtility.DisplayDialog("Error!", "You need to select at least one or more objects.", "Dismiss");
            return false;
        }

        return true;
    }
    
    private void OnGUI()
    {
        _styles.Init();
        var serializedObject = new SerializedObject((ScriptableObject) this);
            
        EditorGUILayout.BeginVertical();
        // Draw Top Help/Tutorial Boxes
        GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Check Updates")) Application.OpenURL("https://github.com/hooh-hooah/ModdingTool/tree/release/");
            if (GUILayout.Button("Tutorials")) Application.OpenURL("https://github.com/hooh-hooah/AI_Tips");
        GUILayout.EndHorizontal();
        // Draw Separator Lines
        DrawUILine(new Color(0, 0, 0));
        // Draw Scroll Positions
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(0), GUILayout.Height(0));
        {
            DrawLightProbeSetting(serializedObject);
            DrawUnityUtility(serializedObject);
            DrawXMLHelper(serializedObject);
            DrawModBuilder(serializedObject);
        }
        EditorGUILayout.EndScrollView();
        // End Scroll Positions
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        UpdateEditorPreference();
    }

    private void UpdateEditorPreference()
    {
        EditorPrefs.SetBool("hoohTool_foldoutElement", foldoutElement);
        EditorPrefs.SetBool("hoohTool_foldoutMacros", foldoutMacros);
        EditorPrefs.SetBool("hoohTool_foldoutProbeset", foldoutProbeset);
        EditorPrefs.SetBool("hoohTool_foldoutScaffolding", foldoutScaffolding);
        EditorPrefs.SetBool("hoohTool_foldoutMod", foldoutMod);
        EditorPrefs.SetBool("hoohTool_foldoutBundler", foldoutBundler);
    }

    // Show control window - WiP
    [MenuItem("hooh Tools/Show Window")]
    public static void ShowWindow()
    {
        GetWindow<HoohTools>(false, "hooh Tools", true);
    }
}
#endif