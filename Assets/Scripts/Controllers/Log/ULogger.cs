namespace UDBase.Controllers.LogSystem {
	/// <summary>
	/// ULogger은 ILog를 이용해서 보다 짧은 로깅을 할 수 있게 도와줌 / ILog 사용시 반드시 CreateLogger 필수
	/// 노트:
	/// 보일러플레이트 : 오버라이딩 함수 / 제네릭 : 템플릿 / 복싱 : object값으로 들어가기 위한 형변환 과정에서 일어나는 메모리 이동
	/// 이렇게 보일러플레이트가 많은 이유는 매개변수에 의한 추가할당을 방지하기 위해 존재함
	/// 값 형식에서는 복싱을 피하기 위해 제네릭이 필요함 
	/// </summary>
	public struct ULogger {
		readonly ILog        _log;
		readonly ILogContext _context;

		public ULogger(ILog log, ILogContext context) {
			_log     = log;
			_context = context;
		}

		public void Message(string msg) {
			_log.Message(_context, msg);
		}

		public void MessageFormat<T1>(string msg, T1 arg1) {
			_log.MessageFormat(_context, msg, arg1);
		}

		public void MessageFormat<T1, T2>(string msg, T1 arg1, T2 arg2) {
			_log.MessageFormat(_context, msg, arg1, arg2);
		}

		public void MessageFormat<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) {
			_log.MessageFormat(_context, msg, arg1, arg2, arg3);
		}

		public void MessageFormat<T1, T2, T3, T4>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			_log.MessageFormat(_context, msg, arg1, arg2, arg3, arg4);
		}

		public void MessageFormat<T1, T2, T3, T4, T5>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			_log.MessageFormat(_context, msg, arg1, arg2, arg3, arg4, arg5);
		}

		public void MessageFormat(string msg, params object[] args) {
			_log.MessageFormat(_context, msg, args);
		}

		public void Warning(string msg) {
			_log.Warning(_context, msg);
		}

		public void WarningFormat<T1>(string msg, T1 arg1) {
			_log.WarningFormat(_context, msg, arg1);
		}

		public void WarningFormat<T1, T2>(string msg, T1 arg1, T2 arg2) {
			_log.WarningFormat(_context, msg, arg1, arg2);
		}

		public void WarningFormat<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) {
			_log.WarningFormat(_context, msg, arg1, arg2, arg3);
		}

		public void WarningFormat<T1, T2, T3, T4>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			_log.WarningFormat(_context, msg, arg1, arg2, arg3, arg4);
		}

		public void WarningFormat<T1, T2, T3, T4, T5>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			_log.WarningFormat(_context, msg, arg1, arg2, arg3, arg4, arg5);
		}


		public void WarningFormat(string msg, params object[] args) {
			_log.WarningFormat(_context, msg, args);
		}

		public void Assert(string msg) {
			_log.Assert(_context, msg);
		}

		public void AssertFormat<T1>(string msg, T1 arg1) {
			_log.AssertFormat(_context, msg, arg1);
		}

		public void AssertFormat<T1, T2>(string msg, T1 arg1, T2 arg2) {
			_log.AssertFormat(_context, msg, arg1, arg2);
		}

		public void AssertFormat<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) {
			_log.AssertFormat(_context, msg, arg1, arg2, arg3);
		}

		public void AssertFormat<T1, T2, T3, T4>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			_log.AssertFormat(_context, msg, arg1, arg2, arg3, arg4);
		}

		public void AssertFormat<T1, T2, T3, T4, T5>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			_log.AssertFormat(_context, msg, arg1, arg2, arg3, arg4, arg5);
		}

		public void AssertFormat(string msg, params object[] args) {
			_log.AssertFormat(_context, msg, args);
		}

		public void Error(string msg) {
			_log.Error(_context, msg);
		}

		public void ErrorFormat<T1>(string msg, T1 arg1) {
			_log.ErrorFormat(_context, msg, arg1);
		}

		public void ErrorFormat<T1, T2>(string msg, T1 arg1, T2 arg2) {
			_log.ErrorFormat(_context, msg, arg1, arg2);
		}

		public void ErrorFormat<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) {
			_log.ErrorFormat(_context, msg, arg1, arg2, arg3);
		}

		public void ErrorFormat<T1, T2, T3, T4>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			_log.ErrorFormat(_context, msg, arg1, arg2, arg3, arg4);
		}

		public void ErrorFormat<T1, T2, T3, T4, T5>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			_log.ErrorFormat(_context, msg, arg1, arg2, arg3, arg4, arg5);
		}

		public void ErrorFormat(string msg, params object[] args) {
			_log.ErrorFormat(_context, msg, args);
		}

		public void Exception(string msg) {
			_log.Exception(_context, msg);
		}

		public void ExceptionFormat<T1>(string msg, T1 arg1) {
			_log.ExceptionFormat(_context, msg, arg1);
		}

		public void ExceptionFormat<T1, T2>(string msg, T1 arg1, T2 arg2) {
			_log.ExceptionFormat(_context, msg, arg1, arg2);
		}

		public void ExceptionFormat<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) {
			_log.ExceptionFormat(_context, msg, arg1, arg2, arg3);
		}

		public void ExceptionFormat<T1, T2, T3, T4>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
			_log.ExceptionFormat(_context, msg, arg1, arg2, arg3, arg4);
		}

		public void ExceptionFormat<T1, T2, T3, T4, T5>(string msg, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
			_log.ExceptionFormat(_context, msg, arg1, arg2, arg3, arg4, arg5);
		}

		public void ExceptionFormat(string msg, params object[] args) {
			_log.ExceptionFormat(_context, msg, args);
		}
	}
}
