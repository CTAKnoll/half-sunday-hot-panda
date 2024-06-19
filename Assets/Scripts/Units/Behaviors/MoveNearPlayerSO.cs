using UnityEngine;

namespace Behavior
{
    [CreateAssetMenu(menuName ="Behaviors / MoveNearPlayer")]
    public class MoveNearPlayerSO : ScriptableUnitBehavior
    {
        public override IUnitBehavior GetBehavior(GameObject parent)
        {
            return new MoveNearPlayer(parent);
        }
    }

}