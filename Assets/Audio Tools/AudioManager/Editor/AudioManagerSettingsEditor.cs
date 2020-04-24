using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(AudioManagerSettings))]
public class AudioManagerSettingsEditor : Editor
{
    private SerializedProperty musicL;
    private SerializedProperty sfxL;
    ReorderableList mList;
    ReorderableList sList;
    
    private void OnEnable()
    {
        // Get the <wave> array from WaveManager, in SerializedProperty form.
        // Set up the reorderable list
        musicL = serializedObject.FindProperty("musicList");
        sfxL = serializedObject.FindProperty("sfxList");
        
        mList = new ReorderableList(serializedObject, musicL, true, true, true, true);
        sList = new ReorderableList(serializedObject, sfxL, true, true, true, true);
        
        mList.drawElementCallback = DrawMusicListItems; // Delegate to draw the elements on the list
        mList.drawHeaderCallback = DrawMusicListHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
        
        sList.drawElementCallback = DrawSfxListItems; // Delegate to draw the elements on the list
        sList.drawHeaderCallback = DrawSfxListHeader; // Skip this line if you set displayHeader to 'false' in your ReorderableList constructor.
    }
    
    void DrawMusicListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        
        SerializedProperty element = mList.serializedProperty.GetArrayElementAtIndex(index); // The element in the list
        
        EditorGUI.LabelField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), "Audio Clip");
        EditorGUI.PropertyField(
            new Rect(new Rect(rect.x + 80, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight)), 
            element,
            GUIContent.none
        );
    }
    
    void DrawMusicListHeader(Rect rect)
    {
        string name = "Music List";
        EditorGUI.LabelField(rect, name);
    }
    
    void DrawSfxListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = sList.serializedProperty.GetArrayElementAtIndex(index); // The element in the list
        
        EditorGUI.LabelField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), "Audio Clip");
        EditorGUI.PropertyField(
            new Rect(new Rect(rect.x + 80, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight)), 
            element,
            GUIContent.none
        );
    }
    
    void DrawSfxListHeader(Rect rect)
    {
        string name = "Sfx List";
        EditorGUI.LabelField(rect, name);
    }

    //This is the function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI(); 
        serializedObject.Update(); // Update the array property's representation in the inspector
        
        EditorGUILayout.Space();
        
        mList.DoLayoutList(); // Have the ReorderableList do its work
        
        EditorGUILayout.Space();
        
        sList.DoLayoutList();
        
        // We need to call this so that changes on the Inspector are saved by Unity.
        serializedObject.ApplyModifiedProperties();
    }
}
