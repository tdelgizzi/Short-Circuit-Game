using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastManager : MonoBehaviour
{

    Subscription<ToastEvent> toast_event_sub;

    Vector3 hidden_pos;
    Vector3 visible_pos;

    [SerializeField]
    private RectTransform toast_panel;
    [SerializeField]
    private TextMeshProUGUI toast_text;

    // These inspector-accessible variables control how the toast UI panel moves between the hidden and visible positions.
    [SerializeField]
    private AnimationCurve ease;
    [SerializeField]
    private AnimationCurve ease_out;

    // Duration controls.
    [SerializeField]
    private float ease_duration = 0.5f;
    [SerializeField]
    private float show_duration = 2.0f;

    Queue<ToastEvent> messages = new Queue<ToastEvent>();

    private bool toasting = false;

    // Start is called before the first frame update
    void Awake()
    {
        toast_event_sub = EventBus.Subscribe<ToastEvent>(_toastRecived);
        toasting = false;

        Vector2 currentPos = toast_panel.transform.localPosition;
        //TODO FIX THESE
        hidden_pos = currentPos + (new Vector2(0, -300));
        visible_pos = currentPos;
        toast_panel.anchoredPosition = hidden_pos;


        toast_text.text = "Testing";

    }

    // Update is called once per frame
    void Update()
    {
        if (!toasting && messages.Count > 0)
        {
            ToastEvent e = messages.Dequeue();
            show_duration = e.duration;
            toasting = true;
            toast_text.text = e.current_msg;
            AudioManager.PlayClipNow(e.aclip);
            if (e.buttonPress == false)
            { 
                StartCoroutine(DoToast(ease_duration, show_duration));
            }
            else
            {
                StartCoroutine(DoToastKeyPress(ease_duration, e.buttonPressKey));
            }
        }
    }

    IEnumerator DoToast(float duration_ease_sec, float duration_show_sec)
    {
        // Ease In the UI panel
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_ease_sec;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = ease.Evaluate(progress);
            toast_panel.anchoredPosition = Vector3.LerpUnclamped(hidden_pos, visible_pos, eased_progress);

            yield return null;
        }


        yield return new WaitForSeconds(duration_show_sec);


        // Ease Out the UI panel
        initial_time = Time.time;
        progress = 0.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = ease_out.Evaluate(progress);
            toast_panel.anchoredPosition = Vector3.LerpUnclamped(visible_pos, hidden_pos, 1.0f - eased_progress);

            yield return null;
        }

        // When we're done toasting, we tell the "Update" function that we're ready for more requests.
        toasting = false;

    }

    IEnumerator DoToastKeyPress(float duration_ease_sec, KeyCode keyPressKey)
    {
        // Ease In the UI panel
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_ease_sec;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = ease.Evaluate(progress);
            toast_panel.anchoredPosition = Vector3.LerpUnclamped(hidden_pos, visible_pos, eased_progress);

            yield return null;
        }
        bool loop = true;

        while (loop){
            if (Input.GetKeyDown(keyPressKey))
            {
                Debug.Log("Toast key was pressed this frame");
                loop = false;
            }
            else
            {
                yield return null;
            }
        }


        // Ease Out the UI panel
        initial_time = Time.time;
        progress = 0.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = ease_out.Evaluate(progress);
            toast_panel.anchoredPosition = Vector3.LerpUnclamped(visible_pos, hidden_pos, 1.0f - eased_progress);

            yield return null;
        }

        // When we're done toasting, we tell the "Update" function that we're ready for more requests.
        toasting = false;

    }

    void _toastRecived(ToastEvent e)
    {
        messages.Enqueue(e);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(toast_event_sub);
    }
}

public class ToastEvent
{

    public string current_msg;
    public float duration;
    public bool buttonPress;
    public KeyCode buttonPressKey;
    public string aclip;

    public ToastEvent(string msg, float _dur) { current_msg = msg; duration = _dur; buttonPress = false; }
    public ToastEvent(string msg, float _dur, string _aclip) { current_msg = msg; duration = _dur; buttonPress = false; aclip = _aclip; }
    public ToastEvent(string msg, KeyCode keyPressIn, string _aclip) { current_msg = msg; buttonPress = true; buttonPressKey = keyPressIn; aclip = _aclip; }

    public ToastEvent(string msg) { current_msg = msg; duration = 3f; }

}
