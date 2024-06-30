using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorSequence : MonoBehaviour
{
    [SerializeField]
    private List<ScriptableUnitBehavior> behaviors;

    public List<IUnitBehavior> behaviorObjs = new List<IUnitBehavior>();

    private int _currentIndex = 0;
    private IUnitBehavior _currentState;
    public IUnitBehavior CurrentState => _currentState;

    public BehaviorSequence(ICollection<IUnitBehavior> behaviors)
    {
        behaviors = new List<IUnitBehavior>(behaviors);
    }

    private void Start()
    {
        behaviorObjs = new List<IUnitBehavior>();
        //Get instantiated versions of all behavior ScrObjs
        foreach(var behaviorSO in behaviors)
        {
            var instantiatedBehavior = behaviorSO.GetBehavior(gameObject);
            behaviorObjs.Add(instantiatedBehavior);
        }

        _currentIndex = -1;
        
        GoToNextState();
    }

    private void GoToNextState()
    {
        _currentState?.Exit();
        _currentIndex = (_currentIndex + 1) % behaviorObjs.Count;
        _currentState = behaviorObjs[_currentIndex];

        _currentState.Enter();
    }

    private void OnDisable()
    {
        _currentState?.Exit();
    }

    private void Update()
    {
        var result = _currentState.Process(Time.deltaTime);

        if(result.Equals(IUnitBehavior.Result.DONE))
        {
            GoToNextState();
        }
    }
}
