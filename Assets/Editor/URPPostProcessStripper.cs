//using System.Collections.Generic;
//using System.IO;
//using UnityEditor.Build;
//using UnityEditor.Build.Reporting;
//using UnityEditor.Rendering;
//using UnityEngine;
//
///// <summary>
///// Strips all shaders from the URP from build, none of these seem to be required when not using post processing.
///// Also makes any textures used by the URP post processing 32x32 to make them as small as possible.
///// This saves around 3-4 mb's for my test project. Your mileage may vary.
///// </summary>
//
//internal class URPPostProcessStripper : IPreprocessShaders, IPreprocessBuildWithReport {
//    public int callbackOrder => 0;
//
//    public void OnPreprocessBuild(BuildReport report) {
//        var packagesPath = Application.dataPath.Replace("/Assets", "") + "/Library/PackageCache/";
//        Recurse(Directory.GetDirectories(packagesPath, "com.unity.render-pipelines.*"));
//    }
//
//    static void Recurse(string[] directories) {
//        foreach (var directory in directories) {
//            if (IsIgnoredPath(directory)) continue;
//
//            // Debug.Log($"directory: {directory}");
//            // Debug.Log($"Modifying entries in: {directory}");
//
//            StripFiles(Directory.GetFiles(directory, "*.png.meta"));
//            StripFiles(Directory.GetFiles(directory, "*.tga.meta"));
//
//            Recurse(Directory.GetDirectories(directory));
//        }
//    }
//
//    static void StripFiles(string[] files) {
//        foreach (var filename in files) {
//            if (IsIgnoredPath(filename)) continue;
//            
//            // Debug.Log($"Modifying entry: {filename}");
//            var lines = File.ReadAllLines(filename);
//            for (var i = 0; i < lines.Length; i++) {
//                lines[i] = lines[i].Replace("maxTextureSize: 2048", "maxTextureSize: 32");
//            }
//
//            File.WriteAllLines(filename, lines);
//        }
//    }
//
//    static bool IsIgnoredPath(string path) {
//        return path.Contains("editor") || path.Contains("Editor");
//    }
//
//    public void OnProcessShader(
//        Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData) {
//        Debug.Log(shader.name);
//        if (!shader.name.Contains("Universal Render Pipeline")) return;
//        shaderCompilerData.Clear();
//    }
//}