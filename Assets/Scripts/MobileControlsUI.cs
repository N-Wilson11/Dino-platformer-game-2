using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileControlsUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement targetPlayer;
    [SerializeField] private Vector2 buttonSize = new Vector2(120f, 120f);
    private bool controlsCreated;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (!ShouldEnableMobileControls())
        {
            return;
        }

        if (FindFirstObjectByType<MobileControlsUI>() != null)
        {
            return;
        }

        GameObject controller = new GameObject("MobileControlsUI");
        controller.AddComponent<MobileControlsUI>();
        DontDestroyOnLoad(controller);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        TryInitializeControls();
    }

    private void Update()
    {
        if (!controlsCreated)
        {
            TryInitializeControls();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        targetPlayer = null;
        controlsCreated = false;
        TryInitializeControls();
    }

    private void TryInitializeControls()
    {
        if (!ShouldEnableMobileControls())
        {
            gameObject.SetActive(false);
            return;
        }

        if (targetPlayer == null)
        {
            targetPlayer = FindAnyObjectByType<PlayerMovement>();
        }

        if (targetPlayer == null)
        {
            return;
        }

        EnsureEventSystem();
        Canvas canvas = EnsureCanvas();

        if (canvas.transform.Find("MobileControlsRoot") != null)
        {
            controlsCreated = true;
            return;
        }

        CreateControls(canvas.transform);
        controlsCreated = true;
    }

    private static bool ShouldEnableMobileControls()
    {
        return Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld;
    }

    private static void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static Canvas EnsureCanvas()
    {
        Canvas existingCanvas = FindFirstObjectByType<Canvas>();
        if (existingCanvas != null)
        {
            return existingCanvas;
        }

        GameObject canvasObject = new GameObject("MobileCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        return canvas;
    }

    private void CreateControls(Transform canvasTransform)
    {
        GameObject root = new GameObject("MobileControlsRoot", typeof(RectTransform));
        root.transform.SetParent(canvasTransform, false);

        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.offsetMin = Vector2.zero;
        rootRect.offsetMax = Vector2.zero;

        Button leftButton = CreateButton(rootRect, "LeftButton", "<", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(140f, 140f));
        Button rightButton = CreateButton(rootRect, "RightButton", ">", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(300f, 140f));
        Button jumpButton = CreateButton(rootRect, "UpButton", "^", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-160f, 140f));

        AddHoldEvents(leftButton, _ => targetPlayer.SetMobileLeft(true), _ => targetPlayer.SetMobileLeft(false));
        AddHoldEvents(rightButton, _ => targetPlayer.SetMobileRight(true), _ => targetPlayer.SetMobileRight(false));
        AddPressEvent(jumpButton, _ => targetPlayer.MobileJump());
    }

    private Button CreateButton(
        Transform parent,
        string objectName,
        string label,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 anchoredPosition)
    {
        GameObject buttonObject = new GameObject(objectName, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = buttonSize;
        rectTransform.anchoredPosition = anchoredPosition;

        Image background = buttonObject.GetComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0.35f);

        CreateButtonLabel(buttonObject.transform, label);

        return buttonObject.GetComponent<Button>();
    }

    private static void CreateButtonLabel(Transform buttonTransform, string label)
    {
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        labelObject.transform.SetParent(buttonTransform, false);

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text text = labelObject.GetComponent<Text>();
        text.text = label;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 42;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    private static void AddPressEvent(Button button, UnityAction<BaseEventData> onDown)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        AddEventEntry(trigger, EventTriggerType.PointerDown, onDown);
    }

    private static void AddHoldEvents(Button button, UnityAction<BaseEventData> onDown, UnityAction<BaseEventData> onUp)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        AddEventEntry(trigger, EventTriggerType.PointerDown, onDown);
        AddEventEntry(trigger, EventTriggerType.PointerUp, onUp);
        AddEventEntry(trigger, EventTriggerType.PointerExit, onUp);
    }

    private static void AddEventEntry(EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}
