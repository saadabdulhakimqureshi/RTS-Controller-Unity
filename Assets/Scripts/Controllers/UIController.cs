using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIController instance;
    [SerializeField] public Image SelectionAreaImage;
    [SerializeField] public RectTransform SelectionAreaRectTransform;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSelectionArea(float width, float height, float x, float y)
    {
        SelectionAreaRectTransform.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        SelectionAreaRectTransform.anchoredPosition = new Vector2(x, y) + new Vector2(width / 2, height / 2);

    }

    public void EnableSelectionArea(bool enable)
    {
        SelectionAreaImage.enabled = enable;
    }
}
