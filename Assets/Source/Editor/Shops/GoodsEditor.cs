using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shops
{
    [CustomEditor(typeof(Goods))]
    public class GoodsEditor : Editor
    {
        private const string AddButton = "AddButton";
        private const string MainList = "MainList";
        private const string Item = "Item";
        private const string ItemGood = "ItemGood";
        private const string DeleteItem = "DeleteItem";
        private const string ItemValue = "ItemValue";
        private const string ValueList = "ValueList";
        private const string Foldout = "Foldout";
        private const string Key = "Key";
        private const string Value = "Value";
        private const string IntValue = "IntValue";
        private const string FloatValue = "FloatValue";
        private const string BoolValue = "BoolValue";
        private const string Price = "Price";

        [SerializeField] private VisualTreeAsset _tree;
        [SerializeField] private VisualTreeAsset _goodItem;
        [SerializeField] private VisualTreeAsset _goodValue;

        private SerializedProperty _content;
        private VisualElement _root;
        private VisualElement _holder;
        private Goods _goods;

        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            _tree.CloneTree(_root);
            Draw(_root.Q<ListView>(MainList));
            return _root;
        }

        private void OnEnable()
        {
            _goods = (Goods)target;
            _content = serializedObject.FindProperty(nameof(_content));
        }

        private VisualElement CreateElement(int id)
        {
            SerializedProperty item = _content.GetArrayElementAtIndex(id);
            SerializedProperty itemKey = item.FindPropertyRelative(Key);
            SerializedProperty pairValue = item.FindPropertyRelative(Value);
            SerializedProperty goodValue = pairValue.FindPropertyRelative(nameof(SerializedPair<object, object>.Key));
            SerializedProperty valuesList = pairValue.FindPropertyRelative(Value);

            VisualElement element = CloneItem(_goodItem);
            Foldout foldout = element.Q<Foldout>(Foldout);
            EnumField itemValueField = element.Q<EnumField>(ItemValue);
            ListView valuesListView = element.Q<ListView>(ValueList);

            element.Q<Button>(DeleteItem).RegisterCallback<ClickEvent>(_ => OnDeleteContent(id));
            element.Q<Button>(AddButton).RegisterCallback<ClickEvent>(_ => OnAddValue(valuesList));
            element.Q<EnumField>(ItemGood).BindProperty(itemKey);
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

        private void OnDeleteContent(int id)
        {
            OnSerializeModified(() => { _content.DeleteArrayElementAtIndex(id); });
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