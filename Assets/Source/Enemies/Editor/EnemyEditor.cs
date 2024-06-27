using System;
using UnityEditor;

namespace Enemies
{
    [CustomEditor(typeof(EnemySetup))]
    public class EnemyEditor : Editor
    {
        private const string ThinkDelayLabel = "+ Random(0, 0.5)";

        private SerializedProperty _animator;
        private SerializedProperty _viewPoint;
        private SerializedProperty _transform;
        private SerializedProperty _rotationSpeed;
        private SerializedProperty _thinkDelay;
        private SerializedProperty _type;
        private SerializedProperty _isDebug;

        private SerializedProperty _sound;
        private SerializedProperty _attackRadius;
        private SerializedProperty _walls;

        private SerializedProperty _attackAngle;
        private SerializedProperty _mortarTrajectoryHeightOffset;
        private SerializedProperty _shootingEffect;
        private SerializedProperty _hitEffect;
        private SerializedProperty _projectile;

        private SerializedProperty _debugTarget;

        private void OnEnable()
        {
            _animator = serializedObject.FindProperty(nameof(_animator));
            _viewPoint = serializedObject.FindProperty(nameof(_viewPoint));
            _transform = serializedObject.FindProperty(nameof(_transform));
            _rotationSpeed = serializedObject.FindProperty(nameof(_rotationSpeed));
            _thinkDelay = serializedObject.FindProperty(nameof(_thinkDelay));
            _type = serializedObject.FindProperty(nameof(_type));
            _isDebug = serializedObject.FindProperty(nameof(_isDebug));

            _sound = serializedObject.FindProperty(nameof(_sound));
            _attackRadius = serializedObject.FindProperty(nameof(_attackRadius));
            _attackAngle = serializedObject.FindProperty(nameof(_attackAngle));
            _walls = serializedObject.FindProperty(nameof(_walls));
            _mortarTrajectoryHeightOffset = serializedObject.FindProperty(nameof(_mortarTrajectoryHeightOffset));

            _shootingEffect = serializedObject.FindProperty(nameof(_shootingEffect));
            _hitEffect = serializedObject.FindProperty(nameof(_hitEffect));
            _projectile = serializedObject.FindProperty(nameof(_projectile));
            _debugTarget = serializedObject.FindProperty(nameof(_debugTarget));
        }

        public override void OnInspectorGUI()
        {
            DrawMain();
            DrawAdditions();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawMain()
        {
            EditorGUILayout.PropertyField(_animator);
            EditorGUILayout.PropertyField(_viewPoint);
            EditorGUILayout.PropertyField(_thinkDelay);
            EditorGUILayout.PropertyField(_transform);
            EditorGUILayout.PropertyField(_rotationSpeed);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_thinkDelay);
            EditorGUILayout.LabelField(ThinkDelayLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_type);
            EditorGUILayout.PropertyField(_isDebug);
            EditorGUILayout.PropertyField(_attackRadius);
            EditorGUILayout.PropertyField(_walls);
        }

        private void DrawAdditions()
        {
            EditorGUILayout.PropertyField(_sound);

            switch ((EnemyTypes)_type.enumValueIndex)
            {
                case EnemyTypes.Standard:
                    ShowStandardOptions();
                    break;

                case EnemyTypes.Mortar:
                    ShowMortarOptions();
                    break;

                case EnemyTypes.Bunker:
                    ShowBunkerOptions();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_isDebug.boolValue == false)
                return;

            EditorGUILayout.PropertyField(_debugTarget);
        }

        private void ShowStandardOptions()
        {
            _attackAngle.floatValue = (float)ValueConstants.Zero;
            _mortarTrajectoryHeightOffset.floatValue = (float)ValueConstants.Zero;
            _projectile.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_shootingEffect);
            EditorGUILayout.PropertyField(_hitEffect);
        }

        private void ShowMortarOptions()
        {
            _shootingEffect.objectReferenceValue = null;
            _hitEffect.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_attackAngle);
            EditorGUILayout.PropertyField(_mortarTrajectoryHeightOffset);
            EditorGUILayout.PropertyField(_projectile);
        }

        private void ShowBunkerOptions()
        {
            _mortarTrajectoryHeightOffset.floatValue = (float)ValueConstants.Zero;
            _projectile.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_attackAngle);
            EditorGUILayout.PropertyField(_shootingEffect);
            EditorGUILayout.PropertyField(_hitEffect);
        }
    }
}