using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;
public class DeckSystem : MonoBehaviour
{
    public SO_InitPlayer initPlayerState;
    private Random r;
    
    private Stack<IGameItem> cardsInDeck;
    private List<IGameItem> cardsInHand;
    private Stack<IGameItem> cardsInDiscardPile;
    private List<IGameItem> cardsInPlay;
    
    [SerializeField] private int deckCount;
    [SerializeField] private int discardCount;
    [SerializeField] private int handCount;
    [SerializeField] private int playCount;
    [SerializeField] private int totalCount;

    private bool isFirstTime = true;

    public static event Action<IGameItem[]> OnHandUpdated;

    
    private void Awake()
    {
        StateLogicControl.OnGameLoaded += Initialize;
        StateLogicControl.OnReRoll += OnReRollHandler;
        StateLogicControl.OnEnterShopState += EnterShopStateHandler;
        StateLogicControl.OnEnterCombatState += OnEnterCombatStateHandler;
        ViewManager.OnItemAdded += AddItemHandler;
    }

    private void OnDisable()
    {
        StateLogicControl.OnGameLoaded -= Initialize;
        StateLogicControl.OnReRoll -= OnReRollHandler;
        StateLogicControl.OnEnterShopState -= EnterShopStateHandler;
        StateLogicControl.OnEnterCombatState -= OnEnterCombatStateHandler;
        ViewManager.OnItemAdded -= AddItemHandler;
    }
    
    private void Initialize()
    {
        Debug.Log("Card System Initialized ");
        cardsInDeck = new Stack<IGameItem>();
        cardsInHand = new List<IGameItem>();
        cardsInDiscardPile = new Stack<IGameItem>();
        cardsInPlay = new List<IGameItem>
        {
            null,
            null,
            null,
            null,
            null
        };
        
        r = new Random();
        { //CREATE DECK
            CreateDeck();
            var deck = cardsInDeck.ToList();
            cardsInDeck.Clear();
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
        
        OnHandUpdated?.Invoke(cardsInHand.ToArray());
    }
    
    private void CreateDeck()
    {
        AddCardToDeckInitial(new TestItem1(), 5);
        AddCardToDeckInitial(new TestItem2(), 5);
        AddCardToDeckInitial(new Firecracker(), 5);
    }

    private void AddCardToDeckInitial(IGameItem item, int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            cardsInDeck.Push(item);
        }
    }

    private void OnReRollHandler()
    {   
        Debug.Log("Card System OnReRollHandler");
        Draw5();
        OnHandUpdated?.Invoke(cardsInHand.ToArray());
    }

    private void OnEnterCombatStateHandler()
    {
        isFirstTime = false;
    }
    
    private void EnterShopStateHandler()
    {
        if (isFirstTime) return;
        Draw5();
        OnHandUpdated?.Invoke(cardsInHand.ToArray());
    }

    private void AddItemHandler(int fromIndex, int toIndex)
    {
        var temp = cardsInHand[fromIndex];
        cardsInHand[fromIndex] = null;
        cardsInPlay[toIndex] = temp;
    }
    
    private void Draw5()
    {
        for (var i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInHand[i] == null) continue;
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
        }
        
    }

    private void LateUpdate()
    {
        if (cardsInDeck == null || cardsInDiscardPile == null || cardsInHand == null) return;
        deckCount = cardsInDeck.Count;
        discardCount = cardsInDiscardPile.Count;
        handCount = cardsInHand.Count(i => i != null);
        playCount = cardsInPlay.Count(i => i != null);
        totalCount = deckCount + discardCount + handCount + playCount;
    }
}
