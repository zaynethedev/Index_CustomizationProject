using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System;

public class IndexPlatformPacking : EditorWindow
{
    private GameObject selectedObject;
    private IndexCustomPlatform selectedPlatform;
    private Font font;

    [MenuItem("Index Customization Tools/Index Platform Packing")]
    public static void ShowWindow()
    {
        GetWindow<IndexPlatformPacking>("Index Platform Packing");
    }

    private void OnEnable()
    {
        font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/Utopium.otf");
    }

    private void OnGUI()
    {
        GUI.backgroundColor = new Color(0.38f, 0.5f, 0.8f);
        GUIStyle headerStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter,
            fixedHeight = 30,
            font = font
        };

        GUILayout.Label("INDEX PLATFORM BUNDLER", headerStyle, GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        GUIStyle bigLabel = new GUIStyle(EditorStyles.label)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            font = font
        };

        GUIStyle bigButton = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            font = font
        };

        GUIStyle bigLabel2 = new GUIStyle(EditorStyles.label)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            font = font
        };

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("SELECT A PLATFORM OBJECT", bigLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        var platformObjects = FindObjectsOfType<IndexCustomPlatform>().Select(p => p.gameObject).ToArray();

        if (platformObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("No objects found with IndexCustomPlatform component.", MessageType.Warning);
            return;
        }

        foreach (var obj in platformObjects)
        {
            Vector2 buttonSize = bigButton.CalcSize(new GUIContent(obj.name.ToUpper()));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(obj.name.ToUpper(), bigButton, GUILayout.Width(buttonSize.x + 15)))
            {
                selectedObject = obj;
                selectedPlatform = obj.GetComponent<IndexCustomPlatform>();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        if (selectedObject != null && selectedPlatform != null)
        {
            GUILayout.Space(10);
            GUIStyle panelStyle = new GUIStyle
            {
                normal = { background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f)) },
                padding = new RectOffset(10, 10, 10, 10)
            };

            GUILayout.BeginVertical(panelStyle);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("- SELECTED PLATFORM -", bigLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("NAME: " + selectedPlatform.PlatformName.ToUpper(), bigLabel2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("DESCRIPTION: " + selectedPlatform.PlatformDescription.ToUpper(), bigLabel2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("AUTHOR: " + selectedPlatform.PlatformAuthor.ToUpper(), bigLabel2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.Space(10);

            Vector2 bundleButtonSize = bigButton.CalcSize(new GUIContent($"BUNDLE '{selectedPlatform.PlatformName.ToUpper()}'"));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button($"BUNDLE '{selectedPlatform.PlatformName.ToUpper()}'", bigButton, GUILayout.Width(bundleButtonSize.x + 15)))
            {
                BundlePlatform();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    private void BundlePlatform()
    {
        if (selectedObject == null || selectedPlatform == null)
        {
            Debug.LogError("No platform selected!");
            return;
        }

        string bundleName = selectedPlatform.PlatformName.Replace(" ", "_").ToLower();
        string outputFolder = Path.Combine(Application.dataPath, "IndexPlatformBundles");
        string jsonPath = Path.Combine(outputFolder, "info.json");
        string bundlePath = Path.Combine(outputFolder, "platform.bundle");

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string jsonContent = "{\n" +
                             "  \"Name\": \"" + selectedPlatform.PlatformName + "\",\n" +
                             "  \"Description\": \"" + selectedPlatform.PlatformDescription + "\",\n" +
                             "  \"Author\": \"" + selectedPlatform.PlatformAuthor + "\"\n" +
                             "}";
        File.WriteAllText(jsonPath, jsonContent);

        string tempPath = Path.Combine("Assets", $"{bundleName}_Temp.prefab");
        GameObject tempObject = Instantiate(selectedObject);
        PrefabUtility.SaveAsPrefabAsset(tempObject, tempPath);
        DestroyImmediate(tempObject);

        AssetBundleBuild build = new AssetBundleBuild
        {
            assetBundleName = "platform.bundle",
            assetNames = new[] { tempPath }
        };

        BuildPipeline.BuildAssetBundles(outputFolder, new[] { build }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        File.Delete(tempPath);
        AssetDatabase.Refresh();

        string zipPath = Path.Combine(outputFolder, $"{bundleName}.indexplatform");
        using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
        {
            zip.CreateEntryFromFile(jsonPath, "info.json");
            zip.CreateEntryFromFile(bundlePath, "platform.bundle");
        }

        foreach(string filePath in Directory.GetFiles(outputFolder))
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Extension != ".indexplatform" && file.Extension != ".meta")
            {
                File.Delete(filePath);
            }
        }

        EditorUtility.DisplayDialog("Success", "Platform packed successfully!", "OK");
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}