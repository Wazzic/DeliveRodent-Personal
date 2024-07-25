using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGridManager : MonoBehaviour
{
    public List<LobbyPlayerCard> playerCards;
    [SerializeField] private GameObject playerUICard;

    [SerializeField] List<RectTransform> xToJoins;

    GridLayoutGroup gridGroup;
    public void Awake()
    {
        playerCards = new List<LobbyPlayerCard>();
        gridGroup = GetComponent<GridLayoutGroup>();

        
    }
    public LobbyPlayerCard AddPlayerCard()
    {
        //Debug.Log("AddPlayerCard");
        LobbyPlayerCard newCard = Instantiate(playerUICard).GetComponent<LobbyPlayerCard>();
        RectTransform newCardTrans = newCard.GetComponent<RectTransform>();
        newCardTrans.SetParent(this.transform);
        newCardTrans.SetSiblingIndex(playerCards.Count);
        playerCards.Add(newCard);
        //playerCards[playerCards.Count - 1].playerCardController = 
        //ResizeGrid();
        return newCard;
    }

    public LobbyPlayerCard AddPlayerCard(LobbyPlayerCard newCard)
    {
        //Debug.Log("AddPlayerCard");
        RectTransform newCardTrans = newCard.GetComponent<RectTransform>();
        newCardTrans.SetParent(this.transform);
        newCardTrans.SetSiblingIndex(playerCards.Count);
        playerCards.Add(newCard);
        //playerCards[playerCards.Count - 1].playerCardController = 
        //ResizeGrid();
        return newCard;
    }

    public void RemovePlayerCard(LobbyPlayerCard card)
    { 
        playerCards.Remove(card);
        //ResizeGrid();
    }
    public void ResizeGrid()
    {
        var rectTran = GetComponent<RectTransform>();
        Vector2 panelSize = new Vector2(rectTran.rect.width, rectTran.rect.height);
        //Debug.Log(screenSize);
        panelSize.x -= gridGroup.padding.left + gridGroup.padding.right;
        panelSize.y -= gridGroup.padding.top + gridGroup.padding.bottom;
        
        Vector2 rowCol = Vector2.one;
        int rows = 1;
        int cols = 1;
        if (playerCards.Count > 1)
        {
            rows = 2;
        }
        if (playerCards.Count > 2)
        {
            cols = 2;
        }

        Vector2 cellSize = new Vector2(panelSize.x / rows, panelSize.y / cols);
        gridGroup.cellSize = cellSize;
        // + (gridGroup.padding.left + gridGroup.padding.right) * Vector2.right;
        //gridGroup.cellSize = (panelSize - 2 * (Vector2.right * gridGroup.padding.right + Vector2.up * gridGroup.padding.top)) / (playerCards.Count);
        //gridGroup.cellSize = new Vector2(100, 500);
        foreach (LobbyPlayerCard card in playerCards)
        {
            card.ResizeCardComps(cellSize, playerCards.Count);
        }
    }
    public void HideJoinPanel()
    {
        xToJoins[playerCards.Count].gameObject.SetActive(false);
    }
    public void ShowJoinPanel()
    {
        xToJoins[playerCards.Count].gameObject.SetActive(true);
    }
}
