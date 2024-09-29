using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardsManager : MonoBehaviour {
    public static CardsManager instance;
    public List<Transform> spawnLocations = new List<Transform>();
    public List<CardDescription> drawPile;
    public List<CardDescription> handCards;
    private readonly int handCardSize = 4;
    public CardDescription activeCard;
    public bool isCardActive;
    private CardPlayer cardPlayer;
    private TurnManager turnManager;
    private Vector3 offScreen = new Vector3(1000,1000,0);

    private void Awake() {
        instance = this;
    }

    void Start() {
        activeCard = null;
        isCardActive = false;
        turnManager = TurnManager.instance;
    }

    public void SpawnCards() {
        drawPile = new List<CardDescription>();
        handCards = new List<CardDescription>();
        
        //add all cards to draw pile initially
        for(int i=0;i<ConstantData.instance.deck.Count;i++) {
            CardDescription card = Instantiate(ConstantData.instance.deck[i],offScreen,transform.rotation);
            drawPile.Add(card);
        }

        //draw 4 cards from the draw pile
        for(int i=0;i<handCardSize;i++) {
            //spawn card game object
            Transform parent = spawnLocations[i];
            CardDescription drawCard = drawPile[i];
            drawCard.transform.position = parent.position;
            drawCard.transform.rotation = parent.rotation;
            drawCard.transform.localScale = parent.localScale;
            drawCard.transform.parent = parent;
            handCards.Add(drawCard);
            drawCard.GetComponent<CardInteraction>().ResetCard();
        }
        List<CardDescription> newDrawPile = new List<CardDescription>();
        for(int i=handCardSize;i<drawPile.Count;i++) {
            newDrawPile.Add(drawPile[i]);
        }
        drawPile = newDrawPile;
    }

    public void EndTurn() {
        //Move all cards to draw pile
        SetCardInactive();
        for(int i=0;i<handCards.Count;i++) {
            CardDescription cardToMove = handCards[i];
            cardToMove.transform.parent = null;
            cardToMove.GetComponent<CardInteraction>().Discard();
            drawPile.Add(cardToMove);
        }
        handCards = new List<CardDescription>();
    }

    public void StartTurn() {
        handCards = new List<CardDescription>();
        //move cards to hand
        for(int i=0;i<handCardSize;i++) {
            CardDescription cardToDraw = drawPile[i];
            Transform parent = spawnLocations[i];
            cardToDraw.transform.position = parent.position;
            cardToDraw.transform.rotation = parent.rotation;
            cardToDraw.transform.localScale = parent.localScale;
            cardToDraw.transform.parent = parent;
            cardToDraw.GetComponent<CardInteraction>().ResetCard();
            handCards.Add(cardToDraw);
        }
        List<CardDescription> newDrawPile = new List<CardDescription>();
        for(int i=handCardSize;i<drawPile.Count;i++) {
            newDrawPile.Add(drawPile[i]);
        }
        drawPile = newDrawPile;
    }

    public bool PlayActiveCard(CharacterHealthManager target) {
        if(activeCard == null) return false;
        if(!CardPlayer.CanPlay(activeCard,target)) return false;
        if(activeCard.attackAll) {
            List<CharacterHealthManager> enemies = turnManager.combatants;
            for(int i=0;i<enemies.Count;i++) {
                if(enemies[i] == null) continue;
                CardPlayer.PlayCard(activeCard, enemies[i]);
            }
        } else {
            CardPlayer.PlayCard(activeCard, target);
        }
        //put card into draw pile
        CardDescription cardToMove = activeCard;
        cardToMove.transform.parent = null;
        SetCardInactive();
        cardToMove.GetComponent<CardInteraction>().Discard();

        //check win state with delay
        turnManager.CheckForWin();

        return true;
    }

    public void SetCardActive(CardDescription card) {
        if(activeCard == null) {
            card.GetComponent<CardInteraction>().HoverCard();
            activeCard = card;
            isCardActive = true;
        }
    }

    public void SetCardInactive() {
        if(activeCard == null) return;
        activeCard.GetComponent<CardInteraction>().PopBackCard();
        activeCard = null;
        isCardActive = false;
    }

}