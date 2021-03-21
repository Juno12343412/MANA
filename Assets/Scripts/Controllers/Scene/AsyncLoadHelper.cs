using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UDBase.Controllers.SceneSystem {
	/// <summary>
	/// 비동기 씬 로딩을 도와주는 헬퍼 클래스
	/// </summary>
	public class AsyncLoadHelper : MonoBehaviour {
		public float Progress { get; private set; }

		Action<string> _loadCallback;

		void Start() {
			SceneManager.activeSceneChanged += OnSceneChanged;
		}

		void OnDestroy() {
			SceneManager.activeSceneChanged -= OnSceneChanged;
		}

		void OnSceneChanged(Scene scene0, Scene scene1) {
			_loadCallback?.Invoke(scene1.name);
		}

		internal void LoadScene(string sceneName, Action<string> callback) {
			_loadCallback = callback;
			StartCoroutine(LoadSceneCoroutine(sceneName));
		}

		IEnumerator LoadSceneCoroutine(string sceneName) {
			var operation = SceneManager.LoadSceneAsync(sceneName);
			operation.allowSceneActivation = false;
			while (!operation.isDone && operation.progress + Mathf.Epsilon < 0.90f ) {
				Progress = operation.progress;
				yield return null;
			}
			Progress = 1.0f;
			operation.allowSceneActivation = true;
		}
	}
}
