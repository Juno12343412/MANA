namespace UDBase.UI.Common {

	/// <summary>
	/// 객체가 표시될 때 발생하는 이벤트
	/// </summary>
	public struct UI_ElementShown {
		public UIElement Element { get; private set; }

		public UI_ElementShown(UIElement element) {
			Element = element;
		}
	}

	/// <summary>
	/// 객체가 꺼질 때 발생하는 이벤트
	/// </summary>
	public struct UI_ElementHidden {
		public UIElement Element { get; private set; }

		public UI_ElementHidden(UIElement element) {
			Element = element;
		}
	}
}