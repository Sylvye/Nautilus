using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerAbilityHandler : MonoBehaviour
{
    public List<GameObject> objs; // replace with an "ability"/"item" object
    public int currentInvIndex;

    public int launchPower; // eventually, have each item have a custom launch power, see above

    private Launcher objLauncher;
    private Transform spawned;

    private void Awake()
    {
        objLauncher = GetComponent<Launcher>();
        spawned = GameObject.Find("Spawned").transform;
    }

    public void Fire()
    {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        objLauncher.Launch(objs[currentInvIndex], dir.normalized, launchPower, 1); // TEMPORARY!!! MAKE EACH ITEM HAVE A DESIGNATED TARGET PARENT
    }

    public void NextItem()
    {
        currentInvIndex = ++currentInvIndex % objs.Count;
    }

    public void PrevItem()
    {
        currentInvIndex = (--currentInvIndex % objs.Count + objs.Count) % objs.Count;
    }
}
