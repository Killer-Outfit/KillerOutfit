using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNew : MonoBehaviour
{

    private GameObject enemyManager;

    public Material face;
    [HideInInspector]
    public int score;
    private CharacterController controller;
    public Collider laserSpawn;
    public Animator anim;
    private AnimatorOverrideController animatorOverrideController;
    private float maxHealth;
    [HideInInspector]
    public float currentHealth;
    private string attackType;
    private string inputQueue;
    private int currentHitNum;
    private int maxEnergy;
    [HideInInspector]
    public int scraps;

    [SerializeField]
    private AudioClip[] punchSounds;
    [SerializeField]
    private AudioClip[] kickSounds;
    [SerializeField]
    private AudioClip[] miscSounds;

    private AudioSource audioSource;

    //[HideInInspector]
    public string state;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public int energy;
    public outfits top;
    public outfits misc;
    public outfits bot;

    public outfits top1;
    public outfits misc1;
    public outfits bot1;

    public outfits top2;
    public outfits misc2;
    public outfits bot2;

    public bool outfitT;
    public bool outfitM;
    public bool outfitB;

    public int bodyPart;

    public Collider laser;
    private GameObject healthbar;
    private GameObject[] energyBars;

    private int curX;
    private int combo;
    private GameObject scoreUI;
    private GameObject scrapsUI;
    private GameObject camera;

    void Start()
    {
        combo = 0;
        curX = (int)transform.position.x;
        scraps = 0;
        score = 0;
        // Initialize UI bar objects
        energyBars = new GameObject[3];
        energyBars[0] = GameObject.Find("B1");
        energyBars[1] = GameObject.Find("B2");
        energyBars[2] = GameObject.Find("B3");
        healthbar = GameObject.Find("HBar");
        scoreUI = GameObject.Find("Score");
        scrapsUI = GameObject.Find("ScrapCount");
        camera = GameObject.Find("Main Camera");

        // Temp vars for outfit switching
        bodyPart = 1;
        outfitT = true;
        outfitM = true;
        outfitB = true;

        enemyManager = GameObject.Find("Overmind");

        audioSource = GetComponent<AudioSource>();
        maxEnergy = 300;
        energy = maxEnergy;
        controller = GetComponent<CharacterController>();
        maxHealth = 100;
        currentHealth = maxHealth;
        state = "idle";
        attackType = "";
        inputQueue = "";
        anim = GetComponent<Animator>();
        currentHitNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
        scrapsUI.GetComponent<UnityEngine.UI.Text>().text = "Scraps: " + scraps.ToString();
        // arbitrary score adds 100 for each game unit moved
        if ((int)transform.position.x > curX)
        {
            score += 100 * ((int)transform.position.x - curX);
            curX = (int)transform.position.x;
        }
        // Get inputs
        if(state != "stagger")
        {
            if (Input.GetButtonDown("XButton") || Input.GetMouseButtonDown(0))
            {
                //Instantiate(laser, transform.position, transform.rotation);
                inputQueue = "punch";
            }
            else if (Input.GetButtonDown("YButton") || Input.GetMouseButtonDown(1))
            {
                inputQueue = "kick";
            }
            else if (Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Space))
            {
                inputQueue = "misc";
            }
            else if (Input.GetButtonDown("BButton"))
            {
                inputQueue = "change";
            } else if (Input.GetAxis("L2") > 0 || Input.GetKey("f"))
            {
                Debug.Log("pressing L2");
                inputQueue = "heal";
            }
        }

        if (state == "idle" || state == "run" || state == "stagger")
        {
            gameObject.GetComponent<playerMove>().setAttacking(false);
            CheckQueue();
        }

    }

    public void CheckQueue()
    {
        if(state != "stagger")
        {
            if (inputQueue != "")
            {
                if (inputQueue == "punch")
                {
                    pressX();
                }
                else if (inputQueue == "kick")
                {
                    pressY();
                }
                else if (inputQueue == "misc")
                {
                    pressA();
                }
                else if (inputQueue == "change")
                {
                    if (bodyPart == 1)
                    {
                        if (outfitT)
                        {
                            changeOutfit(top2);
                            outfitT = false;
                        }
                        else
                        {
                            changeOutfit(top1);
                            outfitT = true;
                        }
                    }
                    else if (bodyPart == 2)
                    {
                        if (outfitM)
                        {
                            changeOutfit(misc2);
                            outfitM = false;
                        }
                        else
                        {
                            changeOutfit(misc1);
                            outfitM = true;
                        }
                    }
                    else
                    {
                        if (outfitB)
                        {
                            changeOutfit(bot2);
                            outfitB = false;
                        }
                        else
                        {
                            changeOutfit(bot1);
                            outfitB = true;
                        }
                    }
                    bodyPart += 1;
                    if (bodyPart == 4)
                    {
                        bodyPart = 1;
                    }
                }
                else if (inputQueue == "heal")
                {
                    if (enemyManager.GetComponent<Overmind>().areThereEnemies())
                    {
                        if (spendScraps(1) && currentHealth != maxHealth)
                        {
                            increaseHealth(.5f);
                        }
                    }
                }
                inputQueue = "";
            }
            else // No inputs at the moment
            {
                currentHitNum = 0;
                if (state != "idle" && state != "run") //If this is the end of an attack string
                {
                    anim.SetTrigger("backtoIdle");
                }
                if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                {
                    state = "run";
                    anim.SetBool("isIdle", false);
                }
                else
                {
                    state = "idle";
                    anim.SetBool("isIdle", true);
                }
                gameObject.GetComponent<playerMove>().setAttacking(false);

            }
        }
    }

    // Activate punch
    public void pressX()
    {
        //Instantiate(laser, transform.position, transform.rotation);
        //Instantiate(laserSpawn, transform.position, transform.rotation);
        //Debug.Log("pressed x");
        anim.SetTrigger("punch");
        attackType = "punch";
        state = "attacking";
        gameObject.GetComponent<playerMove>().setAttacking(true);
        StartCoroutine("launchAttack");
    }

    // Activate kick
    public void pressY()
    {
        anim.SetTrigger("kick");
        attackType = "kick";
        state = "attacking";
        gameObject.GetComponent<playerMove>().setAttacking(true);
        StartCoroutine("launchAttack");
    }

    // Activate misc attack
    public void pressA()
    {
        anim.SetTrigger("miscAttack");
        attackType = "misc";
        state = "attacking";
        gameObject.GetComponent<playerMove>().setAttacking(true);
        StartCoroutine("launchAttack");
    }

    // Make the attack activate
    IEnumerator launchAttack()
    {
        float startTime = 0f;
        bool hit;
        outfits currentOutfitItem = null;
        // Set the collider being used based on current attack type
        Collider attack = null;
        if (attackType == "punch")
        {
            currentOutfitItem = top;
        }
        else if (attackType == "kick")
        {
            currentOutfitItem = bot;
        }
        else if (attackType == "misc")
        {
            currentOutfitItem = misc;
        }
        anim.SetFloat("animSpeed", currentOutfitItem.animSpeedMultiplier[currentHitNum]);
        attack = currentOutfitItem.attackColliders[currentHitNum];
        currentOutfitItem.trails[currentHitNum].enabled = true;
        // Go through each phase of the attack based on the outfit attack stats
        for (int i = 0; i < currentOutfitItem.GetPhases(currentHitNum); i++)
        {
            startTime += Time.deltaTime;
            // Reset hit counter and set speed
            hit = false;
            //GetComponent<playerMove>().movementSpeed = currentOutfitItem.GetPhaseMove(currentHitNum, i);
            //GetComponent<PlayerMove>().collideMaxSpeed = currentOutfitItem.GetPhaseMove(currentHitNum, i);
            //GetComponent<PlayerMove>().turningSpeed = currentOutfitItem.GetPhaseTurnSpeed(currentHitNumber, i);

            // Go through this phase's timer
            for (float j = 0; j < currentOutfitItem.GetPhaseTime(currentHitNum, i); j += Time.deltaTime)
            {
                // Apply acceleration
                //GetComponent<playerMove>().movementSpeed += currentOutfitItem.GetPhaseAcc(currentHitNum, i);

                // if this phase is an active hitbox and hasn't hit an enemy yet, try to hit an enemy
                if (currentOutfitItem.GetPhaseActive(currentHitNum, i) && hit == false)
                {
                    if (attackType == "misc" && j == 0)
                    {
                        if(energy >= 100 * (currentHitNum + 1))
                        {
                            Instantiate(currentOutfitItem.projectiles[currentHitNum], transform.position, transform.rotation);
                            useEnergy(100 * (currentHitNum + 1));
                            currentHitNum = 0;
                        }else if(j == 0)
                        {
                            Debug.Log("you dumb");
                        }
                        
                        
                    }
                    else
                    {
                        Collider[] cols = Physics.OverlapBox(attack.bounds.center, attack.bounds.extents, attack.transform.rotation, LayerMask.GetMask("Default"));
                        //Debug.Log(cols.Length);
                        foreach (Collider c in cols)
                        {
                            //Debug.Log(c.name);
                            if (c.tag == "Enemy")
                            {
                                combo++;
                                score += combo * 553;
                                increaseEnergy(10);
                                // Decrease the hit target's health based on the attack's damage
                                //Debug.Log("hit enemy");
                                c.GetComponent<EnemyGeneric>().TakeDamage(currentOutfitItem.attackDamage[currentHitNum], false); // Change knockdown array
                                StartCoroutine("hitpause");
                                camera.GetComponent<CameraScript>().doShake(0.02f);
                                hit = true;
                                if (currentOutfitItem == top)
                                {
                                    AudioClip clip = GetRandomPunch();
                                    audioSource.PlayOneShot(clip);
                                }
                                if (currentOutfitItem == bot)
                                {
                                    AudioClip clip = GetRandomKick();
                                    audioSource.PlayOneShot(clip);
                                }
                                if (currentOutfitItem == misc)
                                {
                                    AudioClip clip = GetRandomMisc();
                                    audioSource.PlayOneShot(clip);
                                }
                            }
                        }
                    }
                }
                yield return null;
            }
        }

        //GetComponent<PlayerMove>().DefaultTurn();
        //GetComponent<PlayerMove>().DefaultSpeed();
        currentOutfitItem.trails[currentHitNum].enabled = false;

        currentHitNum++;
        if (currentHitNum == 3)
        {
            currentHitNum = 0;
        }
        CheckQueue();
    }

    private IEnumerator hitpause()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
    }

    public void increaseEnergy(int energyGained)
    {
        energy += energyGained;
        if(energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        updateBars();
    }

    public void useEnergy(int energyUsed)
    {
        energy -= energyUsed;
        if(energy < 0)
        {
            energy = 0;
        }

        updateBars();
    }

    public void updateBars()
    {
        if (energy > 200)
        {
            energyBars[2].transform.localScale = new Vector3(((energy - 200) / 100) * 1.062334f, 1f, 1f);
            energyBars[1].transform.localScale = new Vector3(1.062334f, 1f, 1f);
            energyBars[0].transform.localScale = new Vector3(1.062334f, 1f, 1f);
        }
        else if (energy > 100)
        {
            energyBars[2].transform.localScale = new Vector3(0, 1f, 1f);
            energyBars[1].transform.localScale = new Vector3(((energy - 100) / 100) * 1.062334f, 1f, 1f);
            energyBars[0].transform.localScale = new Vector3(1.062334f, 1f, 1f);
        }
        else
        {
            energyBars[2].transform.localScale = new Vector3(0, 1f, 1f);
            energyBars[1].transform.localScale = new Vector3(0, 1f, 1f);
            energyBars[0].transform.localScale = new Vector3((energy / 100) * 1.062334f, 1f, 1f);
        }
    }

    public void decreaseHealth(float damage)
    {
        // Do stagger
        inputQueue = "";
        state = "stagger";
        gameObject.GetComponent<playerMove>().setStagger(true);
        StopCoroutine("launchAttack");
        StartCoroutine("Stagger");

        increaseEnergy((int)damage / 2);
        combo = 0;
        currentHitNum = 0;
        score -= (int)damage * 12;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthbar.transform.localScale = new Vector3(0, 0, 0);
            killPlayer();
        }
        else
        {
            healthbar.transform.localScale -= new Vector3(damage / 100, 0, 0);
        }
        Debug.Log(currentHealth);
        //healthbar.value = currentHealth / maxHealth;
        // If health drops to or bellow 0 then the player dies
        
    }

    private IEnumerator Stagger()
    {
        top.trails[currentHitNum].enabled = false;
        bot.trails[currentHitNum].enabled = false;
        misc.trails[currentHitNum].enabled = false;
        anim.SetTrigger("stagger");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("backtoIdle");
        state = "idle";
        gameObject.GetComponent<playerMove>().setStagger(false);
    }

    public bool spendScraps(int scrapSpediture)
    {
        if (scraps >= scrapSpediture)
        {
            scraps -= scrapSpediture;
            return false;
        }
        return true;
    }

    public void increaseHealth(float heal)
    {
        currentHealth += heal;
        if (currentHealth >= 100)
        {
            heal = 0;
            currentHealth = 100;
        }
        healthbar.transform.localScale += new Vector3(heal / 100, 0, 0);
        Debug.Log(currentHealth);
        //healthbar.value = currentHealth / maxHealth;
        // If health drops to or bellow 0 then the player dies
        
    }

    private void killPlayer()
    {
        score -= 1000;
        controller.enabled = false;
        //controller.transform.position = checkpoint.getCheckpoint();
        controller.enabled = true;
        Destroy(this.gameObject);
        currentHealth = maxHealth;
        //healthbar.value = currentHealth / maxHealth;
        //transform.position = checkpoint.getCheckpoint();
        //canvas.SendMessage("PlayerDead", true);
    }

    // Change outfit function takes in the new outfit 
    public void changeOutfit(outfits newOutfit)
    {
        if (newOutfit.outfitType == "Top")
        {
            top = newOutfit;
        }
        else if (newOutfit.outfitType == "Misc")
        {
            misc = newOutfit;
        }
        else if (newOutfit.outfitType == "Bot")
        {
            bot = newOutfit;
        }
        // 
        newOutfit.outfitSkinRenderer.sharedMesh = newOutfit.outfitMesh;
        if(newOutfit.outfitType == "Misc")
        {
            Material[] mats = new Material[2];
            mats[0] = newOutfit.outfitMaterial;
            mats[1] = face;
            newOutfit.outfitSkinRenderer.materials = mats;
        }else
        {
            newOutfit.outfitSkinRenderer.material = newOutfit.outfitMaterial;
        }
        
        // Create new runtime animator override controller
        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        // Create a list of current animations and their replacements
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        int indexT = 0;
        int indexM = 0;
        int indexB = 0;
        // For each animation in the current animation tree
        foreach (var a in aoc.animationClips) { 
            // If an animation name contains the outfitType(must be the word punch, kick, and misc)
            if (a.name.Contains(top.attackType))
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, top.attacks[indexT]));
                indexT += 1;
            }
            else if (a.name.Contains(misc.attackType))
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, misc.attacks[indexM]));
                indexM += 1;
            }
            else if (a.name.Contains(bot.attackType))
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, bot.attacks[indexB]));
                indexB += 1;
            }
        }
        // Override all animations in the anims list
        aoc.ApplyOverrides(anims);
        anim.runtimeAnimatorController = aoc;
    }

    private AudioClip GetRandomPunch()
    {
        return punchSounds[UnityEngine.Random.Range(0, punchSounds.Length)];
    }
    private AudioClip GetRandomKick()
    {
        return kickSounds[UnityEngine.Random.Range(0, kickSounds.Length)];
    }
    private AudioClip GetRandomMisc()
    {
        return miscSounds[UnityEngine.Random.Range(0, miscSounds.Length)];
    }

}


