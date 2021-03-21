namespace UDBase.Controllers.LogSystem {
	/// <summary>
	/// ULogger은 ILog를 이용해서 보다 짧은 로깅을 할 수 있게 도와줌 / ILog 사용시 반드시 CreateLogger 필수
	/// 노트:
	/// 보일러플레이트 : 오버라이딩 함수 / 제네릭 : 템플릿 / 복싱 : object값으로 들어가기 위한 형변환 과정에서 일어나는 메모리 이동
	/// 이렇게 보일러플레이트가 많은 이유는 매개변수에 의한 추가할당을 방지하기 위해 존재함
	/// 값 형식에서는 복싱을 피하기 위해 제네릭이 필요함 
	/// </summary>
	public interface ILog {

		ULogger CreateLogger(ILogContext context);

		void Message(ILogContext context, string msg);

		void MessageFormat<T1>(ILogContext context, string msg, T1 arg1);

		void MessageFormat<T1, T2>(ILogContext context, string msg, T1 arg1, T2 arg2);

		void MessageFormat<T1, T2, T3>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3);

		void MessageFormat<T1, T2, T3, T4>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		void MessageFormat<T1, T2, T3, T4, T5>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		void MessageFormat(ILogContext context, string msg, params object[] args);

		void Warning(ILogContext context, string msg);

		void WarningFormat<T1>(ILogContext context, string msg, T1 arg1);

		void WarningFormat<T1, T2>(ILogContext context, string msg, T1 arg1, T2 arg2);

		void WarningFormat<T1, T2, T3>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3);

		void WarningFormat<T1, T2, T3, T4>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		void WarningFormat<T1, T2, T3, T4, T5>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		void WarningFormat(ILogContext context, string msg, params object[] args);

		void Assert(ILogContext context, string msg);

		void AssertFormat<T1>(ILogContext context, string msg, T1 arg1);

		void AssertFormat<T1, T2>(ILogContext context, string msg, T1 arg1, T2 arg2);

		void AssertFormat<T1, T2, T3>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3);

		void AssertFormat<T1, T2, T3, T4>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		void AssertFormat<T1, T2, T3, T4, T5>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		void AssertFormat(ILogContext context, string msg, params object[] args);

		void Error(ILogContext context, string msg);

		void ErrorFormat<T1>(ILogContext context, string msg, T1 arg1);

		void ErrorFormat<T1, T2>(ILogContext context, string msg, T1 arg1, T2 arg2);

		void ErrorFormat<T1, T2, T3>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3);

		void ErrorFormat<T1, T2, T3, T4>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		void ErrorFormat<T1, T2, T3, T4, T5>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		void ErrorFormat(ILogContext context, string msg, params object[] args);

		void Exception(ILogContext context, string msg);

		void ExceptionFormat<T1>(ILogContext context, string msg, T1 arg1);

		void ExceptionFormat<T1, T2>(ILogContext context, string msg, T1 arg1, T2 arg2);

		void ExceptionFormat<T1, T2, T3>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3);

		void ExceptionFormat<T1, T2, T3, T4>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		void ExceptionFormat<T1, T2, T3, T4, T5>(ILogContext context, string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		void ExceptionFormat(ILogContext context, string msg, params object[] args);
	}
}
