using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionBar : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _label;

    private void Update()
    {
        _label.text = (GameManager.Self.Player.UnitStats.Protection * 100).ToString() + "%";
    }
}
