using System;
using UnityEngine;

namespace UDBase.Utils {

    /// <summary>
    /// 키 누르기 종류
    /// </summary>
    public enum KeyKind
    {
        Press,
        Down,
        Up,
        NONE = 99
    }

    /// <summary>
    /// 키 셋팅 클래스
    /// </summary>
    public class KeySetting
    {
        /// <summary>
        /// 키 코드 값
        /// </summary>
        public KeyCode KeyValue { get; set; }
        
        /// <summary>
        /// 해당 키 실행 시 실행할 콜백 값
        /// </summary>
        public Action  Func     { get; set; }

        /// <summary>
        /// Press, Down, Up 중에서 고르기
        /// </summary>
        public KeyKind Kind     { get; set; }
        
        public KeySetting (KeyCode key, Action func, KeyKind kind = KeyKind.Press) {
            KeyValue = key;
            Func     = func;
            Kind     = kind;
        }

        /// <summary>
        /// 현재 키 값을 눌렀는지 확인하는 함수
        /// </summary>
        public bool Update() {
            
            switch (Kind)
            {
                case KeyKind.Press:
                    if (Input.GetKey(KeyValue))
                    {
                        Func();
                        return true;
                    }
                    return false;
                case KeyKind.Down:
                    if (Input.GetKeyDown(KeyValue))
                    {
                        Func();
                        return true;
                    }
                    return false;
                case KeyKind.Up:
                    if (Input.GetKeyUp(KeyValue))
                    {
                        Func();
                        return true;
                    }
                    return false;
            }
            return false;
        }
    }
}