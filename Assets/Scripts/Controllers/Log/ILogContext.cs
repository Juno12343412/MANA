namespace UDBase.Controllers.LogSystem {

	/// <summary>
	/// ILog 메소드를 사용할 인터페이스
	/// 특정 클래스에서 로그가 필요할 경우 이 인터페이스를 상속시키면됨
	/// 정적 메서드에 일반적인 컨텍스트 또는 필요한 로그가 필요한 경우 정적 클래스 인스턴스를 사용할 수 있음.
	/// 또한 ILog에 대한 짧은 호출을 위해 ULogger를 사용할 수 있음.
	/// </summary>
	public interface ILogContext { }
}
