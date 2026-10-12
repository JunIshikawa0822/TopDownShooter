#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HTN
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        private struct TypeOption
        {
            public Type type;
            public string content;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var labelPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelPosition, label);

            var typeMenuPosition = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y, position.width - EditorGUIUtility.labelWidth - 2, EditorGUIUtility.singleLineHeight);
            
            string typeName = GetTypeName(property);
            if (GUI.Button(typeMenuPosition, new GUIContent(typeName), EditorStyles.popup))
            {
                ShowTypeMenu(property);
            }

            EditorGUI.PropertyField(position, property, GUIContent.none, true);

            EditorGUI.EndProperty();
        }

        private string GetTypeName(SerializedProperty property)
        {
            if (string.IsNullOrEmpty(property.managedReferenceFullTypename))
            {
                return "Null";
            }
            
            var type = GetType(property.managedReferenceFullTypename);
            return type != null ? type.Name : "Missing Type";
        }

        private Type GetType(string typeName)
        {
            var splitIndex = typeName.IndexOf(' ');
            var assemblyName = typeName.Substring(0, splitIndex);
            var className = typeName.Substring(splitIndex + 1);
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == assemblyName);
            return assembly?.GetType(className);
        }

        private void ShowTypeMenu(SerializedProperty property)
        {
            GenericMenu menu = new GenericMenu();
            var baseType = GetFieldType();
            
            menu.AddItem(new GUIContent("Null"), string.IsNullOrEmpty(property.managedReferenceFullTypename), () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            var types = GetInheritedTypes(baseType);
            foreach (var type in types)
            {
                menu.AddItem(new GUIContent(type.content), property.managedReferenceFullTypename.EndsWith(type.type.FullName), () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(type.type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            
            menu.ShowAsContext();
        }

        private Type GetFieldType()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) return type.GetElementType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) return type.GetGenericArguments()[0];
            return type;
        }

        private List<TypeOption> GetInheritedTypes(Type baseType)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface)
                .Select(p => new TypeOption { type = p, content = p.Name })
                .ToList();
            return types;
        }
    }
}
#endif