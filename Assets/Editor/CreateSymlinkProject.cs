using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class CreateSymlinkProject {

    [MenuItem("Utils/Create Symlink Project")]
    static void DoIt()
    {
        string folderName = EditorUtility.SaveFolderPanel("Symlink Project Location", "", "");

        string newAssetFolder = folderName + "/Assets";
        string currentAssetFolder = Application.dataPath;

        string newProjectSettingsFolder = folderName + "/ProjectSettings";
        string currentProjectSettingsFolder = Application.dataPath.Replace("Assets", "ProjectSettings");

        string newPackagesFolder = folderName + "/Packages";
        string currentPackagesFolder = Application.dataPath.Replace("Assets", "Packages");

        CreateSymlink(newAssetFolder, currentAssetFolder);
        CreateSymlink(newProjectSettingsFolder, currentProjectSettingsFolder);

        CreateSymlink(newPackagesFolder, currentPackagesFolder);


    }

    static void CreateSymlink(string newFolder, string linkFolder)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Process.Start ( "cmd.exe", "/c mklink /D \"" + newFolder + "\" \"" + linkFolder + "\"" );
        }
        else if (  Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor )
        {
            Process.Start ( "ln", "-s \"" + linkFolder + "\" \"" + newFolder + "\"" );
        }
    }
}
