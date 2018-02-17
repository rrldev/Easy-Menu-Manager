using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using DG.Tweening;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasGroup))]
public class MenuController : MonoBehaviour
{

    //	private Animator _animator;
    [HideInInspector]
    public CanvasGroup _canvasGroup;
    CanvasRenderer[] renderers;

    public EasyMenuManager parentMM;
    public MenuController parentMC;
    public bool attached;
    public bool childController; // if true, this controller is attached to another controller.

    public bool centerOnAwake;
    public bool disableInteractable;
    public bool disableRaycasts;

    public bool dontEnableIfTweening;

    public RectTransform selfRect;
    public Vector2 offsetMaxCenter;
    public Vector2 offsetMinCenter;
    public Transform selfTransform;
    public Vector3 localPositionCenter;

    public Vector2 anchorCenter;
    public Vector3 defaultRotation;
    public Vector3 defaultScale;
    public float defaultAlpha;
    public Vector2 defaultPivot;

    public Vector2 anchorMax;
    public Vector2 anchorMin;

    public Tween openMoveTween;
    public Tween openRotateTween;
    public Tween openScaleTween;
    public Tween openAlphaTween;

    public Tween closeMoveTween;
    public Tween closeRotateTween;
    public Tween closeScaleTween;
    public Tween closeAlphaTween;

    public Vector2 customAnchorPosition;
    public Vector2 customAnchorPosition2;

    public UnityEvent openEvents; // these aren't actually used at runtime. just temp events because the unity editor wouldnt update changes of the the actual events.
    public UnityEvent closeEvents;

    Canvas rootCanvas;
    RectTransform rootRect;
    public Vector2 screenSizeinCanvasUnits; // how many units of the root canvas covers the entire game screen (used to animate menus completely off screen regardless of canvas mode)




    public bool IsOpen;
    //	{
    //		get { return _animator.GetBool("IsOpen"); }
    //		set { _animator.SetBool("IsOpen", value); }
    //	}

    public void OnEnable()
    {

        //		if (GetComponent<Animator>() != null) {	_animator = GetComponent<Animator>(); }
        if (GetComponent<CanvasGroup>() != null) { _canvasGroup = GetComponent<CanvasGroup>(); }
        if (GetComponent<RectTransform>() != null) { selfRect = GetComponent<RectTransform>(); }
        if (GetComponent<Transform>() != null) { selfTransform = GetComponent<Transform>(); }

        renderers = GetComponentsInChildren<CanvasRenderer>();
    }

    public void SetParentMenuManager(EasyMenuManager emmParent)
    {
        parentMM = emmParent;
        if (parentMM != null) { attached = true; }
        else { attached = false; }
        childController = false;
    }
    public void SetParentMenuController(MenuController controllerParent)
    {
        parentMC = controllerParent;
        if (parentMC != null) { attached = true; }
        else { attached = false; }
        childController = true;
    }
    public void SetParentsNull()
    {
        parentMC = null;
        parentMM = null;
        attached = false;
    }

