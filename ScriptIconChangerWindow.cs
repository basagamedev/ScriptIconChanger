using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

public class ScriptIconChangerWindow : EditorWindow
{
    private MonoScript selectedScript;
    private Texture2D selectedIcon;

    [MenuItem("Tools/Script Icon Changer")]
    public static void ShowWindow()
    {
        ScriptIconChangerWindow window = GetWindow<ScriptIconChangerWindow>("Script Icon Changer");
        window.minSize = new Vector2(400, 200); 
        window.maxSize = new Vector2(400, 200);
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Script and Icon", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        selectedScript = (MonoScript)EditorGUILayout.ObjectField(new GUIContent("Script", "Select the script you want to change the icon for."), selectedScript, typeof(MonoScript), false);
        selectedIcon = (Texture2D)EditorGUILayout.ObjectField(new GUIContent("Icon", "Select the new icon for the script."), selectedIcon, typeof(Texture2D), false);

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(selectedScript == null || selectedIcon == null))
        {
            if (GUILayout.Button(new GUIContent("Change Icon", "Click to change the icon of the selected script.")))
            {
                ChangeIcon();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Please select both a script and an icon before clicking 'Change Icon'.", MessageType.Info);
    }

    private void ChangeIcon()
    {
        if (selectedScript == null)
        {
            EditorUtility.DisplayDialog("Error", "No script selected! Please select a script before changing the icon.", "OK");
            return;
        }

        if (selectedIcon == null)
        {
            EditorUtility.DisplayDialog("Error", "No icon selected! Please select an icon before changing the script icon.", "OK");
            return;
        }

        AssetDatabase.StartAssetEditing();
        string path = AssetDatabase.GetAssetPath(selectedScript);
        MonoImporter monoImporter = AssetImporter.GetAtPath(path) as MonoImporter;
        monoImporter.SetIcon(selectedIcon);
        AssetDatabase.ImportAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.StopAssetEditing();

        EditorUtility.DisplayDialog("Success", $"Icon for script {selectedScript.name} was successfully changed!", "OK");
        Debug.Log($"Icon for script {selectedScript.name} was successfully changed!");
    }
}
