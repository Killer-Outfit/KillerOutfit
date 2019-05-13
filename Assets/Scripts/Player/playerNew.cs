using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNew : MonoBehaviour
{
    public bool debugmode = false; // set true for hitbox debugging

    private GameObject enemyManager;
    private CharacterController controller;
    private Animator anim;
    private AnimatorOverrideController animatorOverrideController;
    private float maxHealth;
    private string attackType;
    private string inputQueue;
    private int currentHitNum;
    private int maxEnergy;
    private outfits top;
    private outfits misc;
    private outfits bot;

    private GameObject healthbar;
    private GameObject[] energyBars;

    private int curX;
    private int combo;
    private float qTime;
    private GameObject scoreUI;
    private GameObject scrapsUI;
    private GameObject camera;
    private GameObject shopCamera;

    private GameObject gameOver;

    [SerializeField]
    private AudioClip[] punchSounds;
    [SerializeField]
    private AudioClip[] kickSounds;
    [SerializeField]
    private AudioClip[] miscSounds;
    private AudioSource audioSource;

    public Material face;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public int scraps;

    [HideInInspector]
    public string state;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public int energy;
   

    void Start()
    {
        combo = 0;
        qTime = 0;
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
        shopCamera = GameObject.Find("OutfitCamera");

        top = GameObject.Find("TOP_1").GetComponent<outfits>();
        misc = GameObject.Find("MISC_1").GetComponent<outfits>();
        bot = GameObject.Find("BOT_1").GetComponent<outfits>();

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

        gameOver = GameObject.Find("GameOverElements");
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		if (camera.GetComponent<Camera>().enabled)
		{
			scoreUI.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
			scrapsUI.GetComponent<UnityEngine.UI.Text>().text = "Scraps: " + scraps.ToString();
			// arbitrary score adds 100 for each game unit moved
			if ((int)transform.position.x > curX)
			{
				score += 100 * ((int)transform.position.x - curX);
				curX = (int)transform.position.x;
			}
			// Get inputs and put them into the queue
			if(state != "stagger")
			{
				if (Input.GetButtonDown("XButton") || Input.GetMouseButtonDown(0))
				{
					//Instantiate(laser, transform.position, transform.rotation);
					inputQueue = "punch";
                    qTime = 0.4f;
				}
				else if (Input.GetButtonDown("YButton") || Input.GetMouseButtonDown(1))
				{
					inputQueue = "kick";
                    qTime = 0.4f;
                }
				else if (Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Space))
				{
					inputQueue = "misc";
                    qTime = 0.4f;
                }
				else if (Input.GetAxis("L2") > 0 || Input.GetKey("f"))
				{
					inputQueue = "heal";
                    qTime = 0.4f;
                }
			}

            //Empty the queue if something's been in there too long
            qTime -= Time.deltaTime;
            if(qTime <= 0)
            {
                inputQueue = "";
            }

			if (state == "idle" || state == "run" || state == "stagger")
			{
				gameObject.GetComponent<playerMove>().setAttacking(false);
				CheckQueue();
			}
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
                    if (debugmode)
                        attack.GetComponent<SkinnedMeshRenderer>().enabled = true;
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
                else
                {
                    if (debugmode)
                        attack.GetComponent<SkinnedMeshRenderer>().enabled = false;
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
        {   // If the player dies, enable the Game Over menu so players can restart or quit to main menu
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
        //score -= 1000;
        controller.enabled = false;
        //controller.transform.position = checkpoint.getCheckpoint();
        controller.enabled = true;
        Destroy(this.gameObject);
        currentHealth = maxHealth;
        //healthbar.value = currentHealth / maxHealth;
        //transform.position = checkpoint.getCheckpoint();
        //canvas.SendMessage("PlayerDead", true);
        Time.timeScale = 0.0f;
        gameOver.SetActive(true);
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
            //mats[1] = face;
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


