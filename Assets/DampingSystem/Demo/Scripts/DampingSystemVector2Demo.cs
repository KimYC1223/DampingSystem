using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DampingSystem.Demo
{
    public class DampingSystemVector2Demo : MonoBehaviour, IPointerClickHandler
    {

        [Header("Target")]
        [SerializeField] private RectTransform target;

        [Header("Panel")]
        [SerializeField] private DampingSystemPanel panel;

        [Header("Canvas")]
        [SerializeField] private RectTransform canvas;
        
        [Header("Cursor Effect")]
        [SerializeField] private RectTransform cursorImage;
        [SerializeField] private Animator cursorEffectAnimator;
        [SerializeField] private string cursorEffectParamName;

        private DampingSystemVector2 dampingSystem;
        private Vector2 targetPosition;
        private int cursorEffectParamHash;

        private void OnEnable()
        {
            InitTargetPosition();
            InitField();
        }

        private void InitTargetPosition()
        {
            var rectTransform = GetComponent<RectTransform>();
            var screenCenter = new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);

            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.zero;
            target.anchoredPosition = screenCenter;

            cursorImage.anchorMin = Vector2.zero;
            cursorImage.anchorMax = Vector2.zero;
            cursorImage.anchoredPosition = screenCenter;
            cursorImage.gameObject.SetActive(false);

            targetPosition = target.anchoredPosition;
        }

        private void InitField()
        {
            panel.Init(SetDampingSystem);

            cursorEffectParamHash = Animator.StringToHash(cursorEffectParamName);
            cursorImage.gameObject.SetActive(false);
        }

        private void SetDampingSystem(DampingSystemInitialCondition condition)
        {
            dampingSystem = new DampingSystemVector2(
                frequency: condition.Frequency,
                dampingRatio: condition.DampingRatio,
                initialResponse: condition.InitialResponse,
                initialCondition: target.anchoredPosition);
        }

        private void Update()
        {
            if (dampingSystem is null)
                return;

            target.anchoredPosition = dampingSystem.Calculate(targetPosition);
        }

        private void DrawCursorEffect()
        {
            cursorImage.gameObject.SetActive(true);
            cursorImage.anchoredPosition = targetPosition;
            cursorEffectAnimator.SetTrigger(cursorEffectParamHash);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count == 0 || results[0].gameObject != gameObject)
                return;

            Debug.Log(eventData.position);
            targetPosition = eventData.position / canvas.localScale;
            Debug.Log(targetPosition);

            DrawCursorEffect();
        }
    }
}