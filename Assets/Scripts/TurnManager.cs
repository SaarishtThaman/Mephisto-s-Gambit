using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static TurnManager instance;

    public CharacterHealthManager currentTurnCharacter;
    private CardsManager cardsManager;
    public List<CharacterHealthManager> combatants;
    private GameManager gameManager;

    private void Awake() {
        instance = this;
    }

    void Start() {
        cardsManager = CardsManager.instance;
        gameManager = GameManager.instance;
    }

    public void StartBattle() {
        combatants = new List<CharacterHealthManager>();
        EnemyHealthManager[] enemies = GameObject.FindObjectsByType<EnemyHealthManager>(FindObjectsSortMode.None);
        for(int i=0;i<enemies.Length;i++) {
            combatants.Add(enemies[i]);
        }
        combatants.Sort((x,y)=>x.transform.position.x.CompareTo(y.transform.position.x));
        currentTurnCharacter = PlayerHealthManager.instance;
        currentTurnCharacter.PlayTurn();
        cardsManager.SpawnCards();
    }

    public void EndTurn() {
        cardsManager.EndTurn();
        //Play AI Turn
        StartCoroutine(WaitForTurn());
    }

    IEnumerator WaitForTurn() {
        //apply effects to player
        PlayerHealthManager.instance.StatusEffectsApply();
        yield return new WaitForSeconds(1f);
        for(int i=0;i<combatants.Count;i++) {
            if(combatants[i] == null) continue;
            combatants[i].PlayTurn();
            yield return new WaitForSeconds(1.5f);
            combatants[i].StatusEffectsApply();
            yield return new WaitForSeconds(1f);
        }
        //Debug.Log("ended turn draw pile: "+cardsManager.drawPile.Count+" handCards: "+cardsManager.handCards.Count);
        //start again
        gameManager.GameTurnStart();
        currentTurnCharacter = PlayerHealthManager.instance;
        currentTurnCharacter.PlayTurn();
        cardsManager.StartTurn();
        //Debug.Log("started turn draw pile: "+cardsManager.drawPile.Count+" handCards: "+cardsManager.handCards.Count);
    }

    public void CheckForWin() {
        StartCoroutine(CheckForDelayedWin());
    }

    IEnumerator CheckForDelayedWin() {
        yield return new WaitForSeconds(0.45f);
        bool allDead = true;
        for(int i=0;i<combatants.Count;i++) {
            if(combatants[i] != null) {
                allDead = false;break;
            }
        }
        if(allDead) {
            gameManager.WinScreen();
        }
    }

}