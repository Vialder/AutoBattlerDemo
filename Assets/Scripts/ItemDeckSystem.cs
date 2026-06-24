using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class ItemDeckSystem : MonoBehaviour
{   
    /*
    public SO_InitPlayer initPlayerState;
    private InputObject input;
    private Random r;

    [SerializeField] private List<CardObject> handObjects;
    private List<(Vector3, Quaternion)> objectPosList; 
    
    private Stack<IGameItem> cardsInDeck;
    [SerializeField] private List<IGameItem> cardsInHand;
    private Stack<IGameItem> cardsInDiscardPile;
    
    [SerializeField] private IGameItem currentlySelectedCard;
    [SerializeField] private int currentlySelectedCardIndex;
    
    public TextMeshProUGUI deckCountText;
    public TextMeshProUGUI discardCountText;

    [SerializeField] private int deckCount;
    [SerializeField] private int discardCount;
    [SerializeField] private int playPoints;

    public static event Action OnHandUpdated;
    public static event Action<int> OnDrawCountUpdated;
    public static event Action<int> OnDiscardCountUpdated;
    

    private void Awake()
    {
        StateLogicControl.OnGameLoaded += Initialize;
        StateLogicControl.OnEnterShopStage += OnEnterShopStageHandler;
        StateLogicControl.OnEndTurn += EndTurn;
    }
    
    private void Initialize()
    {
        Debug.Log("Card System Initialized ");
        cardsInDeck = new Stack<IGameItem>();
        cardsInHand = new List<IGameItem>();
        cardsInDiscardPile = new Stack<IGameItem>();
        playPoints = 4;
        objectPosList = new List<(Vector3, Quaternion)>();
        r = new Random();
        { //CREATE DECK
            CreateDeck();
            var deck = cardsInDeck.ToList();
            //SHUFFLE DECK
            for (int n = deck.Count - 1; n > 0; --n)
            {
                var k = r.Next(n+1);
                (deck[n], deck[k]) = (deck[k], deck[n]);
            }
            //ADD CARDS TO DECK
            for (int i = 0; i < deck.Count; i++)
            {
                cardsInDeck.Push(deck[i]);
            }
        }
        { //ADD STARTING CARDS TO HAND
            for (int i = 0; i < 5; i++)
            {   
                //ADD NULL VALUES SO WE CAN ACCESS THE INDEX
                if (cardsInHand.Count < 5)
                {
                    cardsInHand.Add(null);
                }
                var cardTakenFromDeck = cardsInDeck.Pop();
                cardsInHand[i] = cardTakenFromDeck;
            }
        }
    }

    private void CreateDeck()
    {
        AddCardToDeckInitial(new TestItem1(), 10);
        AddCardToDeckInitial(new TestItem2(), 10);
    }

    private void AddCardToDeckInitial(IGameItem item, int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            cardsInDeck.Push(item);
        }
    }
    
    private void OnDisable()
    {
        StateLogicControl.OnEnterShopStage -= OnEnterShopStageHandler;
        StateLogicControl.OnGameLoaded -= Initialize;
        StateLogicControl.OnEndTurn -= EndTurn;
    }
    
    private void OnEnterShopStageHandler()
    {   
        CreateObjectList();
        RefreshHandObjects();
    }

    private void CreateObjectList()
    {
        handObjects = new List<CardObject>();
        foreach (Transform child in transform)
        {
            handObjects.Add(child.GetComponent<CardObject>());
            objectPosList.Add((child.position, child.rotation));
        }
    }
    
    private void RefreshHandObjects()
    {   
        for (var i = 0; i < handObjects.Count; i++)
        {
            //handObjects[i].attachedItem = cardsInHand[i];
            //handObjects[i].RefreshObject();
        }
    }

    public void ButtonReroll()
    {
        if (playPoints < 0) return; 
        Draw5();
        playPoints -= 1;
        RefreshHandObjects();
    }

    private void EndTurn()
    {
        playPoints = 4;
    }

    private void Draw5()
    {
        for (var i = 0; i < cardsInHand.Count; i++)
        {
            var tempCard =  cardsInHand[i];
            cardsInHand[i] = null;
            cardsInDiscardPile.Push(tempCard);
        }
        
        for (var i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInDeck.Count == 0) //IF DECK IS EMPTY, SHUFFLE DISCARD INTO DECK
            {
                //CREATE TEMPORARY LIST FROM DISCARD PILE
                var tempList = new List<IGameItem>();
                while (cardsInDiscardPile.Count > 0)
                {
                    var temp = cardsInDiscardPile.Pop();
                    tempList.Add(temp);
                }

                //SHUFFLE TEMPORARY LIST
                for (var n = tempList.Count - 1; n > 0; --n)
                {
                    var k = r.Next(n + 1);
                    (tempList[n], tempList[k]) = (tempList[k], tempList[n]);
                }

                //ADD TEMPORARY LIST TO DECK
                foreach (var crd in tempList)
                {
                    cardsInDeck.Push(crd);
                }
            }

            var replacementCard = cardsInDeck.Pop();
            cardsInHand[i] = replacementCard;
            currentlySelectedCard = null;
        }
        
    }

    private void LateUpdate()
    {
        deckCount = cardsInDeck.Count;
        discardCount = cardsInDiscardPile.Count;
        OnDrawCountUpdated?.Invoke(discardCount);
        OnDiscardCountUpdated?.Invoke(discardCount);
    }
    */
}

