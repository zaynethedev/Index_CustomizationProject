using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Linq;

public class IndexThemePacking : EditorWindow
{
    private GameObject selectedObject;
    private IndexCustomTheme selectedTheme;
    private Font font;

    [MenuItem("Index Customization Tools/Index Theme Packing")]
    public static void ShowWindow()
    {
        GetWindow<IndexThemePacking>("Index Theme Packing");
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

        GUILayout.Label("INDEX THEME BUNDLER", headerStyle, GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        GUIStyle bigLabel = new GUIStyle(EditorStyles.label)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            font = font
        };

        GUIStyle bigLabel2 = new GUIStyle(EditorStyles.label)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            font = font
        };

        GUIStyle bigButton = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            font = font
        };

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("SELECT A THEME OBJECT", bigLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        var themeObjects = FindObjectsOfType<IndexCustomTheme>().Select(t => t.gameObject).ToArray();

        if (themeObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("No objects found with IndexCustomTheme component.", MessageType.Warning);
            return;
        }

        foreach (var obj in themeObjects)
        {
            Vector2 buttonSize = bigButton.CalcSize(new GUIContent(obj.name.ToUpper()));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(obj.name.ToUpper(), bigButton, GUILayout.Width(buttonSize.x + 15)))
            {
                selectedObject = obj;
                selectedTheme = obj.GetComponent<IndexCustomTheme>();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        if (selectedObject != null && selectedTheme != null)
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
            GUILayout.Label("- SELECTED THEME -", bigLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("NAME: " + selectedTheme.ThemeName.ToUpper(), bigLabel2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("DESCRIPTION: " + selectedTheme.ThemeDescription.ToUpper(), bigLabel2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("AUTHOR: " + selectedTheme.ThemeAuthor.ToUpper(), bigLabel2);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.Space(10);

            Vector2 bundleButtonSize = bigButton.CalcSize(new GUIContent($"BUNDLE '{selectedTheme.ThemeName.ToUpper()}'"));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button($"BUNDLE '{selectedTheme.ThemeName.ToUpper()}'", bigButton, GUILayout.Width(bundleButtonSize.x + 15)))
            {
                BundleTheme();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    private void BundleTheme()
    {
        if (selectedObject == null || selectedTheme == null)
        {
            Debug.LogError("No theme selected!");
            return;
        }

        string bundleName = selectedTheme.ThemeName.Replace(" ", "_").ToLower();
        string outputFolder = Path.Combine(Application.dataPath, "IndexThemeBundles");
        string jsonPath = Path.Combine(outputFolder, "info.json");
        string bundlePath = Path.Combine(outputFolder, "theme.bundle");

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string jsonContent = "{\n" +
                             "  \"Name\": \"" + selectedTheme.ThemeName + "\",\n" +
                             "  \"Description\": \"" + selectedTheme.ThemeDescription + "\",\n" +
                             "  \"Author\": \"" + selectedTheme.ThemeAuthor + "\"\n" +
                             "}";

        File.WriteAllText(jsonPath, jsonContent);

        string tempPath = Path.Combine("Assets", $"{bundleName}_Temp.prefab");
        GameObject prefabObject = new GameObject($"{bundleName}_Parent");
        selectedObject.transform.SetParent(prefabObject.transform);
        PrefabUtility.SaveAsPrefabAsset(prefabObject, tempPath);
        selectedObject.transform.SetParent(null);
        Destroy(prefabObject);

        AssetBundleBuild build = new AssetBundleBuild
        {
            assetBundleName = "theme.bundle",
            assetNames = new[] { tempPath }
        };

        BuildPipeline.BuildAssetBundles(outputFolder, new[] { build }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        File.Delete(tempPath);
        AssetDatabase.Refresh();

        string zipPath = Path.Combine(outputFolder, $"{bundleName}.indextheme");
        using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
        {
            zip.CreateEntryFromFile(jsonPath, "info.json");
            zip.CreateEntryFromFile(bundlePath, "theme.bundle");
        }

        foreach (string filePath in Directory.GetFiles(outputFolder))
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Extension != ".indextheme" && file.Extension != ".meta")
            {
                File.Delete(filePath);
            }
        }

        EditorUtility.DisplayDialog("Success", "Theme packed successfully!", "OK");
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