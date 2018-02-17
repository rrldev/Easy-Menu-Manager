using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DG.Tweening;

public class EasyMenuManager : MonoBehaviour
{

    public List<MenuProperties> menuList;

    public UnityEvent runTheseEventsWhenAnyMenuOpens;
    public UnityEvent runTheseEventsWhenAnyMenuCloses;

    Vector2 moveVec;
    Vector3 rotateVec;
    Vector3 scaleVec;
    Vector3 hideVec;

    public bool preventAllMenusFromOpening;
    public bool preventAllMenusFromClosing;

    public UnityEvent playSoundEvent; // This event will be passed a string (sound file) and ran if it is called by each individual menus play sound settings.

    public string soundToPlay;
    System.Type[] sta;// = new System.Type[] {typeof(string)};

    public List<MenuProperties> menusToBeClosed; // List of menus to be closed at once (called from compatibility checks and CloseAllMenus function)

    List<MenuProperties> menusClosedToBeDisabled; // List of menus recently closed that need to be disabled (if disableWhenClosed = true) after their close animations are finished.
    List<MenuProperties> queuedToOpen;
    List<MenuProperties> queuedToClose;

    void Start()
    {

        menusClosedToBeDisabled = new List<MenuProperties>();
        queuedToOpen = new List<MenuProperties>();
        queuedToClose = new List<MenuProperties>();

        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController != null)
            {
                menuList[i].menuController.MenuControllerStart();
                SetStartStateOfMenu(menuList[i]);
            }
        }

    }


    void SetStartStateOfMenu(MenuProperties mp)
    {
        if (mp.startState == EasyMenuManagerStartState.Close)
        {
            ForceClose(mp, true, true, false, false);
        }
        else if (mp.startState == EasyMenuManagerStartState.CloseInstant)
        {
            ForceClose(mp, true, true, false, true);
        }
        else if (mp.startState == EasyMenuManagerStartState.CloseNoSound)
        {
            ForceClose(mp, false, true, false, false);
        }
        else if (mp.startState == EasyMenuManagerStartState.CloseInstantNoSound)
        {
            ForceClose(mp, false, true, false, true);
        }
        else if (mp.startState == EasyMenuManagerStartState.Open)
        {
            ForceOpen(mp, true, true, false, false);
        }
        else if (mp.startState == EasyMenuManagerStartState.OpenInstant)
        {
            ForceOpen(mp, true, true, false, true);
        }
        else if (mp.startState == EasyMenuManagerStartState.OpenNoSound)
        {
            ForceOpen(mp, false, true, false, false);
        }
        else if (mp.startState == EasyMenuManagerStartState.OpenInstantNoSound)
        {
            ForceOpen(mp, false, true, false, true);
        }
    }





    public void InstantlyOpenMenu(string menuName)
    {
        InstantlyOpenMenuByProperties(GetMenuPropertiesByName(menuName));
    }
    public void InstantlyOpenMenu(MenuController menuController)
    {
        InstantlyOpenMenuByProperties(GetMenuPropertiesByController(menuController));
    }
    void InstantlyOpenMenuByProperties(MenuProperties mp)
    {
        if (mp == null)
        {
            Debug.LogError("Instant Open Menu Error! Null reference was passed to function!");
            return;
        }
        OpenMenuByProperty(mp);
        InstantlyFinishAnyRunningOpenAnimations(mp);
    }

    public void InstantlyCloseMenu(string menuName)
    {
        InstantlyCloseMenuByProperties(GetMenuPropertiesByName(menuName));
    }
    public void InstantlyCloseMenu(MenuController menuController)
    {
        InstantlyCloseMenuByProperties(GetMenuPropertiesByController(menuController));
    }
    void InstantlyCloseMenuByProperties(MenuProperties mp)
    {
        if (mp == null)
        {
            Debug.LogError("Instant Close Menu Error! Null reference was passed to function!");
            return;
        }
        CloseMenuByProperty(mp);
        InstantlyFinishAnyRunningCloseAnimations(mp);
    }








    public MenuProperties GetMenuPropertiesByName(string menuName)
    {
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuName == menuName)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when getting menuProperties by name! Returning first found! Check for name conflicts!"); }
        return mpmp;
    }
    public MenuProperties GetMenuPropertiesByController(MenuController menuController)
    {
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController == menuController)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when getting menuProperties by menuController! Returning first found! Check for menuController conflicts!"); }
        return mpmp;
    }






    public class EMMSoundStringEvent : UnityEvent<string> { }

    public void PlayMenuSoundByGlobalEvent(MenuProperties menuProp, bool trueForOpenFalseForClose)
    {
        if (playSoundEvent.GetPersistentEventCount() <= 0) { return; } // NO SOUND EVENT. CANNOT PLAY SOUND.
        if (trueForOpenFalseForClose == true && string.IsNullOrEmpty(menuProp.onOpenSoundName)) { return; } // NO OPEN SOUND. CANNOT PLAY SOUND.
        if (trueForOpenFalseForClose == false && string.IsNullOrEmpty(menuProp.onCloseSoundName)) { return; } // NO CLOSE SOUND. CANNOT PLAY SOUND.
        if (sta == null)
            sta = new System.Type[] { typeof(string) };
        for (int i = 0; i < playSoundEvent.GetPersistentEventCount(); i++)
        {
            if (EMMSoundStringEvent.GetValidMethodInfo(playSoundEvent.GetPersistentTarget(i), playSoundEvent.GetPersistentMethodName(i), sta) != null)
            {
                if (trueForOpenFalseForClose) { ((Component)playSoundEvent.GetPersistentTarget(i)).SendMessage(playSoundEvent.GetPersistentMethodName(i), menuProp.onOpenSoundName); }
                else { ((Component)playSoundEvent.GetPersistentTarget(i)).SendMessage(playSoundEvent.GetPersistentMethodName(i), menuProp.onCloseSoundName); }
            }
        }

    }

    void Update()
    {
        for (int i = queuedToOpen.Count - 1; i >= 0; i--)
        {
            if (queuedToOpen[i].menuController.AreAnyOpenOrCloseTweensRunning(false) == false)
            {
                MenuProperties qm = queuedToOpen[i];
                queuedToOpen.RemoveAt(i);
                OpenMenuByProperty(qm);
            }
        }
        for (int i = queuedToClose.Count - 1; i >= 0; i--)
        {
            if (queuedToClose[i].menuController.AreAnyOpenOrCloseTweensRunning(true) == false)
            {
                MenuProperties qm = queuedToClose[i];
                queuedToClose.RemoveAt(i);
                CloseMenuByProperty(qm);
            }
        }
    }


    void LateUpdate()
    {
        if (menusToBeClosed != null && menusToBeClosed.Count > 0)
        {
            CloseMenusToBeClosed();
            menusToBeClosed.Clear();
        }

        if (menusClosedToBeDisabled != null)
        {
            for (int i = menusClosedToBeDisabled.Count - 1; i >= 0; i--)
            {
                if (menusClosedToBeDisabled[i].isOpen == false && menusClosedToBeDisabled[i].menuController.AreAnyOpenOrCloseTweensRunning() == false)
                {
                    //					menusClosedToBeDisabled[i].menuController.gameObject.SetActive(false);
                    hideVec = menusClosedToBeDisabled[i].menuController.anchorCenter;
                    hideVec.x += (10) * menusClosedToBeDisabled[i].menuController.screenSizeinCanvasUnits.x;
                    hideVec.y += (10) * menusClosedToBeDisabled[i].menuController.screenSizeinCanvasUnits.y;
                    menusClosedToBeDisabled[i].menuController.selfRect.anchoredPosition = hideVec;

                    menusClosedToBeDisabled[i].menuController.ToggleRenderers(false);
                    menusClosedToBeDisabled[i].disabledAfterLastClose = true;

                    menusClosedToBeDisabled.RemoveAt(i);
                }
                else if (menusClosedToBeDisabled[i].isOpen)
                {
                    menusClosedToBeDisabled.RemoveAt(i);
                }
            }
        }
    }



    public bool IsMenuOpen(string menuName, bool returnFalseIfTweening = false)
    {
        return IsMenuOpenByProperty(GetMenuPropertiesByName(menuName), returnFalseIfTweening);
    }
    public bool IsMenuOpen(MenuController menuController, bool returnFalseIfTweening = false)
    {
        return IsMenuOpenByProperty(GetMenuPropertiesByController(menuController), returnFalseIfTweening);
    }
    bool IsMenuOpenByProperty(MenuProperties mp, bool returnFalseIfTweening = false)
    {
        if (returnFalseIfTweening && mp.menuController.AreAnyOpenOrCloseTweensRunning()) { return false; }
        return mp.isOpen;
    }



    public void OpenMenu(string menuName)
    {
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuName == menuName)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when trying to open menu by name! Opening first only! Check for name conflicts!"); }
        if (mpmp != null) { OpenMenuByProperty(mpmp); }
    }
    public void OpenMenu(MenuController menuController)
    {
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController == menuController)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when trying to open menu by menuController! Opening first only! Check for menuController conflicts!"); }
        if (mpmp != null) { OpenMenuByProperty(mpmp); }
    }

    void OpenMenuByProperty(MenuProperties mp)
    {
        if (mp.menuController != null)
        {
            //			if (queuedToClose.Contains(mp) ) {queuedToClose.Remove (mp);}
            if (preventAllMenusFromOpening) { return; }

            if (mp.isOpen)
            {
                if (queuedToClose.Contains(mp) == false)
                {
                    if (mp.closeWhenOpenedWhileOpen) { CloseMenuByProperty(mp); }
                    return;
                }
                else
                {
                    queuedToClose.Remove(mp);
                    return;
                }
            }
            else if (mp.isOpen == false && queuedToOpen.Contains(mp))
            {
                if (mp.closeWhenOpenedWhileOpen)
                {
                    if (queuedToOpen.Contains(mp)) { queuedToOpen.Remove(mp); }
                    CloseMenuByProperty(mp);
                }
                return;
            }

            if (mp.preventOpeningWhileClosing && mp.menuController.AreAnyOpenOrCloseTweensRunning(false))
            {
                if (mp.queueOpeningWhenPreventing && queuedToOpen.Contains(mp) == false) { queuedToOpen.Add(mp); }
                return;
            }


            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].isOpen && menuList[i].preventedMenus.Contains(mp.menuController) && menusToBeClosed.Contains(menuList[i]) == false) // OTHER MENU is open and has this menu on its Prevented Menus List.
                {
                    return;
                }
            }

            if (queuedToClose.Contains(mp)) { queuedToClose.Remove(mp); }

            //			mp.menuController.gameObject.SetActive(true);
            //			mp.menuController.MoveToOnScreenPosition();

            if (mp.disabledAfterLastClose)
                mp.menuController.ToggleRenderers(true);
            mp.disabledAfterLastClose = false;

            mp.menuController.IsOpen = true;
            mp.isOpen = true;

            PlayMenuSoundByGlobalEvent(mp, true);

            RunOnOpenAnimationActions(mp);

            if (mp.runOpenEvents)
                mp.whenOpenedDoThis.Invoke();

            if (mp.allowAllByDefault == false) // close every menu NOT on the compatible menus list
            {
                for (int i = 0; i < menuList.Count; i++)
                {
                    if (menuList[i] == mp || mp.compatibleMenus.Contains(menuList[i].menuController))
                    {

                    }
                    else
                    {
                        AddMenuToMenusToBeClosedList(menuList[i]);
                    }
                }
            }
            else // close every menu on the incompatible list
            {
                for (int i = 0; i < menuList.Count; i++)
                {
                    if (menuList[i] == mp || mp.incompatibleMenus.Contains(menuList[i].menuController) == false)
                    {

                    }
                    else
                    {
                        AddMenuToMenusToBeClosedList(menuList[i]);
                    }
                }
            }


            runTheseEventsWhenAnyMenuOpens.Invoke();

        }
    }

    /// <summary>
    /// Forces a menu to open. Should never need to use this. Does not trigger closing of other menus based on compatibility. Cannot be prevented by any other menu's settings.
    /// </summary>
    public void ForceOpen(MenuProperties mp, bool playSounds = true, bool playAnimations = true, bool triggerEvents = true, bool instantlyCompleteAnimations = false)
    {
        if (mp.menuController != null)
        {
            mp.menuController.IsOpen = true;
            mp.isOpen = true;
            if (playSounds)
                PlayMenuSoundByGlobalEvent(mp, true);
            if (playAnimations)
                RunOnOpenAnimationActions(mp);
            else
            {
                if (mp.menuController.closeMoveTween != null) { mp.menuController.closeMoveTween.Complete(); }
                if (mp.menuController.closeRotateTween != null) { mp.menuController.closeRotateTween.Complete(); }
                if (mp.menuController.closeScaleTween != null) { mp.menuController.closeScaleTween.Complete(); }
                if (mp.menuController.closeAlphaTween != null) { mp.menuController.closeAlphaTween.Complete(); }
                mp.menuController.MoveToOnScreenPosition();
            }
            if (instantlyCompleteAnimations) { InstantlyFinishAnyRunningOpenAnimations(mp); }
            if (triggerEvents && mp.runOpenEvents)
                mp.whenOpenedDoThis.Invoke();
            if (triggerEvents)
                runTheseEventsWhenAnyMenuOpens.Invoke();
        }
    }



    public void CloseMenu(string menuName)
    {
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuName == menuName)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when trying to close menu by name! Closing first only! Check for name conflicts!"); }
        if (mpmp != null) { CloseMenuByProperty(mpmp); }
    }
    public void CloseMenu(MenuController menuController)
    {
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController == menuController)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when trying to close menu by menuController! Closing first only! Check for menuController conflicts!"); }
        if (mpmp != null) { CloseMenuByProperty(mpmp); }
    }

    void CloseMenuByProperty(MenuProperties mp)
    {
        if (mp.menuController != null)
        {
            //			if (queuedToOpen.Contains(mp) ) {queuedToOpen.Remove (mp);}
            if (preventAllMenusFromClosing) { return; }

            if (mp.isOpen == false) { return; }

            if (mp.preventClosingWhileOpening && mp.menuController.AreAnyOpenOrCloseTweensRunning(true))
            {
                if (mp.queueClosingWhenPreventing && queuedToClose.Contains(mp) == false) { queuedToClose.Add(mp); }
                return;
            }

            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].isOpen && menuList[i].preventedMenusClosing.Contains(mp.menuController) && menusToBeClosed.Contains(menuList[i]) == false) // OTHER MENU is open and has this menu on its Prevented CLOSING Menus List.
                {
                    return;
                }
            }

            if (queuedToOpen.Contains(mp)) { queuedToOpen.Remove(mp); }

            mp.menuController.IsOpen = false;
            mp.isOpen = false;

            PlayMenuSoundByGlobalEvent(mp, false);

            RunOnCloseAnimationActions(mp);

            if (mp.runClosedEvents)
                mp.whenClosedDoThis.Invoke();


            runTheseEventsWhenAnyMenuCloses.Invoke();

            if (mp.disableWhenClosed) { menusClosedToBeDisabled.Add(mp); }
        }
    }

    /// <summary>
    /// Forces a menu to close. Should never need to use this. Cannot be prevented by any other menu's settings.
    /// </summary>
    public void ForceClose(MenuProperties mp, bool playSounds = true, bool playAnimations = true, bool triggerEvents = true, bool instantlyCompleteAnimations = false)
    {
        if (mp.menuController != null)
        {
            mp.menuController.IsOpen = false;
            mp.isOpen = false;
            if (playSounds)
                PlayMenuSoundByGlobalEvent(mp, false);
            if (playAnimations)
                RunOnCloseAnimationActions(mp);
            else
            {
                if (mp.menuController.openMoveTween != null) { mp.menuController.openMoveTween.Complete(); }
                if (mp.menuController.openRotateTween != null) { mp.menuController.openRotateTween.Complete(); }
                if (mp.menuController.openScaleTween != null) { mp.menuController.openScaleTween.Complete(); }
                if (mp.menuController.openAlphaTween != null) { mp.menuController.openAlphaTween.Complete(); }
            }
            if (instantlyCompleteAnimations) { InstantlyFinishAnyRunningCloseAnimations(mp); }
            if (triggerEvents && mp.runClosedEvents)
                mp.whenClosedDoThis.Invoke();
            if (triggerEvents)
                runTheseEventsWhenAnyMenuCloses.Invoke();
            if (mp.disableWhenClosed) { menusClosedToBeDisabled.Add(mp); }
        }
    }

    public void CloseAllMenus()
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            AddMenuToMenusToBeClosedList(menuList[i]);
        }
        CloseMenusToBeClosed();
    }
    public void CloseAllMenusExcludingOne(string menuName)
    {
        CloseAllMenusExcludingOneByMenuProperties(GetMenuPropertiesByName(menuName));
    }
    public void CloseAllMenusExcludingOne(MenuController menuController)
    {
        CloseAllMenusExcludingOneByMenuProperties(GetMenuPropertiesByController(menuController));
    }
    void CloseAllMenusExcludingOneByMenuProperties(MenuProperties mp)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i] != mp) { AddMenuToMenusToBeClosedList(menuList[i]); }
        }
        CloseMenusToBeClosed();
    }


    public void AddMenuByNameToMenusToBeClosedList(string menuName)
    {
        if (menusToBeClosed == null) { menusToBeClosed = new List<MenuProperties>(); }
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuName == menuName)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when trying to add menu to close list by name! Adding first only! Check for name conflicts!"); }
        if (mpmp != null) { AddMenuToMenusToBeClosedList(mpmp); }
    }
    public void AddMenuByControllerToMenusToBeClosedList(MenuController menuController)
    {
        if (menusToBeClosed == null) { menusToBeClosed = new List<MenuProperties>(); }
        int ccc = 0;
        MenuProperties mpmp = null;
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController == menuController)
            {
                ccc++;
                if (mpmp == null) { mpmp = menuList[i]; }
            }
        }
        if (ccc > 1) { Debug.LogWarning("Multiple menus found when trying to add menu to close list by controller! Adding first only! Check for menuController conflicts!"); }
        if (mpmp != null) { AddMenuToMenusToBeClosedList(mpmp); }
    }

    void AddMenuToMenusToBeClosedList(MenuProperties menuToClose)
    {
        if (menusToBeClosed == null) { menusToBeClosed = new List<MenuProperties>(); }
        menusToBeClosed.Add(menuToClose);
    }

    public void CloseMenusToBeClosed()
    {
        for (int i = 0; i < menusToBeClosed.Count; i++)
        {
            CloseMenuByProperty(menusToBeClosed[i]);
        }
        menusToBeClosed.Clear();
    }

    public void AddNewMenuToMenuList()
    {
        if (menuList == null) { menuList = new List<MenuProperties>(); }
        menuList.Add(new MenuProperties());
    }

    public bool IsOtherMenuControllerOnThisMenusCompatibilityList(int thisMenuNum, int otherMenuNum)
    {
        if (menuList[thisMenuNum].compatibleMenus.Contains(menuList[otherMenuNum].menuController)) { return true; }
        else { return false; }
    }
    public bool IsOtherMenuControllerOnThisMenusIncompatibilityList(int thisMenuNum, int otherMenuNum)
    {
        if (menuList[thisMenuNum].incompatibleMenus.Contains(menuList[otherMenuNum].menuController)) { return true; }
        else { return false; }
    }
    public bool IsOtherMenuControllerOnThisMenusPreventList(int thisMenuNum, int otherMenuNum)
    {
        if (menuList[thisMenuNum].preventedMenus.Contains(menuList[otherMenuNum].menuController)) { return true; }
        else { return false; }
    }
    public bool IsOtherMenuControllerOnThisMenusPreventClosingList(int thisMenuNum, int otherMenuNum)
    {
        if (menuList[thisMenuNum].preventedMenusClosing.Contains(menuList[otherMenuNum].menuController)) { return true; }
        else { return false; }
    }

    public void SetCompatibilityBetweenTwoMenus(int menuJustChanged, int otherMenu, bool trueForCompListFalseForIncompList)
    {
        if (menuList.Count <= menuJustChanged || menuList.Count <= otherMenu) { return; }

        if (trueForCompListFalseForIncompList)
        {
            if (menuList[menuJustChanged].compatibleMenus.Contains(menuList[otherMenu].menuController) == false)
            {
                menuList[menuJustChanged].compatibleMenus.Add(menuList[otherMenu].menuController);
                menuList[otherMenu].compatibleMenus.Add(menuList[menuJustChanged].menuController);
            }
            else
            {
                menuList[menuJustChanged].compatibleMenus.Remove(menuList[otherMenu].menuController);
                menuList[otherMenu].compatibleMenus.Remove(menuList[menuJustChanged].menuController);
            }
        }
        else
        {
            if (menuList[menuJustChanged].incompatibleMenus.Contains(menuList[otherMenu].menuController) == false)
            {
                menuList[menuJustChanged].incompatibleMenus.Add(menuList[otherMenu].menuController);
                menuList[otherMenu].incompatibleMenus.Add(menuList[menuJustChanged].menuController);
            }
            else
            {
                menuList[menuJustChanged].incompatibleMenus.Remove(menuList[otherMenu].menuController);
                menuList[otherMenu].incompatibleMenus.Remove(menuList[menuJustChanged].menuController);
            }
        }
    }

    public void EraseCompatibilityEntriesOfDeletedMenuList(int deletedMenu)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[deletedMenu] == menuList[i]) { continue; }
            if (menuList[i].compatibleMenus.Contains(menuList[deletedMenu].menuController))
                menuList[i].compatibleMenus.Remove(menuList[deletedMenu].menuController);
            if (menuList[i].incompatibleMenus.Contains(menuList[deletedMenu].menuController))
                menuList[i].incompatibleMenus.Remove(menuList[deletedMenu].menuController);
            if (menuList[i].preventedMenus.Contains(menuList[deletedMenu].menuController))
                menuList[i].preventedMenus.Remove(menuList[deletedMenu].menuController);
            if (menuList[i].preventedMenusClosing.Contains(menuList[deletedMenu].menuController))
                menuList[i].preventedMenusClosing.Remove(menuList[deletedMenu].menuController);
        }
    }


    public void ChangeStatusOfOtherMenuOnThisMenusPreventList(int thisMenuNum, int otherMenuNum, bool trueForAddFalseForRemove)
    {
        if (menuList.Count <= thisMenuNum || menuList.Count <= otherMenuNum) { return; }

        if (trueForAddFalseForRemove && menuList[thisMenuNum].preventedMenus.Contains(menuList[otherMenuNum].menuController) == false)
        {
            menuList[thisMenuNum].preventedMenus.Add(menuList[otherMenuNum].menuController);
        }
        else if (menuList[thisMenuNum].preventedMenus.Contains(menuList[otherMenuNum].menuController) == true)
        {
            menuList[thisMenuNum].preventedMenus.Remove(menuList[otherMenuNum].menuController);
        }
    }

    public void ChangeStatusOfOtherMenuOnThisMenusPreventClosingList(int thisMenuNum, int otherMenuNum, bool trueForAddFalseForRemove)
    {
        if (menuList.Count <= thisMenuNum || menuList.Count <= otherMenuNum) { return; }

        if (trueForAddFalseForRemove && menuList[thisMenuNum].preventedMenusClosing.Contains(menuList[otherMenuNum].menuController) == false)
        {
            menuList[thisMenuNum].preventedMenusClosing.Add(menuList[otherMenuNum].menuController);
        }
        else if (menuList[thisMenuNum].preventedMenusClosing.Contains(menuList[otherMenuNum].menuController) == true)
        {
            menuList[thisMenuNum].preventedMenusClosing.Remove(menuList[otherMenuNum].menuController);
        }
    }



    // ANIMATION ACTIONS

    public void RunOnOpenAnimations(string menuName)
    {
        RunOnOpenAnimationActions(GetMenuPropertiesByName(menuName));
    }
    public void RunOnOpenAnimations(MenuController menuController)
    {
        RunOnOpenAnimationActions(GetMenuPropertiesByController(menuController));
    }
    void RunOnOpenAnimationActions(MenuProperties mp)
    {
        if (!mp.enableOpenMove && mp.menuController.closeMoveTween != null) { mp.menuController.closeMoveTween.Complete(); }
        if (!mp.enableOpenRotate && mp.menuController.closeRotateTween != null) { mp.menuController.closeRotateTween.Complete(); }
        if (!mp.enableOpenScale && mp.menuController.closeScaleTween != null) { mp.menuController.closeScaleTween.Complete(); }
        if (!mp.enableOpenAlpha && mp.menuController.closeAlphaTween != null) { mp.menuController.closeAlphaTween.Complete(); }

        mp.menuController.MoveToOnScreenPosition();

        if (mp.enableOpenMove)
        {
            //			if (mp.menuController.openMoveTween != null) {mp.menuController.openMoveTween.Complete();}
            if (mp.menuController.closeMoveTween != null) { mp.menuController.closeMoveTween.Kill(); }
            if (mp.menuController.selfRect != null)
            {
                moveVec = mp.menuController.anchorCenter;

                if (mp.openMoveType == EasyMenuManagerMoveTransitionType.Left || mp.openMoveType == EasyMenuManagerMoveTransitionType.TopLeft || mp.openMoveType == EasyMenuManagerMoveTransitionType.BottomLeft)
                {
                    moveVec.x += !mp.uniformOpenMoveAnimation ? (-1) * mp.menuController.screenSizeinCanvasUnits.x : mp.menuController.screenSizeinCanvasUnits.x;
                }
                if (mp.openMoveType == EasyMenuManagerMoveTransitionType.Right || mp.openMoveType == EasyMenuManagerMoveTransitionType.TopRight || mp.openMoveType == EasyMenuManagerMoveTransitionType.BottomRight)
                {
                    moveVec.x += !mp.uniformOpenMoveAnimation ? mp.menuController.screenSizeinCanvasUnits.x : mp.menuController.screenSizeinCanvasUnits.x;
                }
                if (mp.openMoveType == EasyMenuManagerMoveTransitionType.Bottom || mp.openMoveType == EasyMenuManagerMoveTransitionType.BottomLeft || mp.openMoveType == EasyMenuManagerMoveTransitionType.BottomRight)
                {
                    moveVec.y += !mp.uniformOpenMoveAnimation ? (-1) * mp.menuController.screenSizeinCanvasUnits.y : mp.menuController.screenSizeinCanvasUnits.y;
                }
                if (mp.openMoveType == EasyMenuManagerMoveTransitionType.Top || mp.openMoveType == EasyMenuManagerMoveTransitionType.TopLeft || mp.openMoveType == EasyMenuManagerMoveTransitionType.TopRight)
                {
                    moveVec.y += !mp.uniformOpenMoveAnimation ? mp.menuController.screenSizeinCanvasUnits.y : mp.menuController.screenSizeinCanvasUnits.y;
                }

                if (mp.openMoveType == EasyMenuManagerMoveTransitionType.Custom1)
                    moveVec = mp.menuController.customAnchorPosition;
                if (mp.openMoveType == EasyMenuManagerMoveTransitionType.Custom2)
                    moveVec = mp.menuController.customAnchorPosition2;

                mp.menuController.selfRect.anchoredPosition = moveVec;
                mp.menuController.openMoveTween = mp.menuController.selfRect.DOAnchorPos(mp.menuController.anchorCenter, mp.openMoveDuration, false).SetEase(mp.openMoveEaseType).SetAutoKill(true).OnKill(() => mp.menuController.openMoveTween = null).SetDelay(mp.openMoveDelay);
            }
        }

        if (mp.enableOpenRotate)
        {
            if (mp.menuController.selfRect != null)
            {
                if (mp.menuController.closeRotateTween != null)
                    mp.menuController.closeRotateTween.Complete();
                rotateVec.Set(0, 0, mp.openRotateVariance);
                mp.menuController.selfRect.localEulerAngles = (rotateVec + mp.menuController.defaultRotation);
                mp.menuController.openRotateTween = mp.menuController.selfRect.DOLocalRotate(-(rotateVec + mp.menuController.defaultRotation), mp.openRotateDuration, RotateMode.LocalAxisAdd).SetEase(mp.openRotateEaseType).SetAutoKill(true).OnKill(() => mp.menuController.openRotateTween = null).SetDelay(mp.openRotateDelay);
            }
        }

        if (mp.enableOpenScale)
        {
            if (mp.menuController.selfRect != null)
            {
                if (mp.menuController.closeScaleTween != null)
                    mp.menuController.closeScaleTween.Kill();
                scaleVec.Set(mp.openScaleVariance.x, mp.openScaleVariance.y, 0);
                //				mp.menuController.selfRect.localScale = (scaleVec + mp.menuController.defaultScale);
                mp.menuController.selfRect.localScale = scaleVec;
                mp.menuController.openScaleTween = mp.menuController.selfRect.DOScale(mp.menuController.defaultScale, mp.openScaleDuration).SetEase(mp.openScaleEaseType).SetAutoKill(true).OnKill(() => mp.menuController.openScaleTween = null).SetDelay(mp.openScaleDelay);
            }
        }

        if (mp.enableOpenAlpha)
        {
            if (mp.menuController._canvasGroup != null)
            {
                if (mp.menuController.closeAlphaTween != null)
                    mp.menuController.closeAlphaTween.Kill();
                mp.menuController._canvasGroup.alpha = mp.openAlphaVariance;
                mp.menuController.openAlphaTween = DOTween.To(() => mp.menuController._canvasGroup.alpha, x => mp.menuController._canvasGroup.alpha = x, mp.menuController.defaultAlpha, mp.openAlphaDuration).SetEase(mp.openAlphaEaseType).SetAutoKill(true).OnKill(() => mp.menuController.openAlphaTween = null).SetDelay(mp.openAlphaDelay);
            }
        }
    }


    public void RunOnCloseAnimations(string menuName)
    {
        RunOnCloseAnimationActions(GetMenuPropertiesByName(menuName));
    }
    public void RunOnCloseAnimations(MenuController menuController)
    {
        RunOnCloseAnimationActions(GetMenuPropertiesByController(menuController));
    }
    void RunOnCloseAnimationActions(MenuProperties mp)
    {
        if (!mp.enableCloseMove && mp.menuController.openMoveTween != null) { mp.menuController.openMoveTween.Complete(); }
        if (!mp.enableCloseRotate && mp.menuController.openRotateTween != null) { mp.menuController.openRotateTween.Complete(); }
        if (!mp.enableCloseScale && mp.menuController.openScaleTween != null) { mp.menuController.openScaleTween.Complete(); }
        if (!mp.enableCloseAlpha && mp.menuController.openAlphaTween != null) { mp.menuController.openAlphaTween.Complete(); }

        if (mp.enableCloseMove)
        {
            //			if (mp.menuController.closeMoveTween != null) {mp.menuController.closeMoveTween.Complete();}
            if (mp.menuController.openMoveTween != null) { mp.menuController.openMoveTween.Kill(); }
            if (mp.menuController.selfRect != null)
            {
                moveVec = mp.menuController.anchorCenter;

                if (mp.closeMoveType == EasyMenuManagerMoveTransitionType.Left || mp.closeMoveType == EasyMenuManagerMoveTransitionType.TopLeft || mp.closeMoveType == EasyMenuManagerMoveTransitionType.BottomLeft)
                {
                    moveVec.x += !mp.uniformCloseMoveAnimation ? (-1) * mp.menuController.screenSizeinCanvasUnits.x : mp.menuController.screenSizeinCanvasUnits.x;
                }
                if (mp.closeMoveType == EasyMenuManagerMoveTransitionType.Right || mp.closeMoveType == EasyMenuManagerMoveTransitionType.TopRight || mp.closeMoveType == EasyMenuManagerMoveTransitionType.BottomRight)
                {
                    moveVec.x += !mp.uniformCloseMoveAnimation ? mp.menuController.screenSizeinCanvasUnits.x : mp.menuController.screenSizeinCanvasUnits.x;
                }
                if (mp.closeMoveType == EasyMenuManagerMoveTransitionType.Bottom || mp.closeMoveType == EasyMenuManagerMoveTransitionType.BottomLeft || mp.closeMoveType == EasyMenuManagerMoveTransitionType.BottomRight)
                {
                    moveVec.y += !mp.uniformCloseMoveAnimation ? (-1) * mp.menuController.screenSizeinCanvasUnits.y : mp.menuController.screenSizeinCanvasUnits.y;
                }
                if (mp.closeMoveType == EasyMenuManagerMoveTransitionType.Top || mp.closeMoveType == EasyMenuManagerMoveTransitionType.TopLeft || mp.closeMoveType == EasyMenuManagerMoveTransitionType.TopRight)
                {
                    moveVec.y += !mp.uniformCloseMoveAnimation ? mp.menuController.screenSizeinCanvasUnits.y : mp.menuController.screenSizeinCanvasUnits.y;
                }

                if (mp.closeMoveType == EasyMenuManagerMoveTransitionType.Custom1)
                    moveVec = mp.menuController.customAnchorPosition;
                if (mp.closeMoveType == EasyMenuManagerMoveTransitionType.Custom2)
                    moveVec = mp.menuController.customAnchorPosition2;

                //				mp.menuController.selfRect.anchoredPosition = moveVec;
                mp.menuController.closeMoveTween = mp.menuController.selfRect.DOAnchorPos(moveVec, mp.closeMoveDuration, false).SetEase(mp.closeMoveEaseType).SetAutoKill(true).OnKill(() => mp.menuController.closeMoveTween = null).SetDelay(mp.closeMoveDelay);
            }
        }

        if (mp.enableCloseRotate)
        {
            if (mp.menuController.selfRect != null)
            {
                if (mp.menuController.openRotateTween != null)
                    mp.menuController.openRotateTween.Complete();
                rotateVec.Set(0, 0, mp.closeRotateVariance);
                //				mp.menuController.selfRect.Rotate (rotateVec);
                mp.menuController.closeRotateTween = mp.menuController.selfRect.DOLocalRotate(rotateVec + mp.menuController.defaultRotation, mp.closeRotateDuration, RotateMode.LocalAxisAdd).SetEase(mp.closeRotateEaseType).SetAutoKill(true).OnKill(() => mp.menuController.closeRotateTween = null).SetDelay(mp.closeRotateDelay);
            }
        }

        if (mp.enableCloseScale)
        {
            if (mp.menuController.selfRect != null)
            {
                if (mp.menuController.openScaleTween != null)
                    mp.menuController.openScaleTween.Kill();
                scaleVec.Set(mp.closeScaleVariance.x, mp.closeScaleVariance.y, 0);
                mp.menuController.closeScaleTween = mp.menuController.selfRect.DOScale(scaleVec, mp.closeScaleDuration).SetEase(mp.closeScaleEaseType).SetAutoKill(true).OnKill(() => mp.menuController.closeScaleTween = null).SetDelay(mp.closeScaleDelay);
            }
        }

        if (mp.enableCloseAlpha)
        {
            if (mp.menuController._canvasGroup != null)
            {
                if (mp.menuController.openAlphaTween != null)
                    mp.menuController.openAlphaTween.Kill();
                mp.menuController.closeAlphaTween = DOTween.To(() => mp.menuController._canvasGroup.alpha, x => mp.menuController._canvasGroup.alpha = x, mp.closeAlphaVariance, mp.closeAlphaDuration).SetEase(mp.closeAlphaEaseType).SetAutoKill(true).OnKill(() => mp.menuController.closeAlphaTween = null).SetDelay(mp.closeAlphaDelay);
            }
        }
    }


    public void InstantlyFinishAnyRunningOpenAnimations(MenuProperties mp)
    {
        if (mp == null) { return; }
        if (mp.menuController.openMoveTween != null && mp.menuController.openMoveTween.IsPlaying()) { mp.menuController.openMoveTween.Complete(); }
        if (mp.menuController.openRotateTween != null && mp.menuController.openRotateTween.IsPlaying()) { mp.menuController.openRotateTween.Complete(); }
        if (mp.menuController.openScaleTween != null && mp.menuController.openScaleTween.IsPlaying()) { mp.menuController.openScaleTween.Complete(); }
        if (mp.menuController.openAlphaTween != null && mp.menuController.openAlphaTween.IsPlaying()) { mp.menuController.openAlphaTween.Complete(); }
    }
    public void InstantlyFinishAnyRunningCloseAnimations(MenuProperties mp)
    {
        if (mp == null) { return; }
        if (mp.menuController.closeMoveTween != null && mp.menuController.closeMoveTween.IsPlaying()) { mp.menuController.closeMoveTween.Complete(); }
        if (mp.menuController.closeRotateTween != null && mp.menuController.closeRotateTween.IsPlaying()) { mp.menuController.closeRotateTween.Complete(); }
        if (mp.menuController.closeScaleTween != null && mp.menuController.closeScaleTween.IsPlaying()) { mp.menuController.closeScaleTween.Complete(); }
        if (mp.menuController.closeAlphaTween != null && mp.menuController.closeAlphaTween.IsPlaying()) { mp.menuController.closeAlphaTween.Complete(); }
    }


    public void MirrorOpenAnimationSettingsToCloseAnimationSettings(int menuIndex)
    {
        MenuProperties mp = menuList[menuIndex];
        float maxDur = Mathf.Max(mp.enableOpenMove ? mp.openMoveDuration + mp.openMoveDelay : 0, mp.enableOpenRotate ? mp.openRotateDuration + mp.openRotateDelay : 0, mp.enableOpenScale ? mp.openScaleDuration + mp.openScaleDelay : 0, mp.enableOpenAlpha ? mp.openAlphaDuration + mp.openAlphaDelay : 0);

        mp.enableCloseMove = mp.enableOpenMove;
        mp.closeMoveType = mp.openMoveType;
        mp.closeMoveEaseType = GetOppositeEase(mp.openMoveEaseType);
        mp.enableCloseRotate = mp.enableOpenRotate;
        mp.closeRotateVariance = mp.openRotateVariance;
        mp.closeRotateEaseType = GetOppositeEase(mp.openRotateEaseType);
        mp.enableCloseScale = mp.enableOpenScale;
        mp.closeScaleVariance = mp.openScaleVariance;
        mp.closeScaleEaseType = GetOppositeEase(mp.openScaleEaseType);
        mp.enableCloseAlpha = mp.enableOpenAlpha;
        mp.closeAlphaVariance = mp.openAlphaVariance;
        mp.closeAlphaEaseType = GetOppositeEase(mp.openAlphaEaseType);

        mp.closeMoveDuration = mp.openMoveDuration;
        mp.closeRotateDuration = mp.openRotateDuration;
        mp.closeScaleDuration = mp.openScaleDuration;
        mp.closeAlphaDuration = mp.openAlphaDuration;

        if (mp.enableOpenMove)
            mp.closeMoveDelay = maxDur - (mp.closeMoveDuration + mp.openMoveDelay);
        if (mp.enableOpenRotate)
            mp.closeRotateDelay = maxDur - (mp.closeRotateDuration + mp.openRotateDelay);
        if (mp.enableOpenScale)
            mp.closeScaleDelay = maxDur - (mp.closeScaleDuration + mp.openScaleDelay);
        if (mp.enableOpenAlpha)
            mp.closeAlphaDelay = maxDur - (mp.closeAlphaDuration + mp.openAlphaDelay);

        mp.uniformCloseMoveAnimation = mp.uniformOpenMoveAnimation;

    }

    public void MirrorCloseAnimationSettingsToOpenAnimationSettings(int menuIndex)
    {
        MenuProperties mp = menuList[menuIndex];
        float maxDur = Mathf.Max(mp.enableCloseMove ? mp.closeMoveDuration + mp.closeMoveDelay : 0, mp.enableCloseRotate ? mp.closeRotateDuration + mp.closeRotateDelay : 0, mp.enableCloseScale ? mp.closeScaleDuration + mp.closeScaleDelay : 0, mp.enableCloseAlpha ? mp.closeAlphaDuration + mp.closeAlphaDelay : 0);

        mp.enableOpenMove = mp.enableCloseMove;
        mp.openMoveType = mp.closeMoveType;
        mp.openMoveEaseType = GetOppositeEase(mp.closeMoveEaseType);
        mp.enableOpenRotate = mp.enableCloseRotate;
        mp.openRotateVariance = mp.closeRotateVariance;
        mp.openRotateEaseType = GetOppositeEase(mp.closeRotateEaseType);
        mp.enableOpenScale = mp.enableCloseScale;
        mp.openScaleVariance = mp.closeScaleVariance;
        mp.openScaleEaseType = GetOppositeEase(mp.closeScaleEaseType);
        mp.enableOpenAlpha = mp.enableCloseAlpha;
        mp.openAlphaVariance = mp.closeAlphaVariance;
        mp.openAlphaEaseType = GetOppositeEase(mp.closeAlphaEaseType);

        mp.openMoveDuration = mp.closeMoveDuration;
        mp.openRotateDuration = mp.closeRotateDuration;
        mp.openScaleDuration = mp.closeScaleDuration;
        mp.openAlphaDuration = mp.closeAlphaDuration;

        if (mp.enableCloseMove)
            mp.openMoveDelay = maxDur - (mp.openMoveDuration + mp.closeMoveDelay);
        if (mp.enableCloseRotate)
            mp.openRotateDelay = maxDur - (mp.openRotateDuration + mp.closeRotateDelay);
        if (mp.enableCloseScale)
            mp.openScaleDelay = maxDur - (mp.openScaleDuration + mp.closeScaleDelay);
        if (mp.enableCloseAlpha)
            mp.openAlphaDelay = maxDur - (mp.openAlphaDuration + mp.closeAlphaDelay);

        mp.uniformOpenMoveAnimation = mp.uniformCloseMoveAnimation;

    }



    public Ease GetOppositeEase(Ease easeToMirror)
    {
        if (easeToMirror == Ease.InSine) { return Ease.OutSine; }
        else if (easeToMirror == Ease.OutSine) { return Ease.InSine; }
        else if (easeToMirror == Ease.InQuad) { return Ease.OutQuad; }
        else if (easeToMirror == Ease.OutQuad) { return Ease.InQuad; }
        else if (easeToMirror == Ease.InCubic) { return Ease.OutCubic; }
        else if (easeToMirror == Ease.OutCubic) { return Ease.InCubic; }
        else if (easeToMirror == Ease.InQuart) { return Ease.OutQuart; }
        else if (easeToMirror == Ease.OutQuart) { return Ease.InQuart; }
        else if (easeToMirror == Ease.InQuint) { return Ease.OutQuint; }
        else if (easeToMirror == Ease.OutQuint) { return Ease.InQuint; }
        else if (easeToMirror == Ease.InExpo) { return Ease.OutExpo; }
        else if (easeToMirror == Ease.OutExpo) { return Ease.InExpo; }
        else if (easeToMirror == Ease.InCirc) { return Ease.OutCirc; }
        else if (easeToMirror == Ease.OutCirc) { return Ease.InCirc; }
        else if (easeToMirror == Ease.InElastic) { return Ease.OutElastic; }
        else if (easeToMirror == Ease.OutElastic) { return Ease.InElastic; }
        else if (easeToMirror == Ease.InBack) { return Ease.OutBack; }
        else if (easeToMirror == Ease.OutBack) { return Ease.InBack; }
        else if (easeToMirror == Ease.InBounce) { return Ease.OutBounce; }
        else if (easeToMirror == Ease.OutBounce) { return Ease.InBounce; }
        else { return easeToMirror; }
    }

    public int GetMenuIndexOfMenuController(MenuController _menuController)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController == _menuController)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetMenuControllerParentMatch(EasyMenuManager emmRef, int menuIndex)
    {
        if (menuList[menuIndex].menuController.parentMM != emmRef) { menuList[menuIndex].menuController.SetParentMenuManager(emmRef); }
    }

    public bool IsMenuControllerinMenuList(MenuController mc)
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            if (menuList[i].menuController == mc) { return true; }
        }
        return false;
    }

}

