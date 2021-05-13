using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UDBase.Controllers.SceneSystem.UI {

	/// <summary>
	/// 유니티에서 IScene 로딩 진행 상황을 보여주는 구성 요소 UI.Text(퍼센트) 및 이미지(게이지)
	/// </summary>
	[AddComponentMenu("UDBase/UI/Scene/LoadingView")]
	public class LoadingView : MonoBehaviour {
		[Header("Dependencies")]
		public Image ProgressBar;
		public Text PercentText;

		[Header("Settings")]
		public int PercentDecimals;

		AsyncLoadHelper _helper;
		float _progress;

		/// <summary>
		/// 중속성을 가진 Init
		/// </summary>
		[Inject]
		public void Init(AsyncLoadHelper helper) {
			_helper = helper;
		}

		void Update() {
			UpdateState();
		}

		void UpdateState() {
			if (_helper) {
				_progress = _helper.Progress;
				if (PercentText) {
					var percents = Math.Round(_progress * 100, PercentDecimals);
					PercentText.text = string.Format("{0}%", percents);
				}
				if (ProgressBar) {
					ProgressBar.fillAmount = _progress;
				}
			}
		}
	}
}