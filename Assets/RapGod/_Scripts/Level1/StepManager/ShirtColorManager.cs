using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShirtColorManager : MonoBehaviour
{
    public LayerMask layer;
    public Renderer currentSelected;
    void Update()
    {
        SelectCharacter();
    }

    void SelectCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out RaycastHit hit, 100f, layer))
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(hit.collider.TryGetComponent(out PlayerCharacter player))
                {
                    currentSelected = player.shirt;
                }
            }
        }
    }

    public void ChangeColor(Image img)
    {
        Color col = img.color;
        currentSelected.material.color = col;
    }
}
