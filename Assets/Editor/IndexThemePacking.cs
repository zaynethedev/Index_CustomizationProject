using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Linq;

public class IndexThemePacking : EditorWindow
{
    private GameObject selectedObject;
    private IndexCustomTheme selectedTheme;

    [MenuItem("Index Customization Tools/Index Theme Packing")]
    public static void ShowWindow()
    {
        GetWindow<IndexThemePacking>("Index Theme Packing");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a Theme Object", EditorStyles.boldLabel);
        var themeObjects = FindObjectsOfType<IndexCustomTheme>().Select(t => t.gameObject).ToArray();

        if (themeObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("No objects found with IndexCustomTheme component.", MessageType.Warning);
            return;
        }

        foreach (var obj in themeObjects)
        {
            if (GUILayout.Button(obj.name))
            {
                selectedObject = obj;
                selectedTheme = obj.GetComponent<IndexCustomTheme>();
            }
        }

        if (selectedObject != null && selectedTheme != null)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Selected Theme:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Name:", selectedTheme.ThemeName);
            EditorGUILayout.LabelField("Description:", selectedTheme.ThemeDescription);
            EditorGUILayout.LabelField("Author:", selectedTheme.ThemeAuthor);
            GUILayout.Space(10);
            if (GUILayout.Button("BUNDLE"))
            {
                BundleTheme();
            }
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
        GameObject prefabObject = new GameObject("Theme");
        selectedObject.transform.SetParent(prefabObject.transform);
        PrefabUtility.SaveAsPrefabAsset(prefabObject, tempPath);

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

        File.Delete(jsonPath);
        File.Delete(bundlePath);

        EditorUtility.DisplayDialog("Success", "Theme packed successfully!", "OK");
    }
}