using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAssistant : MonoBehaviour
{
    [SerializeField]
    private Dice[] _dices;
    [SerializeField]
    private Button _confirmationButton;

    private SelectionManager _selectionManager;

    private void Start()
    {
        _selectionManager = GameManager.Self.SelectionManager;
    }

    private void Update()
    {
        bool _isGood = true;
        foreach (Dice dice in _dices)
        {
            if (_selectionManager.SelectedHex == null || dice.transform.parent == dice.DiceHubPanel.transform || dice.transform.parent == dice.DicePanel.transform)
                _isGood = false;
        }
        _confirmationButton.gameObject.SetActive(_isGood);
    }

    public void ResetDicePlace()
    {
        foreach (Dice dice in _dices)
        {
            dice.ResetPlace();
        }
    }
}
