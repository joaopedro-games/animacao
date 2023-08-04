using System;
using UnityEngine;

public class Control_Skull : MonoBehaviour
{
    public AudioClip sfxWalk, sfxAttack, sfxInjured, sfxGroggy, sfxBG;  //sounds. duh.
    public Animator animSkull;   //the character's Animator game object

    float displaceX, maxDisplaceX;    //displacement and its bounds across X axis
    float dir, speed;  //dir = direction character is facing
    float jumpStartPos, jumpCurrPos; //returns coordinates when jump starts and during its execution
    float totalHeight;
    bool isJumpUp, isJumpDown;  //jumping states

    //misc stuff (rigidbody, sfx)
    Rigidbody2D rbSkull;
    AudioSource emissionSound;

    [SerializeField]    //displays private variables on Unity GUI (?)

    void Start()
    {
        maxDisplaceX = 8.5f;
        speed = 8f;
        isJumpUp = false;
        rbSkull = GetComponent<Rigidbody2D>();
        emissionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //jumping up
        if (isJumpUp == true && isJumpDown == false){
            jumpCurrPos = (float)Math.Round((double)transform.position.y, 3);
            if (jumpCurrPos > totalHeight){
                totalHeight = jumpCurrPos;
            } else if (jumpCurrPos < totalHeight){
                isJumpUp = false;
                isJumpDown = true;
                animSkull.SetBool("isJumpUp", false);
                animSkull.SetBool("isJumpDown", true);
            }
        }

        //jumping down
        if (isJumpUp == false && isJumpDown == true)
        {
            jumpCurrPos = (float)Math.Round((double)transform.position.y, 3);
            if (jumpCurrPos <= jumpStartPos)
            {
                isJumpDown = false;
                animSkull.SetBool("isJumpDown", false);
                animSkull.SetBool("isIdle", true);
            }
        }

        //attack animation
        if (Input.GetButtonDown("Fire1"))
        {
            animSkull.SetBool("isAttack", true);
            animSkull.SetBool("isIdle", false);
            animSkull.SetBool("isWalk", false);
            Debug.Log("botão foi pressionado");
        }
        else
        {
            animSkull.SetBool("isAttack", false);
        }
    }

    void FixedUpdate()
    {
        displaceX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (this.transform.position.x >= maxDisplaceX)
        {
            this.transform.position = new Vector2(maxDisplaceX, this.transform.position.y);
        }
        if (this.transform.position.x <= -maxDisplaceX)
        {
            this.transform.position = new Vector2(-maxDisplaceX, this.transform.position.y);
        }

        //walking animation
        if (Input.GetAxis("Horizontal") != 0){
            if(Input.GetAxis("Horizontal") > 0){
                dir = 1f;
            }
            else{
                dir = -1f;
            }
            this.transform.localScale = new Vector3(dir, 1, 0);
            animSkull.SetBool("isWalk", true);
            animSkull.SetBool("isIdle", false);

            //PlaySound();  //sfx method that plays sfx
        }
        else{
            animSkull.SetBool("isWalk", false);
            animSkull.SetBool("isIdle", true);
            //emissionSound.Stop();   //stops sound whee.
        }

        //stun animation
        if (Input.GetKey(KeyCode.G)){
            animSkull.SetBool("isGroggy", true);
            animSkull.SetBool("isIdle", false);
            animSkull.SetBool("isWalk", false);
        }
        else{
            animSkull.SetBool("isGroggy", false);
        }

        //hurt animation
        if (Input.GetKeyDown(KeyCode.F)){
            animSkull.SetBool("isInjured", true);
            animSkull.SetBool("isIdle", false);
            animSkull.SetBool("isWalk", false);
        }
        else{
            animSkull.SetBool("isInjured", false);
        }

        //death
        if (Input.GetKeyDown(KeyCode.M))
        {
            speed = 0f;
            animSkull.SetBool("isDying", true);
            animSkull.SetBool("isIdle", false);
            animSkull.SetBool("isWalk", false);
        }
        //there'd be an else but I find it thematic not to have one ecks dee

        //actual jumping action
        if (Input.GetButtonDown("Jump")){
            jumpStartPos = (float)Math.Round((double)transform.position.y, 3);  //stores current position at time character jumps
            rbSkull.AddForce(Vector2.up * 8, ForceMode2D.Impulse);

            totalHeight = jumpStartPos - 0.001f;
            isJumpUp = true;
            isJumpDown = false;

            animSkull.SetBool("isJumpUp", true);
            animSkull.SetBool("isIdle", false);
            animSkull.SetBool("isWalk", false);
        }

        transform.Translate(displaceX, 0, 0);
    }

    void PlaySound(AudioClip sfx)
    {
        //lorem ipsum dolor amet
    }
}
