using UnityEngine;
using UI.Core;
using System;

namespace UI.YesNoBuildPopup
{

    public class YesNoBuildPopupState : ScreenState<YesNoBuildPopupState, YesNoBuildPopupView>
    {

        private Transform _objectTr;
        private Action _acceptAction;
        private Action _cancelAction;
        private Action _moveRightAction;
        private Action _moveLeftAction;
        private Action _moveUpAction;
        private Action _moveBottomAction;

        public override void Initialize()
        {
            base.Initialize();

            view.AcceptButton.onClick.AddListener(Accept);
            view.CancelButton.onClick.AddListener(Cancel);
            view.ArrowRightButton.onClick.AddListener(MoveRight);
            view.ArrowLeftButton.onClick.AddListener(MoveLeft);
            view.ArrowUpButton.onClick.AddListener(MoveUp);
            view.ArrowBottomButton.onClick.AddListener(MoveBottom);
        }

        private void Update() => UpdatePopupPosition();

        private void UpdatePopupPosition()
        {
            if (!_objectTr) return;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _objectTr.position);
            view.PopupRoot.anchoredPosition = screenPoint - view.CanvasRoot.sizeDelta / 2f;
        }

        private void Show(Transform objectTr, Action acceptAction, Action cancelAction, Action moveRightAction, Action moveLeftAction, Action moveUpAction, Action moveBottomAction)
        {
            base.Show();
            _objectTr = objectTr;
            _acceptAction = acceptAction;
            _cancelAction = cancelAction;
            _moveRightAction = moveRightAction;
            _moveLeftAction = moveLeftAction;
            _moveUpAction = moveUpAction;
            _moveBottomAction = moveBottomAction;
        }

        private void Accept()
        {
            Hide();
            _acceptAction?.Invoke();
        }
        private void Cancel()
        {
            Hide();
            _cancelAction?.Invoke();
        }

        private void MoveRight()
        {
            _moveRightAction?.Invoke();
        }
        private void MoveLeft()
        {
            _moveLeftAction?.Invoke();
        }
        private void MoveUp()
        {
            _moveUpAction?.Invoke();
        }
        private void MoveBottom()
        {
            _moveBottomAction?.Invoke();
        }

    }

}