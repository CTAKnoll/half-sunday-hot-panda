using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    [CreateAssetMenu(menuName = "Behaviors / WaitForSeconds")]
    public class WaitForSecondsSO : ScriptableUnitBehavior
    {
        public float waitTime;
        public override IUnitBehavior GetBehavior(GameObject parent)
        {
            return new WaitForSeconds(parent, waitTime);
        }
    }

    public class WaitForSeconds : BaseUnitBehavior
    {
        public float waitTime;
        private float _elapsed;

        public WaitForSeconds(GameObject parent, float time) : base(parent)
        {
            waitTime = time;
        }

        public override IUnitBehavior.Result Process(float deltaTime)
        {
            _elapsed += deltaTime;

            if(_elapsed > waitTime)
            {
                return IUnitBehavior.Result.DONE;
            }
            else
            {
                return IUnitBehavior.Result.INCOMPLETE;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _elapsed = 0;
        }
    }
}