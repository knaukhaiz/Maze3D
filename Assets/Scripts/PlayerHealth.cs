using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private CharacterController _CharacterController;

    public float health = 100f;
    bool isDead = false;
    GamePopup _GamePopup;

    public Image healthBarVal;

    private void Awake()
    {
        _GamePopup = FindObjectOfType<GamePopup>();
        _CharacterController = GetComponent<CharacterController>();
    }
    public void TakeDamage(float damage)
    {
        if(isDead)
        {
            return;
        }
        health -= damage;
        healthBarVal.fillAmount = health/100f;
        if (health < 1)
        {
            isDead = true;
            _GamePopup.GameEnd(2);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Collider collider = hit.collider;
        if(hit.gameObject.tag.Equals("Finish"))
            _GamePopup.GameEnd(1);
    }
}
