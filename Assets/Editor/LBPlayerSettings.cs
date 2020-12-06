using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LBPlayerSettings : Editor
{
    [MenuItem("Tools / Build Symbols / HOST")]
    static void AddHostBuildSymbol()
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        List<string> allDefines = new List<string>(definesString.Split(';'));
        string[] Symbols = new string[] { "HOST" };

        allDefines.Remove("DEDICATED_SERVER");

        allDefines.AddRange(Symbols.Except(allDefines));

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    [MenuItem("Tools / Build Symbols / DEDICATED SERVER")]
    static void AddDedicatedServerBuildSymbol()
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        List<string> allDefines = new List<string>(definesString.Split(';'));
        string[] Symbols = new string[] { "DEDICATED_SERVER" };

        allDefines.Remove("HOST");

        allDefines.AddRange(Symbols.Except(allDefines));

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    [MenuItem("Tools / Build Symbols / CLIENT")]
    static void MakeItClientBuild()
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

        List<string> allDefines = new List<string>(definesString.Split(';'));

        allDefines.Remove("DEDICATED_SERVER");
        allDefines.Remove("HOST");

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }
}
