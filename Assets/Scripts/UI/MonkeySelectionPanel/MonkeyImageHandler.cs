using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ServiceLocator.UI
{
    public class MonkeyImageHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        private Image monkeyImage;
        private MonkeyCellController owner;
        private Sprite spriteToSet;
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector3 OgPosition, OgAnchoredPosition;

        private void Awake()
        {
            monkeyImage = GetComponent<Image>();
            monkeyImage.sprite = spriteToSet;
            rectTransform = GetComponent<RectTransform>();
            canvas = FindAnyObjectByType<Canvas>();
            OgAnchoredPosition = rectTransform.anchoredPosition;
            OgPosition = rectTransform.position;
        }

        public void ConfigureImageHandler(Sprite spriteToSet, MonkeyCellController owner)
        {
            this.spriteToSet = spriteToSet;
            this.owner = owner;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            owner.MonkeyDraggedAt(rectTransform.position);
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            owner.MonkeyDroppedAt(rectTransform.position);
            ResetMonkey();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            monkeyImage.color = new Color(1, 1, 1, 0.5f);
        }

        private void ResetMonkey()
        {
            monkeyImage.color = new Color(1, 1, 1, 1f);
            rectTransform.position = OgPosition;
            rectTransform.anchoredPosition = OgAnchoredPosition;
            GetComponent<LayoutElement>().enabled = false;
            GetComponent<LayoutElement>().enabled = true;
        }
        
    }
}