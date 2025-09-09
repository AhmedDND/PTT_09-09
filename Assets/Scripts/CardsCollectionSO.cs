using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Collection", menuName = "Card Prototype/Card Collection")]
public class CardsCollectionSO : ScriptableObject
{
    public List<CardSO> cards;
}
