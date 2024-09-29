using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    private Vector3 originalSize, finalSize;
    private Vector3 originalPosition, finalPosition, discardPosition;
    private Vector3 targetSize, targetPosition;
    private Quaternion originalRotation, finalRotation, targetRotation;
    private readonly float growSpeed = 5f, moveSpeed = 15f, rotateSpeed = 60f;
    public Canvas canvas;
    public bool isDiscarded = true;

    private CardsManager cardsManager;

    void Start() {
        cardsManager = CardsManager.instance;
        originalSize = transform.localScale;
        finalSize = transform.localScale * 1.25f;
        targetSize = originalSize;
        originalRotation = transform.rotation;
        finalRotation = Quaternion.identity;
        targetRotation = originalRotation;
        originalPosition = transform.position;
        finalPosition = transform.position + 2f * Vector3.up;
        targetPosition = originalPosition;
        discardPosition = Vector3.right * 50f;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (cardsManager.isCardActive || isDiscarded) return;
        HoverCard();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (cardsManager.isCardActive || isDiscarded) return;
        PopBackCard();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) {
            if (cardsManager.isCardActive || isDiscarded) return;
            cardsManager.SetCardActive(GetComponent<CardDescription>());
        }
    }

    public void PopBackCard() {
        targetPosition = originalPosition;
        targetPosition = originalPosition;
        targetSize = originalSize;
        targetRotation = originalRotation;
        canvas.sortingOrder = 0;
        isDiscarded = false;
    }

    public void ResetCard() {
        originalSize = transform.localScale;
        finalSize = transform.localScale * 1.25f;
        targetSize = originalSize;
        originalRotation = transform.rotation;
        finalRotation = Quaternion.identity;
        targetRotation = originalRotation;
        originalPosition = transform.position;
        finalPosition = transform.position + 2.5f * Vector3.up;
        targetPosition = originalPosition;
        targetPosition = originalPosition;
        targetSize = originalSize;
        targetRotation = originalRotation;
        canvas.sortingOrder = 0;
        isDiscarded = false;
    }

    public void HoverCard() {
        targetPosition = finalPosition;
        targetSize = finalSize;
        targetRotation = finalRotation;
        canvas.sortingOrder = 5;
    }

    public void Discard() {
        isDiscarded = true;
        targetPosition = discardPosition;
        targetSize = Vector3.zero;
    }

}