using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public HeroesMovement heroesMovement;

    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider && raycastHit.collider.gameObject.CompareTag("Hero"))
            {
                heroesMovement = raycastHit.collider.gameObject.GetComponent<HeroesMovement>();
                heroesMovement.isHeroSelected = true;
            }
            
        }
    }
}
