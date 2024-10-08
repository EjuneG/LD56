using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, ITarget
{
    [SerializeField] Animator animator;
    public float Energy;
    public float EnergyForNextLevel => slimeGrowthSO.EnergyNeeded[Level];
    public SlimeState slimeState;
    public ITarget currentTarget;
    [SerializeField] private ColoredFlash flashEffect;
    [SerializeField] private SpawnEnergyBall ballSpawner;
    public float FruitDetectionRange;
    
    Collider2D collider2D;

    [Header("Slime Stats")]
    public float MaxHP;
    public float CurrentHP;
    public float Damage;

    public int Level = 0;
    [SerializeField] private SlimeGrowthSO slimeGrowthSO; // growth thresholds

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip energyPickupSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip growSound;

    public void SwitchStateToEating() => SwitchStateTo(SlimeState.Eating);
    public void SwitchStateToIdle() => SwitchStateTo(SlimeState.Idle);

    private void Start(){
        collider2D = GetComponent<Collider2D>();
        CurrentHP = MaxHP;
        FruitDetectionRange = collider2D.bounds.extents.y;
    }
    private void OnEnable()
    {
        //Player Event
        if (this.gameObject.CompareTag("Player"))
        {
            GameEvent.OnEnergyPickup += PlayPickupSound;
        }
    }

    private void OnDisable()
    {
        //Player Event
        if (this.gameObject.CompareTag("Player"))
        {
            GameEvent.OnEnergyPickup -= PlayPickupSound;
        }
    }


    private void Update()
    {
        if (currentTarget != null)
        {
            if (slimeState == SlimeState.Idle)
            {
                return;
            }
            else if (slimeState == SlimeState.Eating)
            {
                if (eatSound != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(eatSound, 1.2f);
                }
                currentTarget.TakeDamage(Damage * Time.deltaTime, this);
            }
        }else{
            if (slimeState == SlimeState.Eating)
            {
                if(GetFruitNearby() == null)
                {
                    EndEat();
                }else {
                    currentTarget = GetFruitNearby();
                    Debug.Log($"Current target: {currentTarget}");
                }
            }
        }

        //HP regen
        if (gameObject.CompareTag("Player"))
        {
            RegenHP();
        }
    }

    //regen 1% of max HP per second
    private void RegenHP()
    {
        if (CurrentHP < MaxHP)
        {
            CurrentHP += MaxHP * 0.01f * Time.deltaTime;
        }
    }
    public void PlayMoveAnimation()
    {
        animator.Play("Move");
    }
    public void Eat()
    {
        animator.Play("Eat");
        if (slimeState != SlimeState.Eating)
        {
            SwitchStateToEating();
        }
    }

    public void EndEat()
    {
        animator.Play("Idle");
        if (slimeState != SlimeState.Idle)
        {
            SwitchStateToIdle();
        }
    }

    public bool CheckGrow()
    {
        if (Energy >= slimeGrowthSO.EnergyNeeded[Level] && Level < slimeGrowthSO.EnergyNeeded.Count - 1)
        {
            Grow();
            return true;
        }
        else if (Level == 4 && Energy >= slimeGrowthSO.EnergyNeeded[Level])
        {
            audioSource.PlayOneShot(growSound);
            StartCoroutine(ScaleOverTime(new Vector3(8.0f,8.0f,8.0f), 4f));
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void PlayPickupSound()
    {
        audioSource.PlayOneShot(energyPickupSound, 0.3f);
    }

    private void Grow()
    {
        Level++;
        Energy = 0;
        Damage = slimeGrowthSO.Damages[Level];
        MaxHP = slimeGrowthSO.MaxHP[Level];
        CurrentHP = MaxHP;
        if (this.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(growSound);
            if (Level == 1)
            {
                GameEvent.TriggerOnFirstSizeUp();
            }
            else if (Level == 3)
            {
                GameEvent.TriggerOnSecondSizeUp();
            }
        }
    }

    IEnumerator ScaleOverTime(Vector3 target, float duration)
    {
        Vector3 initialScale = transform.localScale;   // Starting scale
        float time = 0f;                               // Elapsed time

        // While scaling hasn't completed
        while (time < duration)
        {
            // Increase the elapsed time
            time += Time.deltaTime;

            // Interpolate between the initial scale and the target scale
            transform.localScale = Vector3.Lerp(initialScale, target, time / duration);

            // Wait until the next frame
            yield return null;
        }

        // Ensure the final scale is set exactly
        transform.localScale = target;

        GameManager.Instance.EndGame(EndCondition.Win);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            currentTarget = other.GetComponent<ITarget>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            currentTarget = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (this.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Slime"))
        {
            Slime otherSlime = other.gameObject.GetComponentInChildren<Slime>();
            otherSlime?.TakeDamage(Damage, this);
        }
    }

    //check if there is a fruit nearby, check range should be similar to the collider2d radius
    private ITarget GetFruitNearby(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, FruitDetectionRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Fruit"))
            {
                return collider.GetComponent<ITarget>();
            }
        }
        return null;
    }

    public void SwitchStateTo(SlimeState newState)
    {
        slimeState = newState;
    }

    public void Die()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.EndGame(EndCondition.Dead);
            return;
        }
        ballSpawner.Spawn();
        if (gameObject.transform.parent != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, Slime source)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Die();
        }

        //get bounced back from attack source
        Vector2 direction;
        if (source != null)
        {
            direction = transform.position - source.transform.position;
        }
        else
        {
            return;
        }

        Rigidbody2D rb;
        Rigidbody2D sourceRb;
        if (this.gameObject.CompareTag("Slime"))
        {
            rb = GetComponentInParent<Rigidbody2D>();
            sourceRb = source.GetComponent<Rigidbody2D>();
        }
        else
        {
            rb = GetComponent<Rigidbody2D>();
            sourceRb = source.GetComponentInParent<Rigidbody2D>();
        }
        rb.AddForce(direction.normalized * sourceRb.mass * 200f);
        flashEffect.Flash(Color.white);

        if (gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(damageSound);
        }
    }
}

public enum SlimeState
{
    Idle,
    Eating
}
