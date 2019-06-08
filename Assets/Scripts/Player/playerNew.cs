using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float hTime;
    private GameObject scoreUI;
    private GameObject scrapsUI;
    private GameObject camera;
    private GameObject shopCamera;

    private GameObject gameOver;

    private GameObject pauseMenu;

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
    private int maxScore;
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

    public GameObject hitParticle;
    public GameObject failParticle;
    public GameObject healParticle;
    private int particledir;

    private LoseSound loseSound;

    private GameObject healthBarUI;
    private GameObject energyBarUI;

    private GameObject audioController;

    private List<Color> trailColors;

    private string masterBusString = "bus:/";
    FMOD.Studio.Bus masterBus;

    public GameObject textBoi;
    public GameObject curTextBoi;

    public bool active;
    public bool nearInteractable;

    public bool input;

    private AudioSource musicSource;

    void Start()
    {
        input = false;
        nearInteractable = false;
        active = true;
        Time.timeScale = 1.0f;
        combo = 0;
        qTime = 0;
        hTime = 0;
        curX = (int)transform.position.x;
        scraps = 999;
        score = 0;
        maxScore = 0;
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
        energyBarUI = GameObject.Find("Player Energy");
        healthBarUI = GameObject.Find("Player Health");
        audioController = GameObject.Find("AudioController");

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
        loseSound = gameOver.GetComponent<LoseSound>();
        gameOver.SetActive(false);

        trailColors = new List<Color>() {
            new Color(0f/255f, 16f/255f, 255f/255f, 255f/255f),
            new Color(255f/255f, 0f/255f, 247f/255f, 255f/255f),
            new Color(255f/255f, 0f/255f, 0f/255f, 255f/255f),
            new Color(255f/255f, 0f/255f, 0f/255f, 255f/255f)
        };

        masterBus = FMODUnity.RuntimeManager.GetBus(masterBusString);
    }

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenuElements");
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            GameObject.Find("Combo").GetComponent<comboShake>().changeCombo(combo);
            if (camera.GetComponent<Camera>().enabled)
            {
                //Debug.Log(maxScore);
                int difference = maxScore - score;
                if (score < maxScore)
                {
                    if (difference > 1000)
                    {
                        score += 500;
                    }
                    else if (difference > 600)
                    {
                        score += 300;
                    }
                    else if (difference > 200)
                    {
                        score += 100;
                    }
                    else if (difference > 100)
                    {
                        score += 20;
                    }
                    else if (difference > 60)
                    {
                        score += 15;
                    }
                    else if (difference > 20)
                    {
                        score += 10;
                    }
                    else
                    {
                        score += 1;
                    }

                }
                scoreUI.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
                scrapsUI.GetComponent<UnityEngine.UI.Text>().text = "Scraps: " + scraps.ToString();
                // arbitrary score adds 100 for each game unit moved
                if ((int)transform.position.x > curX)
                {
                    maxScore += 100 * ((int)transform.position.x - curX);
                    curX = (int)transform.position.x;
                }
                // Get inputs and put them into the queue
                if (state != "stagger" && input && !pauseMenu.activeInHierarchy)
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
                    else if ((Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Space)) && !nearInteractable)
                    {
                        inputQueue = "misc";
                        qTime = 0.4f;
                    }
                    else if (Input.GetAxis("L2") > 0 || Input.GetKey("f"))
                    {
                        qTime = 0.4f;
                        if(hTime <= 0)
                        {
                            inputQueue = "heal";
                        }
                        hTime = 0.1f;
                    }
                }

                //Empty the queue if something's been in there too long
                qTime -= Time.deltaTime;
                if (qTime <= 0)
                {
                    inputQueue = "";
                }

                //Healing timer
                if(hTime > 0)
                {
                    hTime -= Time.deltaTime;
                }

                if (state == "idle" || state == "run" || state == "stagger")
                {
                    gameObject.GetComponent<playerMove>().setAttacking(false);
                    CheckQueue();
                }
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
                        if (currentHealth != maxHealth && spendScraps(1))
                        {
                            increaseHealth(.5f);
                        }
                    }
                    if (state != "idle" && state != "run") //If this during an attack string
                    {
                        state = "idle";
                        anim.SetTrigger("backtoIdle");
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
                if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 && input)
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

        // Set the colliders being used based on current attack type
        SphereCollider[] collidersArray = null;
        List<SphereCollider> activeColliders = new List<SphereCollider>();
        int currentColliderNum = -1;

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

        collidersArray = currentOutfitItem.attackColliderArrays[currentHitNum];
        
        currentOutfitItem.trails[currentHitNum].startColor = trailColors[currentHitNum];
        currentOutfitItem.trails[currentHitNum].endColor = trailColors[currentHitNum];
        currentOutfitItem.trails[currentHitNum].enabled = true;
        
        // Go through each phase of the attack based on the outfit attack stats
        for (int i = 0; i < currentOutfitItem.GetPhases(currentHitNum); i++)
        {
            startTime += Time.deltaTime;
            // Reset hit counter and set speed
            hit = false;

            //If this phase is an active phase, get a list of the current active colliders (length 1 if non-continuous, otherwise use the whole array).
            if (currentOutfitItem.GetPhaseActive(currentHitNum, i))
            {
                activeColliders.Clear();
                currentColliderNum++;
                if(currentOutfitItem.continuousHitbox[currentHitNum])
                {
                    activeColliders.AddRange(collidersArray);
                }
                else
                {
                    activeColliders.Add(collidersArray[currentColliderNum]);
                }
            }

            // List of enemies hit thus far. Used for continuous attacks like outfit 2 kick 3.
            List<int> enemiesHit = new List<int>();

            // Go through this phase's timer
            for (float j = 0; j < currentOutfitItem.GetPhaseTime(currentHitNum, i); j += Time.deltaTime)
            {
                // if this phase is an active hitbox and hasn't hit an enemy yet, try to hit an enemy
                if (currentOutfitItem.GetPhaseActive(currentHitNum, i) && hit == false)
                {
                    if (currentOutfitItem.projectiles[currentHitNum] != null && j == 0)
                    {
                        if(energy >= 100 * (currentHitNum + 1))
                        {
                            var currOutfitScript = currentOutfitItem.GetComponent<outfits>();
                            Vector3 p;
                            if (transform.rotation.y < 0)
                            {
                                p = new Vector3(transform.position.x + currOutfitScript.offsets[0 + 3 * currentHitNum],
                                transform.position.y + currOutfitScript.offsets[1 + 3 * currentHitNum],
                                transform.position.z + currOutfitScript.offsets[2 + 3 * currentHitNum]);
                            }
                            else
                            {
                                p = new Vector3(transform.position.x - currOutfitScript.offsets[0 + 3 * currentHitNum],
                                transform.position.y + currOutfitScript.offsets[1 + 3 * currentHitNum],
                                transform.position.z + currOutfitScript.offsets[2 + 3 * currentHitNum]);
                            }
                            
                            Instantiate(currentOutfitItem.projectiles[currentHitNum], p, transform.rotation);
                            useEnergy(100 * (currentHitNum + 1));
                            currentHitNum = 0;
                        }else if(j == 0)
                        {
                            Instantiate(failParticle, transform.Find("FrontCollider").position, transform.rotation);
                            currentHitNum = 0;
                        }
                    }
                    else
                    {
                        if(attackType == "misc" && j == 0)
                        {
                            if (energy >= 100 * (currentHitNum + 1))
                            {
                                useEnergy(100 * (currentHitNum + 1));
                            }
                            else
                            {
                                Instantiate(failParticle, transform.Find("FrontCollider").position, transform.rotation);
                                currentHitNum = 0;
                                break;
                            }
                        }

                        foreach (SphereCollider s in activeColliders)
                        {
                            // If debugging is on, show hitbox(es)
                            if (debugmode)
                                s.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            Collider[] cols = Physics.OverlapSphere(s.bounds.center, s.radius, LayerMask.GetMask("Default"));
                            //Debug.Log(cols.Length);
                            foreach (Collider c in cols)
                            {
                                //Debug.Log(c.name);
                                if (c.tag == "Enemy" && !enemiesHit.Contains(c.gameObject.GetInstanceID()))
                                {
                                    if (c.GetComponent<EnemyGeneric>().deadForPlayer == false || c.GetComponent<EnemyMovement>().state != "knockdown")
                                    {
                                        if (!c.name.Contains("Miniboss") || (c.name.Contains("Miniboss") && c.GetComponent<Miniboss>().vulnerable))
                                        {
                                            combo++;
                                            maxScore += combo * 553;
                                            spawnText("+" + (combo * 553).ToString());
                                            increaseEnergy(10);
                                            // Decrease the hit target's health based on the attack's damage
                                            c.GetComponent<EnemyGeneric>().TakeDamage(currentOutfitItem.attackDamage[currentHitNum], currentOutfitItem.isKnockdown[currentHitNum]);
                                            GameObject p = Instantiate(hitParticle, s.bounds.center, transform.rotation, null);
                                            p.transform.Rotate(0, 90, 0);
                                        }

                                        // SFX
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

                                        // Hitpause + screenshake
                                        StartCoroutine("hitpause");
                                        camera.GetComponent<CameraScript>().doShake(0.07f);
                                        if (currentOutfitItem.continuousHitbox[currentHitNum])
                                        {
                                            enemiesHit.Add(c.gameObject.GetInstanceID());
                                        }
                                        else
                                        {
                                            hit = true;
                                        }
                                    }
                                }
                                if (c.tag == "Destructible")
                                {
                                    hit = true;
                                    combo++;
                                    maxScore += combo * 553;
                                    spawnText("+" + (combo * 553).ToString());
                                    increaseEnergy(10);
                                    c.GetComponent<Destructible>().doBreak();

                                    // SFX
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

                                    // Hitpause + screenshake
                                    StartCoroutine("hitpause");
                                    camera.GetComponent<CameraScript>().doShake(0.07f);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (debugmode)
                    {
                        foreach (SphereCollider c in activeColliders)
                        {
                            c.GetComponent<SkinnedMeshRenderer>().enabled = false;
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
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
    }

    public void increaseEnergy(int energyGained)
    {
        energy += energyGained;
        if(energy > maxEnergy)
        {
            energy = maxEnergy;
            energyGained = maxEnergy - energy;
        }
        GameObject.Find("Main Canvas").GetComponent<textSpawner>().spawnText("Energy", (float)energyGained, true);
        updateBars();
    }

    public void spawnText(string textForSpawner)
    {
        Debug.Log(textForSpawner);
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        curTextBoi = Instantiate(textBoi, pos, Quaternion.identity);
        curTextBoi.GetComponent<PopupText>().assignText(textForSpawner);
        //Debug.Log(curTextBoi.GetComponent<PopupText>().currentText);
    }
    public void useEnergy(int energyUsed)
    {
        energy -= energyUsed;
        if(energy < 0)
        {
            energyUsed = energy;
            energy = 0;
        }
        GameObject.Find("Main Canvas").GetComponent<textSpawner>().spawnText("Energy", (float)energyUsed, false);
        updateBars();
    }

    public void updateBars()
    {
        if (energy > 200)
        {
            energyBars[2].transform.localScale = new Vector3(((float)(energy - 200) / 100) * 1.062334f, 1f, 1f);
            energyBars[1].transform.localScale = new Vector3(1.062334f, 1f, 1f);
            energyBars[0].transform.localScale = new Vector3(1.062334f, 1f, 1f);
        }
        else if (energy > 100)
        {
            energyBars[2].transform.localScale = new Vector3(0, 1f, 1f);
            energyBars[1].transform.localScale = new Vector3(((float)(energy - 100) / 100) * 1.062334f, 1f, 1f);
            energyBars[0].transform.localScale = new Vector3(1.062334f, 1f, 1f);
        }
        else
        {
            energyBars[2].transform.localScale = new Vector3(0, 1f, 1f);
            energyBars[1].transform.localScale = new Vector3(0, 1f, 1f);
            energyBars[0].transform.localScale = new Vector3(((float)energy / 100) * 1.062334f, 1f, 1f);
        }
    }

    public void decreaseHealth(float damage)
    {
        GameObject.Find("Main Canvas").GetComponent<textSpawner>().spawnText("Health", damage, false);
        // Play hit sound effect
        AudioClip clip = GetRandomPunch();
        audioSource.PlayOneShot(clip);
        // Do stagger
        inputQueue = "";
        state = "stagger";
        gameObject.GetComponent<playerMove>().setStagger(true);
        StopCoroutine("launchAttack");
        StartCoroutine("Stagger");

        increaseEnergy((int)damage / 2);
        combo = 0;
        currentHitNum = 0;
        maxScore -= (int)damage * 12;
        spawnText("-" + ((int)damage * 12).ToString());
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
        //Debug.Log(currentHealth);
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
            return true;
        }
        return false;
    }

    public bool spendScore(int scoreSpediture)
    {
        if (score >= scoreSpediture)
        {
            score -= scoreSpediture;
            return true;
        }
        return false;
    }

    public void increaseHealth(float heal)
    {
        GameObject.Find("Main Canvas").GetComponent<textSpawner>().spawnText("Health", heal, true);

        // Make a green healing particle
        Vector3 particlepos = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z);
        GameObject newHeal = Instantiate(healParticle, particlepos, Quaternion.identity);
        var main = newHeal.GetComponent<ParticleSystem>().main;
        main.startColor = Color.green;

        currentHealth += heal;
        if (currentHealth >= 100)
        {
            heal = 0;
            currentHealth = 100;
        }
        healthbar.transform.localScale += new Vector3(heal / 100, 0, 0);
        //Debug.Log(currentHealth);
        //healthbar.value = currentHealth / maxHealth;
        // If health drops to or bellow 0 then the player dies
        
    }

    private void killPlayer()
    {
        //FMODUnity.RuntimeManager.MuteAllEvents(true);
        //score -= 1000;
        controller.enabled = false;
        //controller.transform.position = checkpoint.getCheckpoint();
        //controller.enabled = true;
        //currentHealth = maxHealth;
        //healthbar.value = currentHealth / maxHealth;
        //transform.position = checkpoint.getCheckpoint();
        //canvas.SendMessage("PlayerDead", true);
        masterBus.setMute(true);
        musicSource = GameObject.Find("Main Canvas").GetComponent<SceneFade>().source;
        musicSource.enabled = false;
        healthbar.SetActive(false);
        for (int i = 0; i < 3; i++)
            energyBars[i].SetActive(false);
        scrapsUI.SetActive(false);
        healthBarUI.SetActive(false);
        energyBarUI.SetActive(false);
        audioController.SetActive(false);
        Time.timeScale = 0.0f;
        gameOver.SetActive(true);
        loseSound.Play();
        //Destroy(this.gameObject);
    }

    public bool revive()
    {
        if (spendScore(500))
        {
            Debug.Log("revived");
            controller.enabled = true;
            masterBus.setMute(false);
            healthbar.SetActive(true);
            for (int i = 0; i < 3; i++)
                energyBars[i].SetActive(true);
            scrapsUI.SetActive(true);
            healthBarUI.SetActive(true);
            energyBarUI.SetActive(true);
            audioController.SetActive(true);
            Time.timeScale = 1.0f;
            gameOver.SetActive(false);
            updateBars();
            healthbar.transform.localScale += new Vector3( currentHealth/ 100, 1, 1);
            return true;
        }
        else
        {
            Debug.Log("NOWORK");
            return false;
        }
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
        newOutfit.outfitSkinRenderer.sharedMesh = newOutfit.outfitMesh;
        if(newOutfit.outfitType == "Misc")
        {
            
            Material[] mats = new Material[2];
            //mats[0] = face;
            mats[0] = newOutfit.outfitMaterial;
            mats[1] = newOutfit.outfitMaterial;
            newOutfit.outfitSkinRenderer.materials = mats;
        }else
        {
            newOutfit.outfitSkinRenderer.material = newOutfit.outfitMaterial;
        }

        newOutfit.outfitSkinRenderer.GetComponent<Cloth>().enabled = newOutfit.clothPhys;
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


