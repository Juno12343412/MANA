using System;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.LogSystem;
using Zenject;

namespace UDBase.UI.Common {
	public class UIManager : MonoBehaviour {
		
		/// <summary>
		/// UI Manager 셋팅
		/// </summary>
		[Serializable]
		public class Settings {

			/// <summary>
			/// UI 객체들을 둘 캔버스
			/// </summary>
			[Tooltip("UI 객체들을 둘 캔버스")]
			public Canvas Canvas;

			/// <summary>
			/// 모든 UI 객체들을 끄고 켜는 키
			/// </summary>
			[Tooltip("모든 UI 객체들을 끄고 켜는 키")]
			public KeyCode ShowHideToggle;
		}
	   
		Settings _settings;

		bool _showHide = true;

		ILog _log;

		/// <summary>
		/// 중속성이 있는 Init
		/// </summary>
		[Inject]
        public void Init(Settings settings, ILog log) {

			_settings = settings;
			_log	  = log;
			if (!_settings.Canvas)
			{
				var wantedCanvas = FindObjectOfType<Canvas>();
				if (wantedCanvas)
				{
					_settings.Canvas = wantedCanvas.GetComponent<Canvas>();
				}
				else
				{
					_log.Warning(UI.Context, "You need to assign Canvas to UIManager or add UICanvas component to wanted canvas.");
				}
			}
		}

		void Update() {
			if ((_settings.ShowHideToggle != KeyCode.None) && Input.GetKeyDown(_settings.ShowHideToggle)) {
				Debug.Log("ㅎㅇ");

				if (_showHide) {
					ShowAll();
				} else {
					HideAll();
				}
				_showHide = !_showHide;
			}
		}

		/// <summary>
		/// 모든 UI 객체를 보여줌
		/// </summary>
		public void ShowAll() {
			var elements = UIElement.Instances;
			foreach (var element in elements) {
				if (!element.HasParent) {
					element.Show();
				}
			}
		}

		/// <summary>
		/// 모든 UI 객체를 숨김
		/// </summary>
		public void HideAll() {
			var elements = UIElement.Instances;
			foreach (var element in elements) {
				if (!element.HasParent) {
					element.Hide();
				}
			}
		}

		/// <summary>
		/// 그룹에 맞는 객체들을 보여줌
		/// </summary>
		public void Show(string group) {
			var elements = UIElement.Instances;

			foreach (var element in elements) {
				if (element.Group == group) {
					element.Show();
				}
			}
		}

		/// <summary>
		/// 그룹에 맞는 객체들을 숨겨줌
		/// </summary>
		public void Hide(string group) {
			var hideElements = new HashSet<UIElement>();

			foreach (var element in UIElement.Instances) {
				if (element.Group == group) {
					hideElements.Add(element);
				}
			}
			foreach (var element in hideElements) {
				element.Hide();
            }
		}

		public GameObject Find(string group) {
			var elements = UIElement.Instances;

			foreach (var element in elements)
			{
				if (element.Group == group)
				{
					return element.gameObject;
				}
			}
			return null;
		}
	}
}