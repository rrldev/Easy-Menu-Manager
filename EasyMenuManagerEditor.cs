using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EasyMenuManager))]
//[CanEditMultipleObjects]
public class EasyMenuManagerEditor : Editor
{

    SerializedProperty emm_menuList;
    SerializedProperty emm_runTheseEventsWhenAnyMenuOpens;
    SerializedProperty emm_runTheseEventsWhenAnyMenuCloses;
    SerializedProperty emm_playSoundEvent;
    GUIStyle titlestyle;
    GUIStyle italicstyle;
    GUIStyle boldstyle;
    Color orange;
    Color lightpurple;
    Color lightpink;
    Color lightorange;
    Color lightyellow;
    GUIStyle redLabel;

    // Use this for initialization
    void OnEnable()
    {

        emm_menuList = serializedObject.FindProperty("menuList");
        emm_runTheseEventsWhenAnyMenuOpens = serializedObject.FindProperty("runTheseEventsWhenAnyMenuOpens");
        emm_runTheseEventsWhenAnyMenuCloses = serializedObject.FindProperty("runTheseEventsWhenAnyMenuCloses");
        emm_playSoundEvent = serializedObject.FindProperty("playSoundEvent");

        titlestyle = new GUIStyle();
        titlestyle.fontStyle = FontStyle.Normal;
        titlestyle.alignment = TextAnchor.MiddleCenter;
        titlestyle.fontSize = 16;

        italicstyle = new GUIStyle();
        italicstyle.fontStyle = FontStyle.Italic;
        italicstyle.alignment = TextAnchor.MiddleRight;
        italicstyle.fontSize = 9;

        boldstyle = new GUIStyle();
        boldstyle.fontStyle = FontStyle.Bold;
        //		boldstyle.alignment = TextAnchor.MiddleRight;
        //		boldstyle.fontSize = 9;

        orange = new Color(255 / 255f, 140 / 255f, 0 / 255f, 1);
        lightpurple = new Color(180 / 255f, 176 / 255f, 255 / 255f, 1);
        lightpink = new Color(255 / 255f, 176 / 255f, 255 / 255f, 1);
        lightorange = new Color(255 / 255f, 222 / 255f, 176 / 255f, 1);
        lightyellow = new Color(255 / 255f, 255 / 255f, 150 / 255f, 1);

        redLabel = new GUIStyle();
        redLabel.normal.textColor = new Color(255 / 255f, 7 / 255f, 7 / 255f, 1);

    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();





        EditorGUILayout.Space();

        if (emm_menuList.arraySize > 0) // CHECK FOR MENUS (GameObject or MenuController Component) THAT WERE DESTROYED AND DELETE THEM
        {
            for (int i = 0; i < emm_menuList.arraySize; i++)
            {
                if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue == null && emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("firstNamed").boolValue == true)
                {
                    serializedObject.ApplyModifiedProperties();
                    ((EasyMenuManager)target).EraseCompatibilityEntriesOfDeletedMenuList(i);
                    serializedObject.Update();

                    emm_menuList.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    //					return;
                }
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (emm_menuList.arraySize > 0)
        {
            bool allClosed = true;
            for (int i = 0; i < emm_menuList.arraySize; i++)
            {
                if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("showContents").boolValue == true)
                {
                    allClosed = false;
                    break;
                }
            }
            if (allClosed == true)
            {
                GUI.color = orange;
                if (GUILayout.Button("O all", GUILayout.MaxWidth(36f)))
                {
                    for (int i = 0; i < emm_menuList.arraySize; i++)
                    {
                        emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("showContents").boolValue = true;
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                GUI.color = Color.white;
            }
            else if (allClosed == false)
            {
                GUI.color = Color.yellow;
                if (GUILayout.Button("- all", GUILayout.MaxWidth(36f)))
                {
                    for (int i = 0; i < emm_menuList.arraySize; i++)
                    {
                        emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("showContents").boolValue = false;
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                GUI.color = Color.white;
            }
        }
        else
        {
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(36f));
        }

        EditorGUILayout.LabelField("MENU LIST", titlestyle);
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(36f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // MENU LIST

        for (int i = 0; i < emm_menuList.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 100f;
            EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController"), new GUIContent("Menu Controller"), GUILayout.ExpandWidth(true));
            EditorGUIUtility.labelWidth = 2f;
            GUI.color = Color.red;
            EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true), GUILayout.MaxWidth(60f));
            EditorGUIUtility.labelWidth = 0;
            if (GUILayout.Button("DEL", GUILayout.MaxWidth(60f)))
            {
                serializedObject.ApplyModifiedProperties();
                ((EasyMenuManager)target).EraseCompatibilityEntriesOfDeletedMenuList(i);
                serializedObject.Update();

                emm_menuList.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();

                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                return;
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue == null)
            {
                EditorGUILayout.LabelField("Set MenuController that is not currently in this list", redLabel, GUILayout.ExpandWidth(true));
            }

            if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue != null)
            {
                for (int q = 0; q < emm_menuList.arraySize; q++)
                {
                    if ((i) != (q))
                    {
                        if (emm_menuList.GetArrayElementAtIndex(q).FindPropertyRelative("menuController").objectReferenceValue == emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue)
                        {
                            // Two Menus with same controller. Clear this new one's menuController value and start over.
                            emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue = null;
                            return;
                        }
                    }
                }

                serializedObject.ApplyModifiedProperties();
                ((EasyMenuManager)target).SetMenuControllerParentMatch((EasyMenuManager)target, i);
                serializedObject.Update();

                if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("firstNamed").boolValue == false)
                {
                    emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("firstNamed").boolValue = true;
                    emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName").stringValue = emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue.name.ToString();
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }

                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 20;
                EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Select This Menu Controller", GUILayout.MaxWidth(260f), GUILayout.ExpandWidth(false)))
                {
                    Selection.activeObject = emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue;
                    return;
                }
                EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();
                serializedObject.Update();
                EditorGUILayout.Space();

                // OPEN AND CLOSE BUTTONS WHEN PLAYER IS RUNNING
                if (Application.isPlaying)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 1f;
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));
                    if (GUILayout.Button("Send 'Open' Command", GUILayout.MaxWidth(180f), GUILayout.ExpandWidth(false)))
                    {
                        ((EasyMenuManager)target).OpenMenu((MenuController)emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue);
                    }
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));
                    if (GUILayout.Button("Send 'Close' Command", GUILayout.MaxWidth(180f), GUILayout.ExpandWidth(false)))
                    {
                        ((EasyMenuManager)target).CloseMenu((MenuController)emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue);
                    }
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }


                EditorGUILayout.BeginHorizontal();
                bool showContentsOfMenuProperties = emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("showContents").boolValue;
                if (showContentsOfMenuProperties == false)
                {
                    GUI.color = orange;
                    if (GUILayout.Button("O", GUILayout.MaxWidth(26f)))
                    {
                        showContentsOfMenuProperties = true;
                    }
                    GUI.color = Color.white;
                }
                else if (showContentsOfMenuProperties == true)
                {
                    GUI.color = Color.yellow;
                    if (GUILayout.Button("-", GUILayout.MaxWidth(26f)))
                    {
                        showContentsOfMenuProperties = false;
                    }
                    GUI.color = Color.white;
                }
                emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("showContents").boolValue = showContentsOfMenuProperties;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                EditorGUIUtility.labelWidth = 80f;
                if (string.IsNullOrEmpty(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName").stringValue) == false)
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName"), new GUIContent("Menu Name"), GUILayout.ExpandWidth(true));
                else if (string.IsNullOrEmpty(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName").stringValue) == true)
                {
                    GUI.color = Color.red;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName"), new GUIContent("Menu Name"), GUILayout.ExpandWidth(true));
                    GUI.color = Color.white;
                }
                EditorGUIUtility.labelWidth = 0;

                EditorGUILayout.EndHorizontal();

                if (string.IsNullOrEmpty(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName").stringValue) == false && showContentsOfMenuProperties == true)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 176f;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeWhenOpenedWhileOpen"), new GUIContent("Close If Opened While Open", "If set to [TRUE], any attempt made by the user to open this menu while it is already open will instead close this menu."), GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 110f;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("disableWhenClosed"), new GUIContent("Hide On Close", "If set to [TRUE], the game object the menu controller is attached to will be moved very far off-screen and all child game objects containing a CanvasRenderer will be disabled after all close animations are finished. This should help reduce SetPass calls if the root canvas this menu belongs to does not move off-screen, but it does have a performance cost since it disables and enables game objects."), GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 136f;
                    EditorGUILayout.LabelField(new GUIContent("Initial State: ", "This is the state that the menu will automatically start in. This WILL NOT trigger open/close events. This CANNOT be prevented by other menus. This WILL NOT cause other menus to close based on compatibility settings."), GUILayout.MaxWidth(74f));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("startState"), GUIContent.none, GUILayout.MaxWidth(150f));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 182f;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("preventOpeningWhileClosing"), new GUIContent("Prevent Opening While Closing", "Enabling this will prevent any attempt to open this menu while this menu's own On Close animations are still playing."), GUILayout.ExpandWidth(true));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("preventClosingWhileOpening"), new GUIContent("Prevent Closing While Opening", "Enabling this will prevent any attempt to close this menu while this menu's own On Open animations are still playing."), GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(85f));
                    EditorGUIUtility.labelWidth = 93f;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("queueOpeningWhenPreventing"), new GUIContent("Queue Opening", "Enabling this will delay the opening of this menu when the 'Prevent Opening While Closing' option above is enabled until the close animations have all finished."), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(86f));
                    EditorGUIUtility.labelWidth = 93f;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("queueClosingWhenPreventing"), new GUIContent("Queue Closing", "Enabling this will delay the closing of this menu when the 'Prevent Closing While Opening' option above is enabled until the open animations have all finished."), GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();


                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Play Sound:", GUILayout.ExpandWidth(true), GUILayout.MaxWidth(74f));
                    EditorGUILayout.LabelField("On Open", italicstyle, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(40f));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("onOpenSoundName"), GUIContent.none, GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField("", italicstyle, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(9f));
                    EditorGUILayout.LabelField("On Close", italicstyle, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(40f));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("onCloseSoundName"), GUIContent.none, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    // ANIMATION ON OPEN AND CLOSE

                    EditorGUILayout.Space();

                    float aniW1 = 46f; // width of animation type label (move, scale, etc.)
                    float aniW2 = 20f; // width of enabled checkbox
                    float aniW3 = 90f; // width of value setting
                    float aniW4 = 120f; // width of easing type
                    float aniWdur = 40f; // width of duration
                    float aniWdurLabel = 16f; // width of duration label
                    float aniWdel = 40f; // width of delay
                    float aniWdelLabel = 17f; // width of delay label
                    string checkTooltip = "Check To Enable This Animation Type";
                    string durationTooltip = "Duration of this animation property (in seconds)";
                    string delayTooltip = "Start delay of this animation property (in seconds)";

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("ON OPEN ANIMATIONS", boldstyle, GUILayout.Width(160f));
                    //					EditorGUILayout.LabelField( " " , boldstyle, GUILayout.Width (1f), GUILayout.ExpandWidth(true) );
                    EditorGUIUtility.labelWidth = 136f;
                    //					EditorGUILayout.PropertyField (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeInstantlyAtStart"), new GUIContent ("Instant Close At Start", "If set to [TRUE], all close animations will run and then instantly complete before the first frame is rendered. Typically a good idea to enable, especially if setting Menu Controller to Move to On-Screen Position On Awake. This is animation ONLY! No sounds or events will run!"), GUILayout.ExpandWidth(true) );
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Move:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableOpenMove"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openMoveType"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openMoveDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openMoveDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openMoveEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Rotate:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableOpenRotate"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openRotateVariance"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openRotateDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openRotateDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openRotateEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Scale:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableOpenScale"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openScaleVariance"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openScaleDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openScaleDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openScaleEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Alpha:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableOpenAlpha"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openAlphaVariance"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openAlphaDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openAlphaDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("openAlphaEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(15f), GUILayout.ExpandWidth(true));
                    if (GUILayout.Button(new GUIContent("Mirror To Close", "Copies and mirrors all open animation settings to close animation settings, which will cause the close animation to look exactly like the open animation playing in reverse."), GUILayout.MaxWidth(111f)))
                    {
                        serializedObject.ApplyModifiedProperties();
                        ((EasyMenuManager)target).MirrorOpenAnimationSettingsToCloseAnimationSettings(i);
                        serializedObject.Update();
                    }
                    if (GUILayout.Button(new GUIContent("Mirror To Open", "Copies and mirrors all close animation settings to open animation settings, which will cause the open animation to look exactly like the close animation playing in reverse."), GUILayout.MaxWidth(111f)))
                    {
                        serializedObject.ApplyModifiedProperties();
                        ((EasyMenuManager)target).MirrorCloseAnimationSettingsToOpenAnimationSettings(i);
                        serializedObject.Update();
                    }
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(15f), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("ON CLOSE ANIMATIONS", boldstyle, GUILayout.ExpandWidth(true));

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Move:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableCloseMove"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeMoveType"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeMoveDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeMoveDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeMoveEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Rotate:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableCloseRotate"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeRotateVariance"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeRotateDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeRotateDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeRotateEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Scale:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableCloseScale"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeScaleVariance"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeScaleDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeScaleDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeScaleEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("Alpha:", checkTooltip), GUILayout.MaxWidth(aniW1));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("enableCloseAlpha"), GUIContent.none, GUILayout.MaxWidth(aniW2));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeAlphaVariance"), GUIContent.none, GUILayout.MaxWidth(aniW3));
                    EditorGUILayout.LabelField(new GUIContent("Dur", durationTooltip), italicstyle, GUILayout.MaxWidth(aniWdurLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeAlphaDuration"), GUIContent.none, GUILayout.MaxWidth(aniWdur));
                    EditorGUILayout.LabelField(new GUIContent("Del", delayTooltip), italicstyle, GUILayout.MaxWidth(aniWdelLabel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeAlphaDelay"), GUIContent.none, GUILayout.MaxWidth(aniWdel));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("closeAlphaEaseType"), GUIContent.none, GUILayout.MaxWidth(aniW4), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();


                    // Compatibility Menus

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 2f;
                    EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
                    bool allowAllByDefault = emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("allowAllByDefault").boolValue;
                    if (allowAllByDefault == false)
                    {
                        GUI.color = lightpurple;
                        if (GUILayout.Button("Compatible Menus Allowed to Stay Open", GUILayout.MaxWidth(360f)))
                        {
                            allowAllByDefault = true;
                        }
                        GUI.color = Color.white;
                    }
                    else if (allowAllByDefault == true)
                    {
                        GUI.color = lightpink;
                        if (GUILayout.Button("Incompatible Menus Disallowed to Stay Open", GUILayout.MaxWidth(360f)))
                        {
                            allowAllByDefault = false;
                        }
                        GUI.color = Color.white;
                    }
                    emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("allowAllByDefault").boolValue = allowAllByDefault;
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 0;
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    string compTT = "Menus that are compatible with each other will not close each other when the other one opens. [Example: Menu A is compatible with Menu B. Menu A is open. Menu B is then opened. Menu A stays open.] Compatibility Button above must be set to 'Compatible Menus Allowed to Stay Open' for these settings to have any effect.";
                    string incompTT = "Menus that are incompatible with each other will close each other when the other one opens. [Example: Menu A is incompatible with Menu B. Menu A is open. Menu B is then opened. Menu A is then closed.] Compatibility Button above must be set to 'Incompatible Menus Disallowed to Stay Open' for these settings to have any effect.";
                    string prevOpTT = "If enabled for a particular menu, while this menu is open, the other menu cannot be opened by any method. [Example: Menu A has its own Menu B Prevent Opening (PO) setting enabled (Check by Menu B in list). Menu A is open. Menu B is closed. Player attempts to open Menu B. Menu B will not open.]";
                    string prevClTT = "If enabled for a particular menu, while this menu is open, the other menu cannot be closed by any method. [Example: Menu A has its own Menu B Prevent Closing (PC) setting enabled (Check by Menu B in list). Menu A is open. Menu B is open. Player attempts to close Menu B. Menu B will not close.]";

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 50f;
                    EditorGUILayout.LabelField(new GUIContent("C", compTT), boldstyle, GUILayout.MaxWidth(6f));
                    EditorGUILayout.LabelField(new GUIContent("= Compatible", compTT), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField(new GUIContent("I", incompTT), boldstyle, GUILayout.MaxWidth(6f));
                    EditorGUILayout.LabelField(new GUIContent("= Incompatible", incompTT), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(new GUIContent("PO", prevOpTT), boldstyle, GUILayout.MaxWidth(14f));
                    EditorGUILayout.LabelField(new GUIContent("= Prevent Opening", prevOpTT), GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField(new GUIContent("PC", prevClTT), boldstyle, GUILayout.MaxWidth(14f));
                    EditorGUILayout.LabelField(new GUIContent("= Prevent Closing", prevClTT), GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    //					EditorGUIUtility.labelWidth = 15f;
                    EditorGUILayout.LabelField("", boldstyle, GUILayout.MaxWidth(4f));
                    EditorGUILayout.LabelField(new GUIContent("C", compTT), boldstyle, GUILayout.MaxWidth(15f));
                    EditorGUILayout.LabelField("", boldstyle, GUILayout.MaxWidth(10f));
                    EditorGUILayout.LabelField(new GUIContent("I", incompTT), boldstyle, GUILayout.MaxWidth(14f));
                    EditorGUILayout.LabelField("", boldstyle, GUILayout.MaxWidth(5f));
                    EditorGUILayout.LabelField(new GUIContent("PO", prevOpTT), boldstyle, GUILayout.MaxWidth(14f));
                    EditorGUILayout.LabelField("", boldstyle, GUILayout.MaxWidth(10f));
                    EditorGUILayout.LabelField(new GUIContent("PC", prevClTT), boldstyle, GUILayout.MaxWidth(14f));
                    EditorGUILayout.LabelField("", boldstyle, GUILayout.MaxWidth(13f));
                    EditorGUILayout.LabelField("Menu Name", boldstyle, GUILayout.MaxWidth(80f));
                    //					EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();

                    bool check = false;
                    bool set = false;
                    int validMenuCount = 0;
                    for (int o = 0; o < emm_menuList.arraySize; o++)
                    {
                        if (o == i) { continue; }
                        if (emm_menuList.GetArrayElementAtIndex(o).FindPropertyRelative("menuController").objectReferenceValue == null) { continue; }
                        validMenuCount++;
                    }
                    if (validMenuCount <= 0)
                    {
                        EditorGUILayout.LabelField("NO OTHER MENUS. CANNOT CHANGE COMPATIBILITY.", redLabel, GUILayout.ExpandWidth(true));
                    }
                    for (int o = 0; o < emm_menuList.arraySize; o++)
                    {
                        if (o == i) { continue; }
                        if (emm_menuList.GetArrayElementAtIndex(o).FindPropertyRelative("menuController").objectReferenceValue == null) { continue; }

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(1f));

                        // COMP CHECK
                        check = ((EasyMenuManager)target).IsOtherMenuControllerOnThisMenusCompatibilityList(i, o);
                        set = check;
                        GUI.color = lightpurple;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)target).SetCompatibilityBetweenTwoMenus(i, o, true);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        // INCOMP CHECK
                        check = ((EasyMenuManager)target).IsOtherMenuControllerOnThisMenusIncompatibilityList(i, o);
                        set = check;
                        GUI.color = lightpink;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)target).SetCompatibilityBetweenTwoMenus(i, o, false);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        // PREVENT OPENING CHECK
                        check = ((EasyMenuManager)target).IsOtherMenuControllerOnThisMenusPreventList(i, o);
                        set = check;
                        GUI.color = lightorange;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)target).ChangeStatusOfOtherMenuOnThisMenusPreventList(i, o, set);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        // PREVENT CLOSING CHECK
                        check = ((EasyMenuManager)target).IsOtherMenuControllerOnThisMenusPreventClosingList(i, o);
                        set = check;
                        GUI.color = lightyellow;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)target).ChangeStatusOfOtherMenuOnThisMenusPreventClosingList(i, o, set);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        EditorGUILayout.LabelField(emm_menuList.GetArrayElementAtIndex(o).FindPropertyRelative("menuName").stringValue, GUILayout.ExpandWidth(true));


                        //						EditorGUILayout.PropertyField (emm_menuList.GetArrayElementAtIndex(o).FindPropertyRelative("onOpenSoundName"), GUIContent.none, GUILayout.ExpandWidth(true) );


                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 126f;
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runOpenEvents"), new GUIContent("Run Events On Open"), GUILayout.ExpandWidth(true));
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runClosedEvents"), new GUIContent("Run Events On Close"), GUILayout.ExpandWidth(true));
                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();

                    //					EditorGUILayout.BeginHorizontal();
                    //					EditorGUIUtility.labelWidth = 2f;
                    //					if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runOpenEvents").boolValue == true)
                    //					{
                    //						EditorGUILayout.PropertyField (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("whenOpenedDoThis"), new GUIContent("Run These When Opened"), GUILayout.ExpandWidth(true) );
                    //					}
                    //					else
                    //					{
                    //						EditorGUILayout.LabelField( "" , GUILayout.ExpandWidth(true) );
                    //					}
                    //					if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runClosedEvents").boolValue == true)
                    //					{
                    //						EditorGUILayout.PropertyField (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("whenClosedDoThis"), new GUIContent("Run These When Closed"), GUILayout.ExpandWidth(true) );
                    //					}
                    //					else
                    //					{
                    //						EditorGUILayout.LabelField( "" , GUILayout.ExpandWidth(true) );
                    //					}
                    //					EditorGUIUtility.labelWidth = 0;
                    //					EditorGUILayout.EndHorizontal();


                    if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runOpenEvents").boolValue == true)
                    {
                        EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("whenOpenedDoThis"), new GUIContent("Run These When Opened"), GUILayout.ExpandWidth(true));
                    }

                    if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runClosedEvents").boolValue == true)
                    {
                        EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("whenClosedDoThis"), new GUIContent("Run These When Closed"), GUILayout.ExpandWidth(true));
                    }



                }
            }


            //			EditorGUILayout.Space();
            EditorGUILayout.LabelField("----------------------------------------------------------------------------------------------------------------------------------");
            //			EditorGUILayout.Space();
        }





        // END OF MENU LIST

        //		EditorGUILayout.Space();
        //		EditorGUILayout.LabelField( "----------------------------------------------------------------------------------------------------------------------------------" );
        //		EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUI.color = Color.cyan;
        EditorGUIUtility.labelWidth = 2f;
        EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("ADD NEW MENU", GUILayout.MaxWidth(200f)))
        {
            serializedObject.ApplyModifiedProperties();
            ((EasyMenuManager)target).AddNewMenuToMenuList();
            serializedObject.Update();
        }
        EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
        EditorGUIUtility.labelWidth = 0;
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Open/Close Sound Event", boldstyle, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Play Sound: On Open/On Close strings are passed to this event", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("This event must accept a string or it will not be called", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("String value typed here is disregarded completely", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("If no event is set or Play Sound field is blank, event will not run", GUILayout.ExpandWidth(true));

        EditorGUILayout.PropertyField(emm_playSoundEvent, new GUIContent("Event to play sound by string"), GUILayout.ExpandWidth(true));


        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Global On Open Events", boldstyle, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("These events are ran every time ANY MENU IS OPENED", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("These run AFTER menu specific events (if any)", GUILayout.ExpandWidth(true));

        EditorGUILayout.PropertyField(emm_runTheseEventsWhenAnyMenuOpens, new GUIContent("Event(s) to run when any menu opens"), GUILayout.ExpandWidth(true));


        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Global On Close Events", boldstyle, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("These events are ran every time ANY MENU IS CLOSED", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("These run AFTER menu specific events (if any)", GUILayout.ExpandWidth(true));

        EditorGUILayout.PropertyField(emm_runTheseEventsWhenAnyMenuCloses, new GUIContent("Event(s) to run when any menu closes"), GUILayout.ExpandWidth(true));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
