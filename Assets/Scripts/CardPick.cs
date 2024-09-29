using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardPick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public CardDescription cardDescription;
    public static CardPick selectedCard = null;
    Vector3 originalSize, finalSize, targetSize;

    void Start() {
        originalSize = transform.localScale;
        finalSize = transform.localScale * 1.25f;
        targetSize = originalSize;
    }

    void Update() {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, 2f * Time.deltaTime);
        if(selectedCard != null) {
            GameManager.instance.EnableProceed();
        } else {
            GameManager.instance.DisableProceed();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(this == selectedCard) return;
        targetSize = finalSize;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(this == selectedCard) return;
        targetSize = originalSize;
    }

    public void OnPointerClick(PointerEventData eventData) {
        targetSize = finalSize;
        selectedCard = this;
        GameManager.instance.pickedCard = selectedCard.cardDescription;
    }

}