using System.Collections.Generic;
using UnityEngine;

public class AlphaController : MonoBehaviour
{
    public int maxElements = 5;
    private List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
    public void AddElement(CanvasGroup newGroup)
    {
        // Добавляем в начало списка
        canvasGroups.Insert(0, newGroup);
        newGroup.transform.SetAsFirstSibling();

        if (canvasGroups.Count > maxElements)
        {
            CanvasGroup toRemove = canvasGroups[canvasGroups.Count - 1];
            canvasGroups.RemoveAt(canvasGroups.Count - 1);
            Destroy(toRemove.gameObject);
        }

        UpdateOpacity();
    }

    private void UpdateOpacity()
    {
        int count = canvasGroups.Count;
        for (int i = 0; i < count; i++)
        {
            float alpha = 1f;
            if (count > 1)
                alpha = 1f - (i / (float)(maxElements - 1));

            canvasGroups[i].alpha = alpha;
            canvasGroups[i].interactable = alpha > 0.5f;
            canvasGroups[i].blocksRaycasts = alpha > 0.5f;
        }
    }

    public void AddNewElement(CanvasGroup canvasGroup)
    {
        AddElement(canvasGroup);
    }
}