    public void ToggleRenderers(bool show)
    {
        //Debug.Log("MENU: " + this.gameObject.name + " | SHOW: " + show);
        if (show)
        {
            foreach (CanvasRenderer _ren in renderers)
            {
                if (_ren.gameObject.GetComponent<EasyMenuManagerIgnoreHide>() == null)
                    _ren.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (CanvasRenderer _ren in renderers)
            {
                if (_ren.gameObject.GetComponent<EasyMenuManagerIgnoreHide>() == null)
                    _ren.gameObject.SetActive(false);
            }
        }
    }

    public void MenuControllerStart()
    {

        if (Application.isPlaying == false) { return; }

        if (selfRect) // THIS WILL FIND THE ROOT CANVAS THAT THIS MENU RENDERS TO AND GET THE BOUNDARIES OF THE SCREEN BASED ON ITS SETTINGS
        {
            RectTransform parRect = selfRect;
            while (parRect != null)
            {
                if (parRect.gameObject.GetComponent<Canvas>() != null) { rootCanvas = parRect.gameObject.GetComponent<Canvas>(); }
                parRect = (RectTransform)parRect.parent;
            }
            if (rootCanvas == null)
            {
                Debug.LogError("Menu Controller attached to game object [" + this.gameObject.name + "] is not a child of a canvas!");
            }
            else
            {
                rootRect = rootCanvas.GetComponent<RectTransform>();
                if (rootCanvas.renderMode == RenderMode.WorldSpace)
                {
                    screenSizeinCanvasUnits.Set(rootRect.sizeDelta.x, rootRect.sizeDelta.y);
                }
                else if (rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    screenSizeinCanvasUnits.Set(rootRect.sizeDelta.x, rootRect.sizeDelta.y);
                }
                else if (rootCanvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    screenSizeinCanvasUnits.Set(rootRect.sizeDelta.x, rootRect.sizeDelta.y);
                }
            }
        }

        //		if (centerOnAwake)
        //		{
        //			MoveToOnScreenPosition();
        //		}

    }

    public void Update()
    {

        if (Application.isPlaying == false || _canvasGroup == null) { return; }

        if (IsOpen == false)
        {
            if (disableInteractable && _canvasGroup.interactable == true) { _canvasGroup.interactable = false; }
            if (disableRaycasts && _canvasGroup.blocksRaycasts == true) { _canvasGroup.blocksRaycasts = false; }
        }
        else if (IsOpen == true)
        {
            if (disableInteractable && _canvasGroup.interactable == false && (dontEnableIfTweening == false || (dontEnableIfTweening == true && AreAnyOpenOrCloseTweensRunning() == false))) { _canvasGroup.interactable = true; }
            if (disableRaycasts && _canvasGroup.blocksRaycasts == false && (dontEnableIfTweening == false || (dontEnableIfTweening == true && AreAnyOpenOrCloseTweensRunning() == false))) { _canvasGroup.blocksRaycasts = true; }
        }

    }


    public bool AreAnyOpenOrCloseTweensRunning()
    {
        bool runningTweenCheck = false;

        if (openMoveTween != null && openMoveTween.IsPlaying()) { runningTweenCheck = true; }
        if (openRotateTween != null && openRotateTween.IsPlaying()) { runningTweenCheck = true; }
        if (openScaleTween != null && openScaleTween.IsPlaying()) { runningTweenCheck = true; }
        if (openAlphaTween != null && openAlphaTween.IsPlaying()) { runningTweenCheck = true; }

        if (closeMoveTween != null && closeMoveTween.IsPlaying()) { runningTweenCheck = true; }
        if (closeRotateTween != null && closeRotateTween.IsPlaying()) { runningTweenCheck = true; }
        if (closeScaleTween != null && closeScaleTween.IsPlaying()) { runningTweenCheck = true; }
        if (closeAlphaTween != null && closeAlphaTween.IsPlaying()) { runningTweenCheck = true; }

        return runningTweenCheck;
    }
    public bool AreAnyOpenOrCloseTweensRunning(bool trueForOpenFalseForClose)
    {
        bool runningTweenCheck = false;

        if (trueForOpenFalseForClose)
        {
            if (openMoveTween != null && openMoveTween.IsPlaying()) { runningTweenCheck = true; }
            if (openRotateTween != null && openRotateTween.IsPlaying()) { runningTweenCheck = true; }
            if (openScaleTween != null && openScaleTween.IsPlaying()) { runningTweenCheck = true; }
            if (openAlphaTween != null && openAlphaTween.IsPlaying()) { runningTweenCheck = true; }
        }
        else
        {
            if (closeMoveTween != null && closeMoveTween.IsPlaying()) { runningTweenCheck = true; }
            if (closeRotateTween != null && closeRotateTween.IsPlaying()) { runningTweenCheck = true; }
            if (closeScaleTween != null && closeScaleTween.IsPlaying()) { runningTweenCheck = true; }
            if (closeAlphaTween != null && closeAlphaTween.IsPlaying()) { runningTweenCheck = true; }
        }

        return runningTweenCheck;
    }



    public void SetOnScreenPosition()
    {
        if (selfRect != null)
        {
            offsetMaxCenter = selfRect.offsetMax;
            offsetMinCenter = selfRect.offsetMin;
            anchorCenter = selfRect.anchoredPosition;
            defaultRotation = selfRect.localEulerAngles;
            defaultScale = selfRect.localScale;
            defaultAlpha = _canvasGroup.alpha;
            defaultPivot = selfRect.pivot;
            anchorMax = selfRect.anchorMax;
            anchorMin = selfRect.anchorMin;
        }
        else if (selfTransform != null)
        {
            localPositionCenter = selfTransform.localPosition;
        }
    }

    public void MoveToOnScreenPosition()
    {
        if (selfRect != null)
        {
            selfRect.offsetMax = offsetMaxCenter;
            selfRect.offsetMin = offsetMinCenter;
            selfRect.anchoredPosition = anchorCenter;
            selfRect.localEulerAngles = defaultRotation;
            selfRect.localScale = defaultScale;
            _canvasGroup.alpha = defaultAlpha;
            selfRect.pivot = defaultPivot;
            selfRect.anchorMax = anchorMax;
            selfRect.anchorMin = anchorMin;
        }
        else if (selfTransform != null)
        {
            selfTransform.localPosition = localPositionCenter;
        }
    }

    public void SetCustomOffScreenPosition()
    {
        if (selfRect != null)
        {
            customAnchorPosition = selfRect.anchoredPosition;
        }

    }
    public void SetCustom2OffScreenPosition()
    {
        if (selfRect != null)
        {
            customAnchorPosition2 = selfRect.anchoredPosition;
        }

    }




}
