using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(RequireInterfaceAttribute), true)]
public class RequireInterfaceDrawer : PropertyDrawer
{
    private bool _helpBoxOpen = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label, true);

        if (_helpBoxOpen)
            height *= 2;

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RequireInterfaceAttribute interfaceAttribute = attribute as RequireInterfaceAttribute;

        //Get field type.
        string[] slices = property.propertyPath.Split('.');
        System.Type propertyType = property.serializedObject.targetObject.GetType();

        bool isArray = false;

        for (int i = 0; i < slices.Length; i++)
        {
            if (slices[i] == "Array")
            {
                isArray = true;
                i++; //skip "data[x]"
                propertyType = propertyType.GetElementType();
            }

            else
            {
                FieldInfo info = propertyType.GetField(slices[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                if(info != null)
                {
                    propertyType = info.FieldType;
                }
                else
                {
                    Debug.LogWarning("RequireInterfaceDrawer: cannot find \"" + slices[i] + "\" field in " + propertyType + ".");
                    return;
                }
            }
        }

        //Interface check.
        if (!interfaceAttribute.type.IsInterface)
        {
            Debug.LogWarning("RequireInterfaceDrawer: Required type is not an interface.");
            return;
        }

        //Property type compatibility check.
        if (property.propertyType != SerializedPropertyType.ObjectReference)
        {
            Debug.LogWarning("RequireInterfaceDrawer: property type not handled by drawer.");
            return;
        }

        //Rect adjustements.
        Rect fieldRect = position;
        fieldRect.xMax -= 20;

        Rect helpButtonRect = position;
        helpButtonRect.width = 20;
        helpButtonRect.x = fieldRect.xMax;

        if (_helpBoxOpen)
        {
            fieldRect.yMax -= fieldRect.height / 2;
            helpButtonRect.yMax = fieldRect.yMax;
        }

        Rect helpBoxRect = position;
        helpBoxRect.yMin += helpBoxRect.height / 2;

        //If using attribute with an array it is possible to bug the type check by drag dropping on
        //the array directly. I couldn't find any other way besides a constant check on the current
        //reference for the array case.
        if (isArray)
        {
            Object checkedObject = property.objectReferenceValue;
            property.objectReferenceValue = GetValidTarget(checkedObject, propertyType, interfaceAttribute);
        }

        Object[] draggedObject = DragAndDrop.objectReferences;

        //Drag and drop object support.
        if ((draggedObject.Length > 0) && position.Contains(Event.current.mousePosition))
        {
            Object assignedObject = EditorGUI.ObjectField(fieldRect, property.name, property.objectReferenceValue, typeof(Object), true);

            assignedObject = GetValidTarget(assignedObject, propertyType, interfaceAttribute);
            if (assignedObject != null)
            {
                property.objectReferenceValue = assignedObject;
            }

            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
        //Default display.
        else
        {
            Object shownObject = property.objectReferenceValue;

            if (property.objectReferenceValue != null)
            {
                if (typeof(GameObject).IsAssignableFrom(property.objectReferenceValue.GetType()))
                {
                    Component component = (property.objectReferenceValue as GameObject).GetComponent(interfaceAttribute.type);

                    if (component != null)
                        shownObject = component;
                }
            }

            Object assignedObject = EditorGUI.ObjectField(fieldRect, property.name, shownObject, interfaceAttribute.type, true);

            if (assignedObject == null)
            {
                property.objectReferenceValue = assignedObject;
            }
        }

        if (GUI.Button(helpButtonRect, "?"))
            _helpBoxOpen = !_helpBoxOpen;

        if (_helpBoxOpen)
        {
            EditorGUI.HelpBox(helpBoxRect, "\""+propertyType.ToString()+ "\" field implemeting \"" + interfaceAttribute.type + "\"", MessageType.Info);
        }
    }

    private Object GetValidTarget(Object target, System.Type propertyType, RequireInterfaceAttribute interfaceAttribute)
    {
        if (target == null)
            return null;

        Object validTarget = null;

        System.Type targetType = target.GetType();

        //If property is of GameObject type.
        if (typeof(GameObject).IsAssignableFrom(propertyType))
        {
            //If trying to assign another GameObject to property.
            if (typeof(GameObject).IsAssignableFrom(targetType))
            {
                Component component = (target as GameObject).GetComponent(interfaceAttribute.type);

                if (component != null)
                    validTarget = target;
            }
            //If trying to assign a Component to property.
            else if (typeof(Component).IsAssignableFrom(targetType))
            {
                GameObject gameobject = (target as Component).gameObject;
                validTarget = gameobject;
            }

        }
        //For all other property types.
        else
        {
            //If trying to assign another GameObject to property.
            if (typeof(GameObject).IsAssignableFrom(targetType))
            {
                Component component = (target as GameObject).GetComponent(interfaceAttribute.type);

                if (component != null && propertyType.IsAssignableFrom(component.GetType()))
                    validTarget = component;
            }
            //If trying to assign any other types.
            else if (interfaceAttribute.type.IsAssignableFrom(targetType))
            {
                if (propertyType.IsAssignableFrom(targetType))
                    validTarget = target;
            }
        }

        return validTarget;
    }
}
