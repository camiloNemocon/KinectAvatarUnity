using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using Windows.Kinect;
using System.Collections.Generic;

[CustomEditor(typeof(KinectInputModule))]
public class KinectInputModuleEditor : Editor
{

    private ReorderableList list;
    KinectInputModule kModule;

    SerializedProperty _scrollTreshold;
    SerializedProperty _scrollSpeed;
    SerializedProperty _waitOverTime;

    void OnEnable()
    {
        kModule = target as KinectInputModule;
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("_inputData"), true, true, true, true);
        list.drawHeaderCallback += OnDrawHeader;
        list.drawElementCallback += OnDrawElements;
        list.onAddDropdownCallback += OnAddDropDown;

        _scrollSpeed = serializedObject.FindProperty("_scrollSpeed");
        _scrollTreshold = serializedObject.FindProperty("_scrollTreshold");
        _waitOverTime = serializedObject.FindProperty("_waitOverTime");
    }

    private void OnAddDropDown(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        if (kModule._inputData.Length >= 2) return;
        if (kModule._inputData.Length == 0)
        {
            menu.AddItem(new GUIContent("Right Hand"),
                     false, OnClickHandler,
                     new DataParams() { jointType = KinectUIHandType.Right });
            menu.AddItem(new GUIContent("Left Hand"),
                     false, OnClickHandler,
                     new DataParams() { jointType = KinectUIHandType.Left });
        }
        else if (kModule._inputData.Length == 1)
        {
            DataParams param;
            string name;
            if (kModule._inputData[0].trackingHandType == KinectUIHandType.Left){
                param = new DataParams() { jointType = KinectUIHandType.Right };
                name = "Right Hand";
            }
            else
            {
                param = new DataParams() { jointType = KinectUIHandType.Left };
                name = "Left Hand";
            }
            menu.AddItem(new GUIContent(name),false, OnClickHandler, param);
        }
        menu.ShowAsContext();
    }

    private void OnClickHandler(object target)
    {
        var data = (DataParams)target;
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("trackingHandType").enumValueIndex = (int)data.jointType;
        serializedObject.ApplyModifiedProperties();
    }

    private void OnDrawElements(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 3;
        float w = 140f;
        //EditorGUI.LabelField(new Rect(rect.x, rect.y, labelStart, EditorGUIUtility.singleLineHeight), "Type");
        KinectUIHandType ty = (KinectUIHandType)element.FindPropertyRelative("trackingHandType").enumValueIndex;

        //EditorGUI.PropertyField(
        //    new Rect(rect.x + labelStart, rect.y, w, EditorGUIUtility.singleLineHeight),
        //    element.FindPropertyRelative("trackingHandType"), GUIContent.none);
        EditorGUI.LabelField(
            new Rect(rect.x , rect.y, w, EditorGUIUtility.singleLineHeight),
            "Tracking Hand: "+ty.ToString(),EditorStyles.boldLabel);


        EditorGUI.LabelField(new Rect(rect.width - w -10f, rect.y, 160f, EditorGUIUtility.singleLineHeight), "Screen Position Multiplier:");
        
        EditorGUI.PropertyField(
            new Rect(rect.width, rect.y, 30f, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("handScreenPositionMultiplier"), GUIContent.none);
    }

    private void OnDrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Tracking Hands");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        serializedObject.Update();
        list.DoLayoutList();
        // Draw other properties
        EditorGUILayout.PropertyField(_scrollSpeed, new GUIContent("Scroll Speed"));
        EditorGUILayout.PropertyField(_scrollTreshold, new GUIContent("Scroll Treshold"));
        EditorGUILayout.PropertyField(_waitOverTime, new GUIContent("Wait Over Time"));


        serializedObject.ApplyModifiedProperties();
    }

    private struct DataParams
    {
        public KinectUIHandType jointType;
    }
}
