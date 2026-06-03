using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BigasManager))]
public class BigasAi : MonoBehaviour
{
    private BigasManager _bigas;
    private readonly float SecondsBetweenAction = 3;
    private readonly float SecondsOfIdle = 1;
    private int _actionIndex = 0;
    private bool _canChangeAction = true;
    void Start() 
    { 
        _bigas = GetComponent<BigasManager>();
    }

    private void Update()
    {
        if (_canChangeAction) StartCoroutine(ChangeAction());
    }

    private IEnumerator ChangeAction() 
    {
        _canChangeAction = false;
        _bigas.State = EBigasState.Idle;
        yield return new WaitForSeconds(SecondsOfIdle);
        _bigas.State = (EBigasState)UnityEngine.Random.Range(1, Enum.GetValues(typeof(EBigasState)).Length);
        yield return new WaitForSeconds(SecondsBetweenAction);
        _canChangeAction = true;
    }
}
