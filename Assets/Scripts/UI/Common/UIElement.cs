using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using Zenject;

namespace UDBase.UI.Common {
    
    /// <summary>
    /// UIElement는 캔버스에 있는 UI들을 Show/Hide 해주는 기능을 담당함
    /// 버튼을 상호작용하는 기능도 사용하려면 CanvasGroup을 넣어줘야함
    /// </summary>
    public class UIElement : MonoBehaviour {
        internal static HashSet<UIElement> Instances = new HashSet<UIElement>();

        /// <summary>
        /// UI Element 상태들
        /// </summary>
        public enum UIElementState {
            Showing, Shown,
            Hiding, Hidden,
            None = 99
        }

        /// <summary>
        /// 객체를 인스턴스화 할 때 보여줄지 여부
        /// </summary>
        [Tooltip("객체를 인스턴스화 할 때 보여줄지 여부")]
        public bool AutoShow = true;

        /// <summary>
        /// 객체를 처음으로 생성했을 때 상호작용을 가능하게 할건가 ?
        /// </summary>
        [Tooltip("객체를 처음으로 생성했을 때 상호작용을 가능하게 할건가 ?")]
        public bool InitialActive = true;

        /// <summary>
        /// 객체를 숨길 때 비활성화를 할건가 ?
        /// </summary>
        [Tooltip("객체를 숨길 때 비활성화를 할건가 ?")]
        public bool DisableOnHide = true;

