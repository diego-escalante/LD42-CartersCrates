using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    public Sprite carryStand;
    public Sprite carryRun;
    public Sprite stand;
    public Sprite run;

    private SpriteRenderer spriteRenderer;
    private Sprite a;
    private Sprite b;

    private string state;

    private float animationSpeed = 0.1f;
    private float elapsedTime = 0;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        setStand();
    }

    public void Update() {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= animationSpeed) {
            elapsedTime -= animationSpeed;
            if (spriteRenderer.sprite != a) {
                spriteRenderer.sprite = a;
            } else {
                spriteRenderer.sprite = b;
            }
        }
    }

    public void setStand() {
        if (state == "STAND") {
            return;
        }
        a = stand;
        b = stand;
        spriteRenderer.sprite = a;
        state = "STAND";
    }

    public void setCarryStand() {
        if (state == "CARRY_STAND") {
            return;
        }
        a = carryStand;
        b = carryStand;
        spriteRenderer.sprite = a;
        state = "CARRY_STAND";
    }

    public void setJump() {
        if (state == "JUMP") {
            return;
        }
        a = run;
        b = run;
        spriteRenderer.sprite = a;
        state = "JUMP";
    }

    public void setCarryJump() {
        if (state == "CARRY_JUMP") {
            return;
        }
        a = carryRun;
        b = carryRun;
        spriteRenderer.sprite = a;
        state = "CARRY_JUMP";
    }

    public void setRun()
    {
        if (state == "RUN") {
            return;
        }
        a = run;
        b = stand;
        spriteRenderer.sprite = a;
        elapsedTime = 0;
        state = "RUN";
    }

    public void setCarryRun()
    {
        if (state == "CARRY_RUN") {
            return;
        }
        a = carryRun;
        b = carryStand;
        spriteRenderer.sprite = a;
        elapsedTime = 0;
        state = "CARRY_RUN";
    }


}
