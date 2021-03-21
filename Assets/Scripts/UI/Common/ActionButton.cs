using UnityEngine;
using UnityEngine.UI;

namespace UDBase.UI.Common
{
	/// <summary>
	/// 특정 작업을 수행하는 기본 클래스
	/// </summary>
	[RequireComponent(typeof(Button))]
	public abstract class ActionButton : MonoBehaviour
	{
		Button _button;

		void Start()
		{
			Init();
		}

		/// <summary>
		/// 콜백 시작 및 초기 업데이트 상태를 설정하려면 콜백 시작 시 호출되어야 함
		/// </summary>
		protected virtual void Init()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(() => OnClick());
			UpdateState();
		}

		/// <summary>
		/// 현재 숨김 및 상호작용 유무
		/// </summary>
		protected void UpdateState()
		{
			gameObject.SetActive(IsVisible());
			_button.interactable = IsInteractable();
		}

		/// <summary>
		/// 현재 버튼이 표시되어있나?
		/// </summary>
		public bool IsVisible()
		{
			return _button.IsActive();
		}

		/// <summary>
		/// 현재 버튼이 상호작용이 가능한가?
		/// </summary>
		public bool IsInteractable()
        {
			return _button.interactable;
        }

		/// <summary>
		/// 클릭 한 번으로 수행할 구체적인 이벤트
		/// </summary>
		public abstract void OnClick();
	}
}