using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Plugins.DialogueSystem.Scripts.UI
{
    public class Answer : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;
        private UnityAction _click;
        public bool IsSelected { get; private set; }

        private void Start()
        {
            button.onClick.AddListener(AnswerSelected);
        }

        public void Show(string text, UnityAction click)
        {
            this.text.text = text;
            _click = click;
            IsSelected = false;
        }

        public void Hide()
        {
            _click = null;
            text.text = "";
        }

        private void AnswerSelected()
        {
            IsSelected = true;
            _click.Invoke();
        }
    }
}