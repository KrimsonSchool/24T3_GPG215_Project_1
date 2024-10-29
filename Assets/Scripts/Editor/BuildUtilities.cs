using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildUtilities : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    [MenuItem("File/Update Build Version")]
    // Sets the build version to a new one manually
    private static void UpdateBuildVersion()
    {
        string buildVersion = GetNewBuildVersion();
        PlayerSettings.bundleVersion = buildVersion;
    }
    
    // Returns a build version in the format of buildYYMMDD#_platform
    private static string GetNewBuildVersion()
    {
        string output = string.Empty;

        // The existing build version (Project Settings > Player > Version)
        string buildVersion = PlayerSettings.bundleVersion;

        // The target platform (Build Settings > Platform)
        string platform = EditorUserBuildSettings.activeBuildTarget.ToString().ToLower();
        
        // The current date in yyMMdd format (MM instead of mm to indicate months, not minutes)
        string date = DateTime.Now.ToString("yyMMdd");

        // Checks for development build (Build Settings > Development Build) and sets the string accordingly
        string devBuild = EditorUserBuildSettings.development ? "dev" : "";
     
        // The number of builds done today
        string buildNumber = "1";

        // Trying to see if any other builds were made today
        // If current build version contains a "_"...
        if (buildVersion.IndexOf("_") != -1)
        {
            // Get the index of the "_"
            int underscoreIndex = buildVersion.IndexOf("_");

            // Check to see if enough characters before the underscore exist for the date format
            if (underscoreIndex > date.Length + 1)
            {
                // Find the date (based on location of "_")
                string oldDate = buildVersion.Substring(underscoreIndex - (date.Length + 1), date.Length);

                if (oldDate == date)
                {
                    // Find the character that comes right before the "_" character
                    string oldBuildNumber = buildVersion.Substring(underscoreIndex - 1, 1);

                    // If the found character is an int...
                    if (int.TryParse(oldBuildNumber, out int i))
                    {
                        // Set the new build number to be old build number + 1 (max 9 to avoid double digits)
                        buildNumber = Math.Clamp(i++, 1, 9).ToString();
                    }
                }
            }
        }

        output = $"{devBuild}build{date}{buildNumber}_{platform}";

        Debug.Log($"Build Version: {output}");

        return output;
    }

    // Not yet implemented, but there to show builds in game
    private static void ShowBuildNumberInGame()
    {
        throw new NotImplementedException();
    }

    // Updates the build version every time before being built
    public void OnPreprocessBuild(BuildReport report)
    {
        UpdateBuildVersion();
    }
}
