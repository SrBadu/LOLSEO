using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SelectableUnit : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anima;

    [SerializeField]
    private GameObject SelectionSprite;

    private void Awake()
    {
        SelectionManager.Instance.AvailableUnits.Add(this);
        agent = GetComponent<NavMeshAgent>();
        anima = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void MoveTo(Vector3 Position)
    {
        agent.SetDestination(Position);
        anima.SetFloat("mov", .5f);
    }

    public void OnSelected()
    {
        SelectionSprite.gameObject.SetActive(true);
    }

    public void OnDeselected()
    {
        SelectionSprite.gameObject.SetActive(false);
    }
    
}
