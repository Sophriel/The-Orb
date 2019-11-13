//  유니티 프로젝트 내의 Assets 폴더, ProjectSettings 폴더를 Export합니다.
//  아래 스크립트를 프로젝트에 추가하면 유니티 화면 위에 Export 탭이 생성됩니다.

//  유니티 패키지 형태로 저장되며 임포트시엔 다른 프로젝트에서 더블클릭만 해도 됩니다.
//  유니티 툴 자체 세팅 (Preferences, Modules)은 Export되지 않습니다.

using UnityEngine;
using System.Collections;
using UnityEditor;

public static class ExportPackage
{
    [MenuItem("Export/Export with tags and layers, Input settings")]

    public static void export()
    {
        //  익스포트 할 파일들 및 폴더 (세팅을 폴더로 묶으면 이상하게 익스포트가 안됩니다.)
        //  Physcis와 Physcis2D 세팅 주의
        string[] projectContent = new string[] { "Assets",
            "ProjectSettings/UnityConnectSettings.asset",
            "ProjectSettings/ProjectSettings.asset",
            "ProjectSettings/EditorSettings.asset",
            "ProjectSettings/EditorBuildSettings.asset",
            "ProjectSettings/QualitySettings.asset",
            "ProjectSettings/GraphicsSettings.asset",

            "ProjectSettings/AudioManager.asset",
            "ProjectSettings/DynamicsManager.asset",
            "ProjectSettings/InputManager.asset",
            "ProjectSettings/Physics2DSettings.asset",
            "ProjectSettings/TimeManager.asset",
            "ProjectSettings/TagManager.asset",
            "ProjectSettings/NavMeshAreas.asset",
            "ProjectSettings/NetworkManager.asset"
        };

        //  저장 형식
        AssetDatabase.ExportPackage(projectContent, "The Orb.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);

        Debug.Log("Project Exported");
    }
}