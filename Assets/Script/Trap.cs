using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    public abstract void Activate();
    public abstract void DeActivate();

    public abstract void Hide();
}
