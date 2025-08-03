using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DampingSystem.Demo
{
    public class DampingSystemCardDemo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public RectTransform Rect => rectTransform;
        public Vector2 RectHalfSize => canvas.rect.size * 0.5f;

        public event Action<DampingSystemCardDemo, PointerEventData> OnCardDrag;
        public event Action<DampingSystemCardDemo, PointerEventData> OnCardPointerDown;
        public event Action<PointerEventData> OnCardPointerUp;

        internal int CardIndex;

        [Header("Canvas")]
        [SerializeField] private RectTransform canvas;

        private const float AngleThreshold = 50f;
        private const float MovementAngle = 10f;

        private DampingSystemVector3 positionDampingSystem;
        private DampingSystemFloat rotationDampingSystem;
        private RectTransform rectTransform;
        private Vector3 targetPosition;
        private float targetAngle;
        private bool isInitialized;

        public void Init(DampingSystemInitialCondition dampingSystemInitialCondition)
        {
            var position = rectTransform.anchoredPosition;
            var angle = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;

            positionDampingSystem = new DampingSystemVector3(
                dampingSystemInitialCondition.Frequency,
                dampingSystemInitialCondition.DampingRatio,
                dampingSystemInitialCondition.InitialResponse,
                position);
            SetTargetPosition(position);

            rotationDampingSystem = new DampingSystemFloat(
                dampingSystemInitialCondition.Frequency,
                dampingSystemInitialCondition.DampingRatio,
                dampingSystemInitialCondition.InitialResponse,
                angle);
            SetTargetAngle(angle);

            isInitialized = true;
        }

        public void SetTargetPosition(Vector2 position) => targetPosition = position;
        public void SetTargetAngle(float angle) => targetAngle = angle;

        public void OnDrag(PointerEventData eventData)
        {
            targetPosition = eventData.position / canvas.localScale - RectHalfSize;
            OnCardDrag?.Invoke(this, eventData);
        }

        public void OnPointerDown(PointerEventData eventData) => OnCardPointerDown?.Invoke(this, eventData);
        public void OnPointerUp(PointerEventData eventData) => OnCardPointerUp?.Invoke(eventData);

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            isInitialized = false;
        }

        private void Update()
        {
            if (isInitialized == false)
                return;

            rectTransform.anchoredPosition = positionDampingSystem.Calculate(targetPosition);

            var calculatedAngle = rotationDampingSystem.Calculate(rotationDampingSystem.Calculate((rectTransform.anchoredPosition.x - targetPosition.x) switch
            {
                > AngleThreshold => MovementAngle,
                < -AngleThreshold => -MovementAngle,
                _ => targetAngle
            }));

            rectTransform.eulerAngles = new Vector3(0f, 0f, calculatedAngle);
        }
    }
}