[System.Serializable]
public class MenuProperties
{
    public string menuName; // name of menu. Used to open menu by name.
    public bool firstNamed;
    public bool isOpen;
    public bool closeWhenOpenedWhileOpen; // If TRUE, menu will close if something tries to open it while it is currently open.
    public bool allowAllByDefault; // If this is set to TRUE, every menu NOT in the incompatibleMenus list is treated as compatible (opening this menu will not close any other)
    public bool showContents;
    public string onOpenSoundName;
    public string onCloseSoundName;
    public List<MenuController> incompatibleMenus;
    public List<MenuController> compatibleMenus; // By default, opening this menu checks this list and lets any on it to stay open as well.
    public List<MenuController> preventedMenus; // menus on this list cannot be opened if this menu is open. Open function will stop before any actions are taken.
    public List<MenuController> preventedMenusClosing; // menus on this list cannot be opened if this menu is open. Open function will stop before any actions are taken.
    public MenuController menuController; // Menu controller component corresponding to this menu. Must be set.
    public UnityEvent whenOpenedDoThis; // these events are ran when the menu is opened. Good for calling sound effects and such.
    public UnityEvent whenClosedDoThis; // these events are ran when the menu is closed. Good for calling sound effects and such.
    public bool runOpenEvents;
    public bool runClosedEvents;

