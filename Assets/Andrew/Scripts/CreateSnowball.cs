﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSnowball : MonoBehaviour {

    public GameObject SnowballTemplate;
    public float CoolDown;
    private float TimeRemaining;

    private AudioSource ASource;
    private SpriteRenderer sr;
    private Animator ani;

    public Texture2D crosshair;
    public Texture2D crosshairGrayScale;
    public Vector2 crosshairPos;
   
    // Start is called before the first frame update
    void Start() {
        SnowballTemplate.transform.position = new Vector3(9990, 9999); //im REALLY not a bad coder you are
        SnowballTemplate.GetComponent<Rigidbody2D>().gravityScale = 0;

        ASource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        TimeRemaining = CoolDown;
        Cursor.SetCursor(crosshair, crosshairPos, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update() {
        if (TimeRemaining <= CoolDown) TimeRemaining += Time.deltaTime;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dx = transform.position.x - mousePos.x;
        bool flip = sr.flipX;

        if ((dx > 0 && !flip || dx < 0 && flip) && TimeRemaining >= CoolDown && !PauseMenu.gameIsPaused) {
            Cursor.SetCursor(crosshair, crosshairPos, CursorMode.Auto);
            if (Input.GetButton("Fire1") && TimeRemaining >= CoolDown) {
                TimeRemaining = 0;
                ASource.pitch = Random.value / 5 + .9f;
                ASource.Play();
                ani.SetTrigger("aniThrow");
                GameObject Snowball = Instantiate(SnowballTemplate);
                Snowball.name = "SnowballClone";
            }
        } else if (!PauseMenu.gameIsPaused) {
            Cursor.SetCursor(crosshairGrayScale, crosshairPos, CursorMode.Auto);
        } else {
            Cursor.SetCursor(null, new Vector2(), CursorMode.Auto);
        }
    }
}
