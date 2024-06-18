using Services;
using UnityEngine;

namespace Spawner
{
    [CreateAssetMenu(fileName = "EnemyTemplate", menuName = "Templates/EnemyTemplate", order = 1)]
    public class EnemyTemplate : Template<Enemy> { }
}