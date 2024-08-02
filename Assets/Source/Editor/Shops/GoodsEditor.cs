using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shops
{
    [CustomEditor(typeof(Goods))]
    public class GoodsEditor : Editor
    {
        private const string AddButton = nameof(AddButton);
        private const string MainList = nameof(MainList);
        private const string Item = nameof(Item);
        private const string ItemGood = nameof(ItemGood);
        private const string Icon = nameof(Icon);
        private const string IconField = nameof(IconField);
        private const string DeleteItem = nameof(DeleteItem);
        private const string ItemValue = nameof(ItemValue);
        private const string ValueList = nameof(ValueList);
        private const string Foldout = nameof(Foldout);
        private const string Key = nameof(Key);
        private const string Value = nameof(Value);
        private const string IntValue = nameof(IntValue);
        private const string FloatValue = nameof(FloatValue);
        private const string BoolValue = nameof(BoolValue);
        private const string Price = nameof(Price);

        [SerializeField] private VisualTreeAsset _tree;
        [SerializeField] private VisualTreeAsset _goodItem;
        [SerializeField] private VisualTreeAsset _goodValue;

        private SerializedProperty _content;
        private SerializedProperty _icons;
        private VisualElement _root;
        private VisualElement _holder;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            _tree.CloneTree(_root);
            Draw(_root.Q<ListView>(MainList));
            return _root;
        }

        private void OnEnable()
        {
            _content = serializedObject.FindProperty(nameof(_content));
            _icons = serializedObject.FindProperty(nameof(_icons));
        }

        private VisualElement CreateElement(int id)
        {
            SerializedProperty item = _content.GetArrayElementAtIndex(id);
            SerializedProperty itemKey = item.FindPropertyRelative(Key);
            SerializedProperty pairValue = item.FindPropertyRelative(Value);
            SerializedProperty goodValue = pairValue.FindPropertyRelative(nameof(SerializedPair<object, object>.Key));
            SerializedProperty valuesList = pairValue.FindPropertyRelative(Value);

            SerializedProperty iconProperty = _icons
                .GetArrayElementAtIndex(FindIconId((GoodNames)itemKey.enumValueIndex))
                .FindPropertyRelative(Value);
            Sprite icon = iconProperty.objectReferenceValue as Sprite;

            VisualElement element = CloneItem(_goodItem);
            Foldout foldout = element.Q<Foldout>(Foldout);
            EnumField itemValueField = element.Q<EnumField>(ItemValue);
            ListView valuesListView = element.Q<ListView>(ValueList);
            PropertyField iconField = element.Q<PropertyField>(IconField);
            VisualElement iconHolder = element.Q<VisualElement>(Icon);

            element.Q<Button>(AddButton).RegisterCallback<ClickEvent>(_ => OnAddValue(valuesList));
            element.Q<EnumField>(ItemGood).BindProperty(itemKey);

            iconField.BindProperty(iconProperty);
            iconHolder.style.backgroundImage = new StyleBackground(icon);
            iconField.RegisterValueChangeCallback(
                callback =>
                    iconHolder.style.backgroundImage =
                        new StyleBackground(callback.changedProperty.objectReferenceValue as Sprite));

            itemValueField.BindProperty(goodValue);
            itemValueField.RegisterValueChangedCallback(_ => ReDraw());

            foldout.text += $"{id}";

            for (int i = 0; i < valuesList.arraySize; i++)
                valuesListView.hierarchy.Add(CreateValue(valuesList, (GoodsValues)goodValue.enumValueIndex, i));

            return element;
        }

        private VisualElement CreateValue(SerializedProperty valuesList, GoodsValues value, int id)
        {
            SerializedProperty pair = valuesList.GetArrayElementAtIndex(id);
            SerializedProperty cell = pair.FindPropertyRelative(Key);
            SerializedProperty price = pair.FindPropertyRelative(Value);
            VisualElement element = CloneItem(_goodValue);

            element.Q<IntegerField>(Price).BindProperty(price);
            element.Q<Button>(DeleteItem).RegisterCallback<ClickEvent>(_ => OnDeleteValue(id, valuesList));

            switch (value)
            {
                case GoodsValues.Int:
                    DrawValue(element.Q<IntegerField>(IntValue), cell.FindPropertyRelative("_intValue"));
                    break;

                case GoodsValues.Float:
                    DrawValue(element.Q<FloatField>(FloatValue), cell.FindPropertyRelative("_floatValue"));
                    break;

                case GoodsValues.Bool:
                    DrawValue(element.Q<Toggle>(BoolValue), cell.FindPropertyRelative("_boolValue"));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }

            return element;
        }

        private VisualElement CloneItem(VisualTreeAsset tree)
        {
            VisualElement element = new VisualElement();
            tree.CloneTree(element);
            return element.Q<VisualElement>(Item);
        }

        private int FindIconId(GoodNames good)
        {
            for (int i = 0; i < _icons.arraySize; i++)
            {
                if ((GoodNames)_icons.GetArrayElementAtIndex(i).FindPropertyRelative(Key).enumValueIndex == good)
                    return i;
            }

            throw new ArgumentOutOfRangeException(nameof(List<SerializedPair<GoodNames, Sprite>>));
        }

        private void ReDraw()
        {
            _holder.RemoveFromHierarchy();
            Draw(_root.Q<ListView>(MainList));
        }

        private void Draw(ListView contentList)
        {
            _holder = new VisualElement();

            for (int i = 0; i < _content.arraySize; i++)
                _holder.Add(CreateElement(i));

            contentList.hierarchy.Add(_holder);
        }

        private void OnDeleteValue(int id, SerializedProperty valuesList)
        {
            OnSerializeModified(() => valuesList.DeleteArrayElementAtIndex(id));
        }

        private void OnAddValue(SerializedProperty valuesList)
        {
            OnSerializeModified(() => { valuesList.arraySize++; });
        }

        private void OnSerializeModified(Action callback)
        {
            serializedObject.Update();
            callback?.Invoke();
            serializedObject.ApplyModifiedProperties();
            ReDraw();
        }

        private void DrawValue<T>(BaseField<T> element, SerializedProperty property)
        {
            element.style.display = DisplayStyle.Flex;
            element.BindProperty(property);
        }
    }
}