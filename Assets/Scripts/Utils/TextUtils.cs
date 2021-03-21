using System;
using System.Text;

namespace UDBase.Utils {

	/// <summary>
	/// 텍스트 처리를 위한 유틸리티
	/// </summary>
	public static class TextUtils {

		/// <summary>
		/// 값이 null인 경우 지정된 값 또는 null이 아닌 빈 문자열을 반환하는 함수
		/// </summary>
		public static string EnsureString(string value) {
			return value != null ? value : ""; 
		}

		/// <summary>
		/// 지정된 문자열에서 모든 공백 및 제어 문자 제거하는 함수
		/// </summary>
		public static string RemoveWhitespaces(string str) {
			var sb = new StringBuilder(str.Length);
			foreach ( var c in str ) {
				if ( char.IsWhiteSpace(c) || char.IsControl(c) ) {
					continue;
				}
				sb.Append(c);
			}
			return sb.ToString();
		}

		/// <summary>
		/// 지정된 문자열이 내부에 모든 공백과 제어 차트가 없는 경우 동일함 확인하는 함수
		/// </summary>
		public static bool EqualsIgnoreWhitespaces(
			string leftStr, string rightStr,
			StringComparison comparison = StringComparison.Ordinal) {
			if ( (leftStr == null) || (rightStr == null) ) {
				return false;
			}
			var normLeft  = RemoveWhitespaces(leftStr );
			var normRight = RemoveWhitespaces(rightStr);
			return normLeft.Equals(normRight, comparison);
		}

		/// <summary>
		/// 트리밍 : 공백 제거
		/// 지정된 문자열에서 모든 작은따옴표 및 큰따옴표 트리밍하는 함수
		/// </summary>
		public static string TrimQuotes(string text) {
			return text?.Trim('\"', '\'');
		}

		/// <summary>
		/// 트리밍 : 공백 제거
		/// 파일 내용 시작 시 모든 시작/끝 빈 문자 및 가능한 바이트 순서 표시 트리밍
		/// https://en.wikipedia.org/wiki/Byte_order_mark
		/// </summary>
		public static string TrimFileContent(string text) {
			return text?.Trim().TrimStart('\uFEFF');
		}
	}

	/// <summary>
	/// ToString()이 호출될 때만 호출되는 문자열 함수를 보유할 유틸리티 클래스 (상속해서 사용)
	/// 할당 안전 로그 호출에 유용.
	/// </summary>
	public class StringFunctor {
		Func<string> _func;

		public StringFunctor(Func<string> func) {
			_func = func;
		}

		public override string ToString() {
			return _func();
		}
	}
}
