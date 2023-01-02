using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace UI
{

    public class InventoryItemUI : MonoBehaviour
    {

        [Header("Components")]
        [SerializeField] private TMP_Text _nameTxt;
        [SerializeField] private TMP_Text _countTxt;
        [SerializeField] private TMP_Text _levelTxt;
        [SerializeField] private TMP_Text _durationTxt;
        [SerializeField] private Image _itemImg;
        private Action _selectAction;
        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(Select);
        }

        public void Set(string name, int count, int level, int dur, Sprite itemSprite, Action selectAction)
        {
            _nameTxt.text = name;
            _countTxt.text = count.ToString();
            _levelTxt.text = level.ToString();
            _durationTxt.text = dur.ToString();
            _itemImg.sprite = itemSprite;
            _selectAction = selectAction;
        }

        private void Select()
        {
        Inventory.InventoryState.Instance.Hide();
        _selectAction?.Invoke();
        }

    }

}