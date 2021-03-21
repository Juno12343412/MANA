using System;

namespace UDBase.Controllers.EventSystem {

	/// <summary>
	/// IEvent는 System.Action을 기반으로 만든 event system 간소화 버전임
	/// 클래스랑 구조체를 전부 이벤트 객체로 사용할 수 있음
	/// 누수가 안나게 하려면 필요없는 이벤트는 무조건 구독 해제를 해줘야함 (ex : Destroy);
	/// 그리고 구독 해제를 안하고 그냥 Destory로 없에버린다면 나중에 핸들러가 체크할 때 문제가 발생할 수 있으니까 [주의]
	/// </summary>
	public interface IEvent {

		/// <summary>
		/// 구독한 이벤트들을 실행함
		/// </summary>
		void Fire<T>(T arg);

		/// <summary>
		/// 핸들러를 통해 해당 T에 함수들을 구독시킴
		/// </summary>
		void Subscribe<T>(object handler, Action<T> callback);

		/// <summary>
		/// 해당 T에 있는 함수를 구독해제 시킴
		/// </summary>
		void Unsubscribe<T>(Action<T> callback);
	}
}