using UnityEngine;

public class StarSwitch : MonoBehaviour
{
    // 0 = bottom left, 1 = top, etc.
    public int switchIndex;
    public StarPuzzleController controller;

    public Sprite offSprite;
    public Sprite onSprite;

    private SpriteRenderer sr;
    private bool isOn = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    private void OnMouseDown()
    {
        // This only runs for the one you clicked
        Debug.Log("Clicked switch " + switchIndex);
        controller.HitSwitch(this);
    }

    public void SetOn(bool value)
    {
        isOn = value;
        UpdateSprite();
    }

    public void ResetSwitch()
    {
        isOn = false;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (sr == null) return;

        if (isOn && onSprite != null)
            sr.sprite = onSprite;
        else if (!isOn && offSprite != null)
            sr.sprite = offSprite;
    }
}