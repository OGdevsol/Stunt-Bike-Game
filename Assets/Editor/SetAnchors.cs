using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class uGUITools : MonoBehaviour
{
	[MenuItem("Just Beginning/uGUI/Anchors to Corners %`")]
	static void AnchorsToCorners()
	{
		RectTransform t = Selection.activeTransform as RectTransform;
		RectTransform pt = Selection.activeTransform.parent as RectTransform;

		if (t == null || pt == null) return;

		Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
			t.anchorMin.y + t.offsetMin.y / pt.rect.height);
		Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
			t.anchorMax.y + t.offsetMax.y / pt.rect.height);

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2(0, 0);
	}

	[MenuItem("Just Beginning/uGUI/Corners to Anchors %]")]
	static void CornersToAnchors()
	{
		RectTransform t = Selection.activeTransform as RectTransform;

		if (t == null) return;

		t.offsetMin = t.offsetMax = new Vector2(0, 0);
	}

	[MenuItem("Just Beginning/uGUI/Unlock All Game")]
	static void UnlockAllGame()
	{ 
		RewardSystem.instance.CustomUnlockAllGame();
	}

}

class EditorScripts:EditorWindow
{
	[MenuItem("Just Beginning/Play/OpenScene 0 _%q")]
	public static void OpenOfflineScene0()
	{
		string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
		EditorSceneManager.OpenScene("Assets/Level/Scenes/Splash.unity");
	}
	[MenuItem("Just Beginning/Play/OpenScene 1 _%w")]
	public static void OpenOfflineScene1()
	{
		string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
		EditorSceneManager.OpenScene("Assets/Level/Scenes/Mainmenu.unity");
	}
	[MenuItem("Just Beginning/Play/OpenScene 2 _%e")]
	public static void OpenOfflineScene2()
	{
		string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
		EditorSceneManager.OpenScene("Assets/Level/Scenes/Gameplay.unity");
	}
}


