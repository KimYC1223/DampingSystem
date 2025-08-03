using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DampingSystem.Demo
{
    public class DampingSystemCardHandDemo : MonoBehaviour
    {
        struct CardTransformData
        {
            public Vector3 position;
            public float angle;
        }

        [Header("Card")]
        [SerializeField] private DampingSystemCardDemo[] cardPrefab;

        [Header("Panel")]
        [SerializeField] private DampingSystemPanel panel;

        private List<CardTransformData> cardTransformdataList = new();

        private void OnEnable()
        {
            InitField();
        }

        private void InitField()
        {
            panel.Init(SetDampingSystem);

            cardTransformdataList.Clear();
            foreach (var card in cardPrefab)
            {
                var position = card.Rect.anchoredPosition;
                var angle = card.transform.eulerAngles.z > 180 ? card.transform.eulerAngles.z - 360 : card.transform.eulerAngles.z;

                cardTransformdataList.Add(new CardTransformData()
                {
                    position = position,
                    angle = angle
                });
            }
        }

        private void SetDampingSystem(DampingSystemInitialCondition condition)
        {
            for (var cardIndex = 0; cardIndex < cardPrefab.Length; cardIndex++)
            {
                var card = cardPrefab[cardIndex];
                card.Init(condition);
                card.CardIndex = cardIndex;
                card.OnCardDrag += HandleOnDrag;
                card.OnCardPointerDown += HandleOnPointerDown;
                card.OnCardPointerUp += HandleOnPointerUp;
            }
        }

        private void HandleOnDrag(DampingSystemCardDemo card, PointerEventData eventData)
        {
            var sortedCards = cardPrefab.OrderBy(card => card.Rect.anchoredPosition.x).ToList();

            var isDraggingCardFound = false;
            for (var index = 0; index < sortedCards.Count; index++)
            {
                if (isDraggingCardFound == false && sortedCards[index] == card)
                {
                    sortedCards[index].transform.SetAsLastSibling();
                    isDraggingCardFound = true;
                    continue;
                }

                sortedCards[index].SetTargetAngle(cardTransformdataList[index].angle);
                sortedCards[index].SetTargetPosition(cardTransformdataList[index].position);
            }
        }

        private void HandleOnPointerDown(DampingSystemCardDemo card, PointerEventData eventData)
        {
            card.SetTargetAngle(0);
            HandleOnDrag(card, eventData);
        }

        private void HandleOnPointerUp(PointerEventData eventData)
        {
            var sortedCards = cardPrefab.OrderBy(card => card.Rect.anchoredPosition.x).ToList();
            for (var index = 0; index < sortedCards.Count; index++)
            {
                sortedCards[index].SetTargetAngle(cardTransformdataList[index].angle);
                sortedCards[index].SetTargetPosition(cardTransformdataList[index].position);
                sortedCards[index].transform.SetSiblingIndex(index);
            }
        }
    }
}