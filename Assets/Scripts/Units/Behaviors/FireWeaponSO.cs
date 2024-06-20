using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Behavior
{
    [CreateAssetMenu(menuName ="Behaviors / FireWeapon")]
    public class FireWeaponSO : ScriptableUnitBehavior
    {
        public FireWeapon.TriggerPull triggerPullType;
        public float triggerPullDuration = 0.5f;

        public override IUnitBehavior GetBehavior(GameObject parent)
        {
            return new FireWeapon(parent, triggerPullType, triggerPullDuration);
        }
    }

    public class FireWeapon : BaseUnitBehavior
    {
        public enum TriggerPull { ONE_FRAME, HOLD}

        Enemy enemy;
        Weapon weapon;
        public TriggerPull triggerPullType;
        public float triggerPullDuration = 0.5f;

        float _elapsed = 0;

        SpacialManager spacialManager;
        public FireWeapon(GameObject parent, TriggerPull pullType, float duration) : base(parent)
        {
            triggerPullType = pullType;
            triggerPullDuration = duration;

            enemy = parent.GetComponent<Enemy>();

            ServiceLocator.TryGetService(out spacialManager);
        }

        public override void Enter()
        {
            base.Enter();

            var approxPlayerPosition = spacialManager.PartitionToWorld(spacialManager.PlayerPartition);

            weapon = enemy.Weapon;
            weapon.TryFireWeapon(Parent, approxPlayerPosition );

        }

        public override IUnitBehavior.Result Process(float deltaTime)
        {
            if(triggerPullType == TriggerPull.ONE_FRAME)
            {
                return IUnitBehavior.Result.DONE;
            }

            _elapsed += deltaTime;

            if (_elapsed > triggerPullDuration)
                return IUnitBehavior.Result.DONE;
            else
                return IUnitBehavior.Result.INCOMPLETE;
        }

        public override void Exit()
        {
            _elapsed = 0;
        }
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(FireWeaponSO))]
    public class FireWeaponSOEditor : Editor
    {
        SerializedProperty triggerPullDuration;
        SerializedProperty triggerPullType;

        private void OnEnable()
        {
            triggerPullDuration = serializedObject.FindProperty(nameof(triggerPullDuration));
            triggerPullType = serializedObject.FindProperty(nameof(triggerPullType));
        }

        public override void OnInspectorGUI()
        {
            FireWeaponSO fireWeaponSO = (FireWeaponSO)target;

            EditorGUILayout.PropertyField(triggerPullType);

            if(fireWeaponSO.triggerPullType.Equals(FireWeapon.TriggerPull.HOLD))
            {
                RenderHoldOptions();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void RenderHoldOptions()
        {
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(triggerPullDuration);
            EditorGUILayout.EndHorizontal();

        }
    }
#endif
#endregion
}