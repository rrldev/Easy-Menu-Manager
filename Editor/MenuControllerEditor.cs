using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[CustomEditor(typeof(MenuController))]
public class MenuControllerEditor : Editor
{

    bool attached;


    GUIStyle titlestyle;
    GUIStyle italicstyle;
    GUIStyle boldstyle;
    Color orange;
    Color lightpurple;
    Color lightpink;
    Color lightorange;
    Color lightyellow;
    GUIStyle redLabel;


    void OnEnable()
    {

        //		boldstyle = new GUIStyle();
        //		boldstyle.fontStyle = FontStyle.Bold;
        //
        //		redLabel = new GUIStyle();
        //		redLabel.normal.textColor = new Color(255/255f, 7/255f, 7/255f, 1);

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
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("EVERY MENU MUST HAVE IT'S ON-SCREEN DEFAULT VALUES SET!", boldstyle);
        EditorGUILayout.LabelField("Setup this menu as it should look when opened,", boldstyle);
        EditorGUILayout.LabelField("then click the yellow button below.", boldstyle);
        EditorGUILayout.LabelField("After any changes to the RectTransform (or parent RectTransforms),", redLabel);
        EditorGUILayout.LabelField("Canvas Group, or any parent Canvases, click this button again!", redLabel);

        GUI.color = Color.yellow;
        if (GUILayout.Button("Set On-Screen Default Values", GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
        {
            serializedObject.ApplyModifiedProperties();
            ((MenuController)target).SetOnScreenPosition();
            serializedObject.Update();
        }
        GUI.color = Color.white;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Default Values:");

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("OffsetMin:", GUILayout.MaxWidth(65f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("offsetMinCenter").vector2Value.x + "," + serializedObject.FindProperty("offsetMinCenter").vector2Value.y + ")", GUILayout.MaxWidth(74f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("OffsetMax:", GUILayout.MaxWidth(65f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("offsetMaxCenter").vector2Value.x + "," + serializedObject.FindProperty("offsetMaxCenter").vector2Value.y + ")", GUILayout.MaxWidth(74f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("AnchorMin:", GUILayout.MaxWidth(65f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("anchorMin").vector2Value.x + "," + serializedObject.FindProperty("anchorMin").vector2Value.y + ")", GUILayout.MaxWidth(74f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("AnchorMax:", GUILayout.MaxWidth(65f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("anchorMax").vector2Value.x + "," + serializedObject.FindProperty("anchorMax").vector2Value.y + ")", GUILayout.MaxWidth(74f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pivot:", GUILayout.MaxWidth(48f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("defaultPivot").vector2Value.x + "," + serializedObject.FindProperty("defaultPivot").vector2Value.y + ")", GUILayout.MaxWidth(74f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("Anchor:", GUILayout.MaxWidth(48f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("anchorCenter").vector2Value.x + "," + serializedObject.FindProperty("anchorCenter").vector2Value.y + ")", GUILayout.MaxWidth(74f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("Alpha:", GUILayout.MaxWidth(48f));
        EditorGUILayout.LabelField("( " + serializedObject.FindProperty("defaultAlpha").floatValue + " )", GUILayout.MaxWidth(62f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Scale:", GUILayout.MaxWidth(65f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("defaultScale").vector3Value.x + "," + serializedObject.FindProperty("defaultScale").vector3Value.y + "," + serializedObject.FindProperty("defaultScale").vector3Value.z + ")", GUILayout.MaxWidth(114f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));

        EditorGUILayout.LabelField("Rotation:", GUILayout.MaxWidth(65f));
        EditorGUILayout.LabelField("(" + serializedObject.FindProperty("defaultRotation").vector3Value.x + "," + serializedObject.FindProperty("defaultRotation").vector3Value.y + "," + serializedObject.FindProperty("defaultRotation").vector3Value.z + ")", GUILayout.MaxWidth(114f));
        EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(1f), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        //		EditorGUILayout.LabelField( "----------------------------------------------------------------------------------------------------------------------------------" );
        EditorGUILayout.Space();

        //		EditorGUILayout.LabelField("This will set a custom close position for use with");
        //		EditorGUILayout.LabelField("the Custom setting of open/Close Move animations.");
        //		EditorGUILayout.LabelField("Position menu as it should look when closed then click this button.");
        if (GUILayout.Button(new GUIContent("Set Custom 1 Off-Screen Position", "This will set a custom close position for use with the Custom 1 setting of the Open/Close Move animations. Position menu as it should look when closed then click this button. This has no effect if move animation is not set to Custom 1."), GUILayout.ExpandWidth(true)))
        {
            serializedObject.ApplyModifiedProperties();
            ((MenuController)target).SetCustomOffScreenPosition();
            serializedObject.Update();
        }
        if (GUILayout.Button(new GUIContent("Set Custom 2 Off-Screen Position", "This will set a custom close position for use with the Custom 2 setting of the Open/Close Move animations. Position menu as it should look when closed then click this button. This has no effect if move animation is not set to Custom 2."), GUILayout.ExpandWidth(true)))
        {
            serializedObject.ApplyModifiedProperties();
            ((MenuController)target).SetCustom2OffScreenPosition();
            serializedObject.Update();
        }

        EditorGUILayout.Space();
        //		EditorGUILayout.LabelField( "----------------------------------------------------------------------------------------------------------------------------------" );
        EditorGUILayout.Space();

        if (GUILayout.Button("Move To Current On-Screen Default Values", GUILayout.ExpandWidth(true)))
        {
            serializedObject.ApplyModifiedProperties();
            ((MenuController)target).MoveToOnScreenPosition();
            serializedObject.Update();
        }


        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //		EditorGUILayout.BeginHorizontal();
        //		EditorGUILayout.PropertyField (serializedObject.FindProperty ("centerOnAwake"), GUIContent.none, GUILayout.MaxWidth(12f) );
        //		EditorGUILayout.LabelField( new GUIContent( " Move To On-Screen Position At Start", "Enabling this will move this menu to it's on-screen position once the Easy Menu Manager starts (By default, on the first frame of the scene). This is useful if you want to keep your UI elements enabled and visible so they can be editted, but move them off-screen so they do not overlap the game elements." ));
        //		EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("disableInteractable"), GUIContent.none, GUILayout.MaxWidth(12f));
        EditorGUILayout.LabelField(" Disable Interactable UI Elements When Closed");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("disableRaycasts"), GUIContent.none, GUILayout.MaxWidth(12f));
        EditorGUILayout.LabelField(" Disable Raycast Collision When Closed");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dontEnableIfTweening"), GUIContent.none, GUILayout.MaxWidth(12f));
        EditorGUILayout.LabelField(new GUIContent(" Don't Enable Interactable/Raycast Until Open Animation End", "Enabling this will keep UI elements non-interactable (if set to disable above) and keep raycasts from hitting UI elements (if set to disable above) until any playing open animations have completed (set in Easy Menu Manager)."));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();



        // MENU LIST STUFF COPIED OVER FROM ORIGINAL MENU MANAGER


        if (serializedObject.FindProperty("attached").boolValue == true && serializedObject.FindProperty("childController").boolValue == false)
        {


            serializedObject.ApplyModifiedProperties();
            SerializedObject emm = new SerializedObject(((MenuController)target).parentMM);
            SerializedProperty emm_menuList = emm.FindProperty("menuList");
            int i = ((MenuController)target).parentMM.GetMenuIndexOfMenuController((MenuController)target);
            serializedObject.Update();
            emm.ApplyModifiedProperties();
            emm.Update();

            serializedObject.ApplyModifiedProperties();
            emm.ApplyModifiedProperties();
            if (((EasyMenuManager)emm.targetObject).IsMenuControllerinMenuList((MenuController)target) == false)
            {
                ((MenuController)target).SetParentsNull();
                return;
            }
            serializedObject.Update();
            emm.Update();

            //			EditorGUILayout.BeginHorizontal();
            //			EditorGUIUtility.labelWidth = 100f;
            //			EditorGUILayout.PropertyField (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController"), new GUIContent ("Menu Controller"), GUILayout.ExpandWidth(true) );
            //			EditorGUIUtility.labelWidth = 2f;
            //			GUI.color = Color.red;
            //			EditorGUILayout.LabelField( "", GUILayout.ExpandWidth(true), GUILayout.MaxWidth (60f) );
            //			EditorGUIUtility.labelWidth = 0;
            //			if (GUILayout.Button("DEL", GUILayout.MaxWidth (60f)) )
            //			{
            //				serializedObject.ApplyModifiedProperties();
            //				((EasyMenuManager) target).EraseCompatibilityEntriesOfDeletedMenuList(i);
            //				serializedObject.Update();
            //				
            //				emm_menuList.DeleteArrayElementAtIndex(i);
            //				serializedObject.ApplyModifiedProperties();
            //				
            //				GUI.color = Color.white;
            //				EditorGUILayout.EndHorizontal();
            //				return;
            //			}
            //			GUI.color = Color.white;
            //			EditorGUILayout.EndHorizontal();

            //			if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue == null)
            //			{
            //				EditorGUILayout.LabelField( "Set MenuController that is not currently in this list", redLabel, GUILayout.ExpandWidth(true)) ;
            //			}

            if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue != null)
            {

                if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("firstNamed").boolValue == false)
                {
                    emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("firstNamed").boolValue = true;
                    emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuName").stringValue = emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("menuController").objectReferenceValue.name.ToString();
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    emm.ApplyModifiedProperties();
                    emm.Update();
                }


                serializedObject.ApplyModifiedProperties();
                emm.ApplyModifiedProperties();
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 20;
                EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Select Parent Easy Menu Manager", GUILayout.MaxWidth(260f), GUILayout.ExpandWidth(false)))
                {
                    Selection.activeObject = (emm.targetObject);
                    return;
                }
                EditorGUILayout.LabelField("", GUILayout.ExpandWidth(true));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();
                serializedObject.Update();
                emm.Update();
                EditorGUILayout.Space();

                // OPEN AND CLOSE BUTTONS WHEN PLAYER IS RUNNING
                if (Application.isPlaying)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 1f;
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));
                    if (GUILayout.Button("Send 'Open' Command", GUILayout.MaxWidth(180f), GUILayout.ExpandWidth(false)))
                    {
                        ((EasyMenuManager)emm.targetObject).OpenMenu((MenuController)target);
                    }
                    EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));
                    if (GUILayout.Button("Send 'Close' Command", GUILayout.MaxWidth(180f), GUILayout.ExpandWidth(false)))
                    {
                        ((EasyMenuManager)emm.targetObject).CloseMenu((MenuController)target);
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
                    EditorGUILayout.PropertyField(emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("disableWhenClosed"), new GUIContent("Hide On Close", "If set to [TRUE], the game object the menu controller is attached to will be moved very far off-screen and all child game objects containing a CanvasRenderer will be disabled after all close animations are finished. This should help reduce SetPass calls."), GUILayout.ExpandWidth(true));
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
                        ((EasyMenuManager)emm.targetObject).MirrorOpenAnimationSettingsToCloseAnimationSettings(i);
                        serializedObject.Update();
                    }
                    if (GUILayout.Button(new GUIContent("Mirror To Open", "Copies and mirrors all close animation settings to open animation settings, which will cause the open animation to look exactly like the close animation playing in reverse."), GUILayout.MaxWidth(111f)))
                    {
                        serializedObject.ApplyModifiedProperties();
                        ((EasyMenuManager)emm.targetObject).MirrorCloseAnimationSettingsToOpenAnimationSettings(i);
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
                        check = ((EasyMenuManager)emm.targetObject).IsOtherMenuControllerOnThisMenusCompatibilityList(i, o);
                        set = check;
                        GUI.color = lightpurple;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)emm.targetObject).SetCompatibilityBetweenTwoMenus(i, o, true);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        // INCOMP CHECK
                        check = ((EasyMenuManager)emm.targetObject).IsOtherMenuControllerOnThisMenusIncompatibilityList(i, o);
                        set = check;
                        GUI.color = lightpink;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)emm.targetObject).SetCompatibilityBetweenTwoMenus(i, o, false);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        // PREVENT OPENING CHECK
                        check = ((EasyMenuManager)emm.targetObject).IsOtherMenuControllerOnThisMenusPreventList(i, o);
                        set = check;
                        GUI.color = lightorange;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)emm.targetObject).ChangeStatusOfOtherMenuOnThisMenusPreventList(i, o, set);
                            serializedObject.Update();
                        }
                        EditorGUILayout.LabelField("", GUILayout.MaxWidth(10f));

                        // PREVENT CLOSING CHECK
                        check = ((EasyMenuManager)emm.targetObject).IsOtherMenuControllerOnThisMenusPreventClosingList(i, o);
                        set = check;
                        GUI.color = lightyellow;
                        set = EditorGUILayout.Toggle(set, GUILayout.MaxWidth(14f));
                        GUI.color = Color.white;
                        if (set != check) // BOOL WAS TOGGLED
                        {
                            serializedObject.ApplyModifiedProperties();
                            ((EasyMenuManager)emm.targetObject).ChangeStatusOfOtherMenuOnThisMenusPreventClosingList(i, o, set);
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


                    serializedObject.ApplyModifiedProperties();
                    emm.ApplyModifiedProperties();
                    ((MenuController)(target)).openEvents = ((EasyMenuManager)emm.targetObject).menuList[i].whenOpenedDoThis;
                    ((MenuController)(target)).closeEvents = ((EasyMenuManager)emm.targetObject).menuList[i].whenClosedDoThis;
                    serializedObject.Update();
                    emm.Update();

                    if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runOpenEvents").boolValue == true)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("openEvents"), new GUIContent("Run These Events When Opened"), GUILayout.ExpandWidth(true));
                    }

                    if (emm_menuList.GetArrayElementAtIndex(i).FindPropertyRelative("runClosedEvents").boolValue == true)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("closeEvents"), new GUIContent("Run These Events When Closed"), GUILayout.ExpandWidth(true));
                    }

                    serializedObject.ApplyModifiedProperties();
                    emm.ApplyModifiedProperties();
                    ((EasyMenuManager)emm.targetObject).menuList[i].whenOpenedDoThis = ((MenuController)(target)).openEvents;
                    ((EasyMenuManager)emm.targetObject).menuList[i].whenClosedDoThis = ((MenuController)(target)).closeEvents;
                    serializedObject.Update();
                    emm.Update();



                }
            }

            emm.ApplyModifiedProperties();

        }





        serializedObject.ApplyModifiedProperties();



    }
}
