using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationButton : MonoBehaviour
{
    public void ConfirmStep()
    {
        GameManager.Self.ResetAllForNextStep();
    }
}
