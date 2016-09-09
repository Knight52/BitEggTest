using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlPadController : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image foreground;
    private bool isPressing;
    private Vector2 center;
    private int lastTouchId;
    private float radius;
    // Use this for initialization
    void Start () {
        center = background.transform.position;
        radius = background.rectTransform.rect.height / 2;
        lastTouchId = -1;
	}
	
	// Update is called once per frame
    public Vector2 Direction { get { return (foreground.transform.position - background.transform.position) / (background.rectTransform.rect.height / 2); } }
    private bool IsInputInRange(out Vector2 position, out int touchId)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        Vector2 pos = Vector2.zero;
        touchId = lastTouchId;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if ((Input.GetTouch(i).position - center).magnitude < radius || Input.GetTouch(i).fingerId == lastTouchId)
            {
                pos = Input.GetTouch(i).position;
                touchId = Input.GetTouch(i).fingerId;
                break;
            }
        }
        position = pos;
        return pos != Vector2.zero;
#elif UNITY_EDITOR
        position = Input.mousePosition;
        touchId = -1;
        return Input.GetMouseButtonDown(0);
        #endif
    }
    private bool ShouldStop(Vector2 pos)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        return pos == Vector2.zero;
#else
        return Input.GetMouseButtonUp(0);
#endif
    }
    private bool IsInputPressingDown(int touchId, out Vector2 position)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        if(touchId >= 0) for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId == touchId)
            {
                position = Input.GetTouch(i).position;
                return true;
            }
        }
        position = Vector2.zero;
        return false;
#else
        position = Input.mousePosition;
        return Input.GetMouseButton(0);
#endif
    }
	void Update ()
    {
        float magnitude = 0;
        Vector2 pos;
        if(IsInputInRange(out pos, out lastTouchId))
        {
            magnitude = (pos - center).magnitude;
            if (magnitude < radius)
            {
                foreground.transform.position = pos;
                isPressing = true;
            }
        }

        if (ShouldStop(pos))
        {
            foreground.transform.position = center;
            lastTouchId = -1;
            isPressing = false;
        }

        if (IsInputPressingDown(lastTouchId, out pos) && isPressing)
        {
            Vector3 mousePosition = new Vector3(pos.x, pos.y);
            Vector3 diff = mousePosition - background.transform.position;
            float distance = diff.magnitude;
            //Debug.Log(distance);
            if (distance > background.rectTransform.rect.height / 2)
            {
                float scale = (background.rectTransform.rect.height / 2) / distance;
                diff.x *= scale;
                diff.y *= scale;
                pos = background.transform.position + diff;
            }
            foreground.transform.position = pos;
        }
	}
}
