﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBunnyHealth : MonoBehaviour
{
    //Holds max lives
    public float maxLives;
    //Holds current lives
    private float currLives;

    //Holds if the Boss can be hit
    private bool BunInvuln;
    //Holds hit cooldown
    private float coolDown;
    //Holds if dead
    private bool isDead;
    //Holds if timer is started
    private bool shakeStart = false;
    //Holds time to wait for screen shake
    private float shakeWait = 0.4f;

    //Component References
    private AudioSource aS;
    private Rigidbody2D rb2;
    private hitEffect hitEffect;
    private Animator anim;
    private ParticleSystem ps;
    private CameraScript cS; 

    // Start is called before the first frame update
    void Start()
    {
        //Get component References 
        aS = GetComponents<AudioSource>()[1];
        rb2 = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hitEffect = GetComponent<hitEffect>();
        ps = GetComponent<ParticleSystem>();
        cS = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        //Set up current lives
        currLives = maxLives;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit by snowball and not invuln
        if (collision.tag == "Snowball" && !BunInvuln)
        {
            //Set bunInvuln to true
            BunInvuln = true; 

            //If dead
            if (currLives - 1 <= 0 && !isDead)
            {
                //turns on is dead
                isDead = true;

                //Start dead animation
                anim.SetTrigger("AniBossDead");

                //start particles and sound effect when animation hits floor
                ps.Play();
                aS.PlayDelayed(0.2f);

                //Sets shake start to true
                shakeStart = true;

                //deactivate other scripts
                gameObject.GetComponent<BossMove>().enabled = false;
                gameObject.GetComponent<BossSnowBallThrow>().enabled = false;

                //Turns off gravity
                rb2.gravityScale = 0;

                //Stops all velocity
                rb2.velocity = new Vector2(0, 0);

                //Turn off collider
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            }

            //Activate hit effect
            hitEffect.hitEffectStart();

            //Increment lives
            currLives--;
        }
    }

    void Update()
    {
        //If BunInvuln is active
        if (BunInvuln)
        {
            //Increment colldown
            coolDown += Time.deltaTime;
        }

        //If cooldown is complete
        if (coolDown > .08)
        {
            //Reset BunInvuln and cooldown
            BunInvuln = false;
            coolDown = 0;
        }

        //If shakeStart is true
        if (shakeStart)
        {
            //Increment timer down to zero and then start screen shake
            if (shakeWait > 0)
            {
                shakeWait -= Time.deltaTime;
            }
            else
            {
                cS.StartShake();
                //Turn of shakeStart
                shakeStart = false;
            }
        }
    }

    //Gets current lives
    public float getLives()
    {
        return currLives;
    }

    //Gets current lives
    public float getMaxLives()
    {
        return maxLives;
    }
}
