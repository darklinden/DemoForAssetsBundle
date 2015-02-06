using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour {

	public string urlPrfix = "http://127.0.0.1:8080/Unity";
	public string objName = "Monster.unity3d";
	public string scName = "AdditionalLevel.unity3d";
	public string objSavePath, scSavePath;
	public Text objProgressLabel, scProgressLabel;

	private WWW goDownloader, scDownloader;

	//public func
//	public void DownloadGameObject ();
//	public void DownloadScene ();
//	public void LoadGameObject ();
//	public void LoadScene ();
//	public void RemoveCachedGameObject ();
//	public void RemoveCachedScene ();

	// Use this for initialization
	void Start () {
		objSavePath = Application.persistentDataPath + "/" + objName;
		if (File.Exists(objSavePath)) {
			objProgressLabel.text = "100%";
		}

		scSavePath = Application.persistentDataPath + "/" + scName;
		if (File.Exists (scSavePath)) {
			scProgressLabel.text = "100%";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//utility
	GameObject gameObjectFromAssetsBundle (AssetBundle bundle) {
		GameObject go = null;
		
		if (bundle.mainAsset) {
			if (bundle.mainAsset.GetType() == typeof(GameObject)) {
				go = bundle.mainAsset as GameObject;
			}
		}
		
		if (go == null) {
			bundle.Unload(false);
			Object [] objs = bundle.LoadAll (typeof(GameObject));
			if (objs.Length > 0) go = objs[0] as GameObject;
		}
		
		return go;
	}
	
	string getSceneName (string path) {
		int indexLastComponent = path.LastIndexOf ("/");
		
		string lastComponent = path;
		if (indexLastComponent > 0 && indexLastComponent < path.Length - 1) {
			lastComponent = path.Substring (indexLastComponent + 1);
		}
		
		int indexExtention = lastComponent.LastIndexOf (".");
		string name = lastComponent.Substring (0, indexExtention);
		
		return name;
	}

	//GameObject download
	public void DownloadGameObject () {
		if (!File.Exists(objSavePath)) {
			StartCoroutine (DownloadGO());
		}
	}

	IEnumerator DownloadGO () {

		while (true) {
			if (goDownloader == null) {
				string fullUrl = urlPrfix + "/" + objName;
				goDownloader = new WWW (fullUrl);
				Debug.Log ("Downloading: " + fullUrl);
			}

			if (goDownloader.error != null) {
				Debug.Log(goDownloader.error);
				goDownloader.Dispose ();
				goDownloader = null;
				yield break;
			}
			
			if (!goDownloader.isDone) {
				int progress = Mathf.RoundToInt(goDownloader.progress * 100);
				objProgressLabel.text = progress + "%";
				yield return null;
			}
			else {
				DownlodGOSuccess ();
				yield break;
			}
		}
	}

	void DownlodGOSuccess () {
		Debug.Log ("Download Success!");
		File.WriteAllBytes (objSavePath, goDownloader.bytes);
		goDownloader.Dispose ();
		goDownloader = null;
	}

	//scene download
	public void DownloadScene () {
		if (!File.Exists(scSavePath)) {
			StartCoroutine (DownloadSC());
		}
	}

	IEnumerator DownloadSC () {

		while (true) {
			if (scDownloader == null) {
				string fullUrl = urlPrfix + "/" + scName;
				scDownloader = new WWW (fullUrl);
				Debug.Log ("Downloading: " + fullUrl);
			}

			if (scDownloader.error != null) {
				Debug.Log(scDownloader.error);
				scDownloader.Dispose ();
				scDownloader = null;
				yield break;
			}
			
			if (!scDownloader.isDone) {
				int progress = Mathf.RoundToInt(scDownloader.progress * 100);
				scProgressLabel.text = progress + "%";
				yield return new WaitForSeconds (.1f);
			}
			else {
				DownlodSCSuccess ();
				yield break;
			}
		}
	}

	void DownlodSCSuccess () {
		File.WriteAllBytes (scSavePath, scDownloader.bytes);
		scDownloader.Dispose ();
		scDownloader = null;
	}

	//load obj
	public void LoadGameObject () {
		AssetBundle ab = AssetBundle.CreateFromFile (objSavePath);
		GameObject go = gameObjectFromAssetsBundle (ab);
		Instantiate (go);
		ab.Unload (false);
	}

	//load scene
	public void LoadScene () {
		AssetBundle ab = AssetBundle.CreateFromFile (scSavePath);
		Application.LoadLevel (getSceneName (scSavePath));
	}

	//delete
	public void RemoveCachedGameObject () {
		File.Delete (objSavePath);
		objProgressLabel.text = "0%";
	}

	public void RemoveCachedScene () {
		File.Delete (scSavePath);
		scProgressLabel.text = "0%";
	}
}