    // ANIMATION PROPERTIES
    public bool enableOpenMove;
    public EasyMenuManagerMoveTransitionType openMoveType;
    public Ease openMoveEaseType;
    public bool enableOpenRotate;
    public float openRotateVariance;
    public Ease openRotateEaseType;
    public bool enableOpenScale;
    public Vector2 openScaleVariance;
    public Ease openScaleEaseType;
    public bool enableOpenAlpha;
    public float openAlphaVariance;
    public Ease openAlphaEaseType;

    public float openMoveDuration = 0.5f;
    public float openRotateDuration = 0.5f;
    public float openScaleDuration = 0.5f;
    public float openAlphaDuration = 0.5f;

    public float openMoveDelay = 0f;
    public float openRotateDelay = 0f;
    public float openScaleDelay = 0f;
    public float openAlphaDelay = 0f;


    public bool enableCloseMove;
    public EasyMenuManagerMoveTransitionType closeMoveType;
    public Ease closeMoveEaseType;
    public bool enableCloseRotate;
    public float closeRotateVariance;
    public Ease closeRotateEaseType;
    public bool enableCloseScale;
    public Vector2 closeScaleVariance;
    public Ease closeScaleEaseType;
    public bool enableCloseAlpha;
    public float closeAlphaVariance;
    public Ease closeAlphaEaseType;

