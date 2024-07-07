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
        private SerializedProperty _rotationPoint;
        private SerializedProperty _hitConfiguration;
        private SerializedProperty _deathParticle;
        private SerializedProperty _deathSound;
        private SerializedProperty _deathDisappearDuration;
        private SerializedProperty _deathLayerName;
        private SerializedProperty _maxHealth;
        private SerializedProperty _rotationSpeed;
        private SerializedProperty _thinkDelay;
        private SerializedProperty _enemyType;
        private SerializedProperty _isDebug;

        private SerializedProperty _fireSound;
        private SerializedProperty _minPitch;
        private SerializedProperty _maxPitch;
        private SerializedProperty _attackRadius;
        private SerializedProperty _walls;

        private SerializedProperty _attackAngle;
        private SerializedProperty _mortarTrajectoryHeightOffset;
        private SerializedProperty _shootingEffect;
        private SerializedProperty _hitTemplate;
        private SerializedProperty _projectile;
        private SerializedProperty _aimTemplate;
        private SerializedProperty _projectileType;
        private SerializedProperty _distanceBetween;
        private SerializedProperty _clusterCount;

        private SerializedProperty _debugTarget;

        private void OnEnable()
        {
            _animator = serializedObject.FindProperty(nameof(_animator));
            _viewPoint = serializedObject.FindProperty(nameof(_viewPoint));
            _rotationPoint = serializedObject.FindProperty(nameof(_rotationPoint));
            _hitConfiguration = serializedObject.FindProperty(nameof(_hitConfiguration));
            _deathParticle = serializedObject.FindProperty(nameof(_deathParticle));
            _deathSound = serializedObject.FindProperty(nameof(_deathSound));
            _deathDisappearDuration = serializedObject.FindProperty(nameof(_deathDisappearDuration));
            _deathLayerName = serializedObject.FindProperty(nameof(_deathLayerName));
            _maxHealth = serializedObject.FindProperty(nameof(_maxHealth));
            _rotationSpeed = serializedObject.FindProperty(nameof(_rotationSpeed));
            _thinkDelay = serializedObject.FindProperty(nameof(_thinkDelay));
            _enemyType = serializedObject.FindProperty(nameof(_enemyType));
            _isDebug = serializedObject.FindProperty(nameof(_isDebug));

            _fireSound = serializedObject.FindProperty(nameof(_fireSound));
            _minPitch = serializedObject.FindProperty(nameof(_minPitch));
            _maxPitch = serializedObject.FindProperty(nameof(_maxPitch));
            _attackRadius = serializedObject.FindProperty(nameof(_attackRadius));
            _attackAngle = serializedObject.FindProperty(nameof(_attackAngle));
            _walls = serializedObject.FindProperty(nameof(_walls));
            _mortarTrajectoryHeightOffset = serializedObject.FindProperty(nameof(_mortarTrajectoryHeightOffset));

            _shootingEffect = serializedObject.FindProperty(nameof(_shootingEffect));
            _hitTemplate = serializedObject.FindProperty(nameof(_hitTemplate));
            _projectile = serializedObject.FindProperty(nameof(_projectile));
            _projectileType = serializedObject.FindProperty(nameof(_projectileType));
            _aimTemplate = serializedObject.FindProperty(nameof(_aimTemplate));
            _distanceBetween = serializedObject.FindProperty(nameof(_distanceBetween));
            _clusterCount = serializedObject.FindProperty(nameof(_clusterCount));

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
            EditorGUILayout.PropertyField(_rotationPoint);
            EditorGUILayout.PropertyField(_hitConfiguration);
            EditorGUILayout.PropertyField(_deathParticle);
            EditorGUILayout.PropertyField(_deathSound);
            EditorGUILayout.PropertyField(_deathDisappearDuration);
            EditorGUILayout.PropertyField(_deathLayerName);
            EditorGUILayout.PropertyField(_maxHealth);
            EditorGUILayout.PropertyField(_rotationSpeed);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_thinkDelay);
            EditorGUILayout.LabelField(ThinkDelayLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_enemyType);
            EditorGUILayout.PropertyField(_isDebug);
            EditorGUILayout.PropertyField(_attackRadius);
            EditorGUILayout.PropertyField(_walls);
            EditorGUILayout.PropertyField(_fireSound);
            EditorGUILayout.PropertyField(_minPitch);
            EditorGUILayout.PropertyField(_maxPitch);
            EditorGUILayout.PropertyField(_hitTemplate);
        }

        private void DrawAdditions()
        {
            switch ((EnemyTypes)_enemyType.enumValueIndex)
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
        }

        private void ShowBunkerOptions()
        {
            _mortarTrajectoryHeightOffset.floatValue = (float)ValueConstants.Zero;
            _projectile.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_attackAngle);
            EditorGUILayout.PropertyField(_shootingEffect);
        }

        private void ShowMortarOptions()
        {
            _shootingEffect.objectReferenceValue = null;
            EditorGUILayout.PropertyField(_attackAngle);
            EditorGUILayout.PropertyField(_mortarTrajectoryHeightOffset);
            EditorGUILayout.PropertyField(_projectile);
            EditorGUILayout.PropertyField(_aimTemplate);
            EditorGUILayout.PropertyField(_projectileType);

            switch ((ProjectileTypes)_projectileType.enumValueIndex)
            {
                case ProjectileTypes.Standard:
                    ShowStandardProjectile();
                    break;

                case ProjectileTypes.Cluster:
                    ShowClusterProjectile();
                    break;

                case ProjectileTypes.Triple:
                    ShowTripleProjectile();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowStandardProjectile()
        {
            _distanceBetween.floatValue = (float)ValueConstants.Zero;
            _clusterCount.intValue = (int)ValueConstants.Zero;
        }

        private void ShowClusterProjectile()
        {
            EditorGUILayout.PropertyField(_distanceBetween);
            EditorGUILayout.PropertyField(_clusterCount);
        }

        private void ShowTripleProjectile()
        {
            _clusterCount.intValue = (int)ValueConstants.Zero;
            EditorGUILayout.PropertyField(_distanceBetween);
        }
    }
}