        /// <summary>
        /// 이 옵션을 선택하면 부모 객체가 표시된 후 자식 객체가 표시됩니다
        /// 그리고 부모 객체가 숨기기 전에 모든 자식 객체가 숨겨집니다
        /// </summary>
        [Tooltip(
            @"이 옵션을 선택하면 부모 객체가 표시된 후 자식 객체가 표시됩니다
            그리고 부모 객체가 숨기기 전에 모든 자식 객체가 숨겨집니다")]
        public bool Ordered;

        /// <summary>
        /// 선택 사항, 지정된 그룹과 모든 객체와 상호 작용할 수 있도록 설정할 수 있습니다
        /// </summary>
        [Tooltip("선택 사항, 지정된 그룹과 모든 객체와 상호 작용할 수 있도록 설정할 수 있습니다")]
        public string Group;

        /// <summary>
        /// 자식 객체들
        /// </summary>
        [Tooltip("자식 객체들")]
        public List<UIElement> Childs = new List<UIElement>();

        /// <summary>
        /// 자식 객체가 존재하는가 ?
        /// </summary>
        public bool HasChilds { 
            get {
                return Childs.Count > 0;
            }
        }

        /// <summary>
        /// 부모객체가 존재하는가 ?
        /// </summary>
        public bool HasParent {
            get {
                return _parent;
            }
        }

        /// <summary>
        /// 현재 UI객체 상태
        /// </summary>
        /// <value>The state.</value>
        public UIElementState State { get; private set; }

        /// <summary>
        /// 이 객체를 상호 작용할 수 있는지 여부를 나타내는 값을 가져오거나 설정하는 프로퍼티
        /// </summary>
        public bool IsInteractable { 
            get {
                return _isInteractable;
            }
            set {
                _isInteractable = value;
                UpdateInteractable(_isInteractable);
                if ( HasChilds ) { 
                    for ( int i = 0; i < Childs.Count; i++ ) {
                        Childs[i].IsInteractable = _isInteractable;
                    } 
                }
            }
        }

        bool _isInteractable = false;
        bool _groupChecked   = false;
        CanvasGroup _group   = null;
        UIElement _parent    = null;

        IEvent _events;

        /// <summary>
        /// 중속성 있는 Init
        /// </summary>
        [Inject]
        public void Init(IEvent events)
        {
            _events = events;
            if ( HasChilds ) { 
                for ( int i = 0; i < Childs.Count; i++ ) {
                    Childs[i].SetParent(this);
                }
            }

            if (!HasParent) {
                IsInteractable = InitialActive;
                if (AutoShow) {
                    Show();
                } else {
                    State = UIElementState.Hidden;
                }
            }
            Instances.Add(this);
            _events?.Subscribe<UI_ElementHidden>(this, OnElementHidden);
        }

        bool IsChild(UIElement element) {
            return Childs.Contains(element);
        }

        bool IsAllChildsHidden() {
            for (int i = 0; i < Childs.Count; i++) {
                if (Childs[i].State != UIElementState.Hidden) {
                    return false;
                }
            }
            return true;
        }

        void OnElementHidden(UI_ElementHidden e) {
            if (Ordered && IsChild(e.Element) && IsAllChildsHidden()) {
                PerformHide();
            }
        }

        void SetParent(UIElement parent) {
            _parent = parent;
        }

        void OnEnable() {
            //Instances.Add(this);
            _events?.Subscribe<UI_ElementHidden>(this, OnElementHidden);
        }

        void OnDisable() {
            //Instances.Remove(this);
            _events?.Unsubscribe<UI_ElementHidden>(OnElementHidden);
        }

        void OnDestroy() {
            Instances.Remove(this);
        }

        public bool CanShow() {
            return
                (State != UIElementState.Showing) &&
                (State != UIElementState.Shown);
        }

        void SetHidden() {
            if (HasChilds) {
                for (int i = 0; i < Childs.Count; i++) {
                    Childs[i].SetHidden();
                }
            }
        }

        void SetShown() {
            if (HasChilds) {
                for (int i = 0; i < Childs.Count; i++) {
                    Childs[i].SetShown();
                }
            }
        }

        /// <summary>
		/// 객체 보여주기
		/// </summary>
		[ContextMenu("Show")]
        public void Show()
        {
            State = UIElementState.Showing;
            gameObject.SetActive(true);
            SetHidden();
            OnShowComplete();

            if (HasChilds) {
                if (!Ordered) {
                    for (int i = 0; i < Childs.Count; i++) {
                        Childs[i].Show();
                    }
                }
            }
        }

        void OnShowComplete() {
            State = UIElementState.Shown;
            _events.Fire(new UI_ElementShown(this));
            if (Ordered) {
                for (int i = 0; i < Childs.Count; i++) {
                    Childs[i].Show();
                }
            }
        }

        public bool CanHide() {
            return
                (State != UIElementState.Hiding) &&
                (State != UIElementState.Hidden);
        }

        /// <summary>
		/// 객체 숨기기
		/// </summary>
		[ContextMenu("Hide")]
        public void Hide() {
            State = UIElementState.Hiding;
            SetShown();
            if (!Ordered) {
                PerformHide();
            }
            if (HasChilds) {
                for (int i = 0; i < Childs.Count; i++) {
                    Childs[i].Hide();
                }
            } else {
                PerformHide();
            }
        }

        void PerformHide()
        {
            /// ...
            OnHideComplete();
        }

        void OnHideComplete() {
            if (DisableOnHide) {
                gameObject.SetActive(false);
            }
            State = UIElementState.Hidden;
            _events.Fire(new UI_ElementHidden(this));
        }

        /// <summary>
		/// 객체 상호작용 켜기
		/// </summary>
		[ContextMenu("Activate")]
        public void Activate() {
            IsInteractable = true;
        }

        /// <summary>
        /// 객체 상호작용 끄기
        /// </summary>
        [ContextMenu("Deactivate")]
        public void Deactivate() {
            IsInteractable = false;
        }

        void UpdateInteractable(bool isInteractable) {
            if (!_groupChecked) {
                _group = GetComponent<CanvasGroup>();
                _groupChecked = true;
            }
            if (_group) {
                _group.interactable = isInteractable;
            }
        }

        /// <summary>
        /// 객체의 상태를 반대로 돌림
        /// </summary>
        public void Switch()
        {
            switch (State)
            {
                case UIElementState.None:
                case UIElementState.Shown:
                    Hide();
                    break;

                case UIElementState.Hidden:
                    Show();
                    break;
            }
        }
    }
}
