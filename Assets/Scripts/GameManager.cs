using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    private CardsManager cardsManager;
    private TurnManager turnManager;
    public GameObject turnEndBtn;
    public GameObject loseScreen, winScreen, finalWinScreen;
    public GameObject proceedBtn;
    public List<CardPick> allCards;
    public Transform[] pickLocations = new Transform[3];
    public CardDescription pickedCard;

    void Awake() {
        instance = this;
        Application.targetFrameRate = 60;
    }

    void Start() {
        cardsManager = CardsManager.instance;
        turnManager = TurnManager.instance;
    }

    public void GameBattleStart(GameObject startBtn) {
        turnManager.StartBattle();
        Destroy(startBtn);
        turnEndBtn.SetActive(true);
    }

    public void GameTurnEnd() {
        turnManager.EndTurn();
        turnEndBtn.SetActive(false);
    }

    public void GameTurnStart() {
        //Debug.Log("making turn button active again "+turnEndBtn);
        turnEndBtn.SetActive(true);
    }

    void Update() {
        if(Input.GetMouseButtonDown(1)) {
            cardsManager.SetCardInactive();
        }
    }

    public void LoseScreen() {
        SoundPlayer.instance.PlaySound(SoundPlayer.instance.lost);
        loseScreen.SetActive(true);
    }

    void SpawnPickCards() {
        List<int> indexes = new List<int>();
        HashSet<int> uniqueIndexes = new HashSet<int>();
        System.Random random = new System.Random();

        while (uniqueIndexes.Count < 3) {
            int index = random.Next(0, allCards.Count); // Generates a random number between 0 and 9
            uniqueIndexes.Add(index);
        }
        indexes.AddRange(uniqueIndexes);

        for(int i=0;i<3;i++) {
            Instantiate(allCards[indexes[i]],pickLocations[i].position,Quaternion.identity,pickLocations[i]);
        }

    }

    public void Proceed() {
        ConstantData.instance.deck.Add(pickedCard);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void WinScreen() {
        SoundPlayer.instance.PlaySound(SoundPlayer.instance.victory);
        if(SceneManager.GetActiveScene().buildIndex == 6) {
            finalWinScreen.SetActive(true);
            return;
        }
        winScreen.SetActive(true);
        SpawnPickCards();
    }

    public void EnableProceed() {
        proceedBtn.SetActive(true);
    }

    public void DisableProceed() {
        proceedBtn.SetActive(false);
    }

    public void RestartLevel() {
        PlayerHealthManager.instance.Start();
        loseScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}