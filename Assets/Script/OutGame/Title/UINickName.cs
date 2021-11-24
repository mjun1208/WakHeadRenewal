using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WakHead
{
    public class UINickName : MonoBehaviour
    {
        [SerializeField] private InputField _nickNameInputField;
        [SerializeField] private Title _title;

        [SerializeField] private Text _nameConnect;

        private void Start()
        {
            _nickNameInputField.onValueChanged.AddListener(SetPlayerName);
        }

        public void SetPlayerName(string name)
        {
            Global.instance.SetPlayerName(name);
        }

        public void ConfirmedName()
        {
            if (string.IsNullOrWhiteSpace(_nickNameInputField.text))
            {
                return;
            }

            SetPlayerName(_nickNameInputField.text);

            _nameConnect.gameObject.SetActive(true);
            _nameConnect.text = "";
            _nameConnect.transform.localScale = new Vector3(1, 1, 1);
            _nameConnect.transform.localRotation = Quaternion.Euler(Vector3.zero);

            this.gameObject.SetActive(false);

            DOTween.Sequence()
                .Append(_nameConnect.DOText($"{_nickNameInputField.text}으로 접속합니다", 1.5f))
                .Append(_nameConnect.transform.DORotate(new Vector3(0, 0, -720f), 2f, RotateMode.LocalAxisAdd))
                .Append(_nameConnect.transform.DOPunchScale(new Vector3(2, 2, 2), 1f))
                .Append(_nameConnect.transform.DOScale(new Vector3(0, 0, 0), 1f))
                .OnComplete(() => { _title.GoLobby(); });
            // _title.GoLobby();
        }
    }
}