    public float closeMoveDuration = 0.5f;
    public float closeRotateDuration = 0.5f;
    public float closeScaleDuration = 0.5f;
    public float closeAlphaDuration = 0.5f;

    public float closeMoveDelay = 0f;
    public float closeRotateDelay = 0f;
    public float closeScaleDelay = 0f;
    public float closeAlphaDelay = 0f;

    public bool uniformOpenMoveAnimation; // If true, moving menu on/off screen for open/close animations will move based on root canvas size (which should be screen size) and not menu size
    public bool uniformCloseMoveAnimation;

    public bool disableWhenClosed; // If true, will disable the game object when the menu is closed (CHANGED. MOVES FAR OFF SCREEN. DISABLES CANVAS RENDERER GAME OBJECTS ONLY!)
    public bool disabledAfterLastClose;

    //	bool muteNextOpen;
    //	bool muteNextClose;

    public bool preventClosingWhileOpening;
    public bool preventOpeningWhileClosing;

    public bool queueClosingWhenPreventing;
    public bool queueOpeningWhenPreventing;

    public EasyMenuManagerStartState startState;



    public MenuProperties()
    {
        incompatibleMenus = new List<MenuController>();
        compatibleMenus = new List<MenuController>();
        preventedMenus = new List<MenuController>();
        preventedMenusClosing = new List<MenuController>();
    }
}

public enum EasyMenuManagerMoveTransitionType
{
    Left, Right, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight, Custom1, Custom2,
}
public enum EasyMenuManagerStartState
{
    CloseInstantNoSound, CloseInstant, CloseNoSound, Close, OpenInstantNoSound, OpenInstant, OpenNoSound, Open
}

