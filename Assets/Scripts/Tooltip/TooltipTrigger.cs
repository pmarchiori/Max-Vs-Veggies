using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private static LTDescr delay;
    public string header;

    [Multiline()]    
    public string content;

    private bool isMouseOver = false;
    private bool isClicked = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        isClicked = false; // Reset clicked state when mouse enters

        delay = LeanTween.delayedCall(0.5f, () => 
        {
            if (isMouseOver && !isClicked)
            {
                TooltipSystem.Show(content, header);
            }
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true;
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }
}
