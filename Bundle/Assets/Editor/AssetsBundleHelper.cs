using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

public class AssetsBundleHelper : MonoBehaviour {

	[MenuItem ("Assets Bundle/Export Selection Objects To Separated AssetsBundles (Current Target)")]
	static void AssetsBundle_ExportObjects () {

		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);

		string path = EditorUtility.SaveFolderPanel ("Save Obejcts To Separated Assets Bundles", "", "save");
		if (path.Length != 0) {
			foreach (Object obj in SelectedAsset) 
			{
				string targetPath = path + "/" + obj.name + ".assetbundle";
				if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget)) {
					Debug.Log(obj.name + " 资源打包成功");
				} 
				else 
				{
					Debug.Log(obj.name + " 资源打包失败");
				}
			}
		}

		AssetDatabase.Refresh ();	
	}

	[MenuItem ("Assets Bundle/Export Selection Objects To One Single AssetsBundle (Current Target)")]
	static void AssetsBundle_ExportObject () {

		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);

		if (SelectedAsset.Length == 1) {
			string path = EditorUtility.SaveFilePanel ("Save Scene To Assets Bundle", "", SelectedAsset[0].name, "unity3d");
			if (path.Length != 0) {
				BuildPipeline.BuildAssetBundle(SelectedAsset[0], null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
				AssetDatabase.Refresh ();
			}
		}
		else {
			string path = EditorUtility.SaveFilePanel ("Save Scene To Assets Bundle", "", "save", "unity3d");
			if (path.Length != 0) {
				BuildPipeline.BuildAssetBundle(null, SelectedAsset, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
				AssetDatabase.Refresh ();
			}
		}
	}

	[MenuItem ("Assets Bundle/Export Scene (Current Target)")]
	static void AssetsBundle_ExportScene () {
		string path = EditorUtility.SaveFilePanel ("Save Scene To Assets Bundle", "", getSceneName(EditorApplication.currentScene), "unity3d");
		if (path.Length != 0) {
			string []levels = {EditorApplication.currentScene};
			BuildPipeline.BuildPlayer(levels, path, EditorUserBuildSettings.activeBuildTarget, BuildOptions.BuildAdditionalStreamedScenes | BuildOptions.UncompressedAssetBundle);
			AssetDatabase.Refresh ();
		}
	}

	static string getSceneName (string path) {
		int indexLastComponent = path.LastIndexOf ("/");

		string lastComponent = path;
		if (indexLastComponent > 0 && indexLastComponent < path.Length - 1) {
			lastComponent = path.Substring (indexLastComponent + 1);
		}

		int indexExtention = lastComponent.LastIndexOf (".");
		string name = lastComponent.Substring (0, indexExtention);

		return name;
	}
}

