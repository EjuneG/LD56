using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Controls the behavior of the NPC slimes, they'll find the nearest fruit and eat, sometimes attack weaker slimes (including the player), grow, and run away from huge creatures
public class SlimeNPC : MonoBehaviour
{
    [SerializeField] private ObjectDetector detector;
    [SerializeField] private Slime slime;
    [SerializeField] private float movespeed = 1f;
    [SerializeField] public int Size = 1; //small = 1, medium = 2, big = 3
    public int Aggression = 1; // 0 - passive, 1 - balanced, 2 - aggressive
    private Vector3 moveDirection;
    private BigCreature bigCreatureTarget;
    Fruit nearestFruit = null;
    GameObject nearestBall = null;
    [field: SerializeField]public NPCState State { get; set; }
    float actionInterval = 3f;
    float actionTimer = 0f;
    float restTimer = 0f;
    float chaseTimer = 0f;

    const float AGG_CHASERANGE = 5f;
    const float BAL_CHASERANGE = 3f;

    void OnEnable()
    {
        GameEvent.OnBigCreatureSpawns += BeginEscape;
    }

    void OnDisable()
    {
        GameEvent.OnBigCreatureSpawns -= BeginEscape;
    }
    void FixedUpdate()
    {
        actionTimer -= Time.deltaTime;
        if (restTimer > 0f)
        {
            restTimer -= Time.deltaTime;
            return;
        }

        if (actionTimer <= 0f)
        {
            DecideAction();
            actionTimer = actionInterval;
        }

        switch (State)
        {
            case NPCState.Escape:
                Escape();
                break;
            case NPCState.Idle:
                Wander();
                break;
            case NPCState.Hunting:
                Hunt();
                break;
            case NPCState.GatherBall:
                if (detector.EnergyBallsInRange.Count == 0)
                {
                    DecideAction();
                }
                else if (nearestBall == null)
                {
                    restTimer = 1f;
                    LocateNearestBall();
                }
                else
                {
                    MoveTo(nearestBall.transform.position);
                }
                break;
            case NPCState.ChasePlayer:
                if (playerIsInRange())
                {
                    MoveTo(GameManager.Instance.Player.transform.position, 1.2f);
                    chaseTimer += Time.deltaTime;
                    if (Aggression == 1 && chaseTimer > 5f)
                    {
                        State = NPCState.Idle;
                        chaseTimer = 0f;
                    }else if (Aggression == 2 && chaseTimer > 10f)
                    {
                        State = NPCState.Idle;
                        chaseTimer = 0f;
                    }
                }
                break;
        }
    }

    public void DecideAction()
    {
        if(detector.FruitsInRange == null){
            return;
        }
        detector.UpdateObjectsInRange();
        LocateNearestFruit();
        LocateNearestBall();
        UpdateDirection();
        if(WantsToChasePlayer()){
            State = NPCState.ChasePlayer;
            }
        else if(bigCreatureTarget != null && Vector3.Distance(transform.position, bigCreatureTarget.transform.position) < 20f){
            State = NPCState.Escape;
        }else if (detector.EnergyBallsInRange.Count > 0)
        {
            State = NPCState.GatherBall;
        }
        else if (detector.FruitsInRange.Count > 0)
        {
            State = NPCState.Hunting;
        }
        else
        {
            State = NPCState.Idle;
        }
    }

    public void Wander()
    {
        MoveTo(moveDirection);
    }
    public void MoveTo(Vector3 destination, float speed = 1f)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movespeed * speed * Time.deltaTime);
    }

    public void InitializeAggression(){
        if(Size == 1){
            //aggression is either 0 or 1
            Aggression = Random.Range(0, 2);
        }else if(Size == 2){
            //aggression is either 1 or 2
            Aggression = Random.Range(1, 3);
        }else{
            Aggression = 2;
    }
    }

    public void BeginEscape(BigCreature bigCreature)
    {
        State = NPCState.Escape;
        bigCreatureTarget = bigCreature;
    }
    public void Escape()
    {
        if (bigCreatureTarget == null)
        {
            State = NPCState.Idle;
            return;
        }else if (Vector3.Distance(transform.position, bigCreatureTarget.transform.position) > 20f)
        {
            State = NPCState.Idle;
            return;
        }
        Vector3 direction = (bigCreatureTarget.transform.position - transform.position).normalized;
        transform.position -= direction * Time.deltaTime * movespeed * 2;
    }
    public void UpdateDirection()
    {
        //move to a location near current location
        moveDirection = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.x + Random.Range(-2f, 2f), 0);
    }

    private void LocateNearestFruit()
    {
        float minDistance = Mathf.Infinity;
        for (int i = detector.FruitsInRange.Count - 1; i >= 0; i--)
        {
            Fruit fruit = detector.FruitsInRange[i];
            if (fruit == null)
            {
                detector.FruitsInRange.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(transform.position, fruit.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestFruit = fruit;
            }
        }
    }

    private void LocateNearestBall()
    {
        float minDistance = Mathf.Infinity;
        for (int i = detector.EnergyBallsInRange.Count - 1; i >= 0; i--)
        {
            GameObject ball = detector.EnergyBallsInRange[i];
            if (ball == null)
            {
                detector.EnergyBallsInRange.RemoveAt(i);
                continue;
            }

            float distance = Vector3.Distance(transform.position, ball.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBall = ball;
            }
        }
    }


    public void Hunt()
    {
        if (slime.currentTarget != null)
        {
            Eat();
        }
        else if (nearestFruit != null)
        {
            MoveTo(nearestFruit.transform.position);
        }
    }

    public bool WantsToChasePlayer(){
        if(Aggression == 0){
            return false;
        }else{
            return playerIsInRange();
        }
    }

    public void Eat()
    {
        if (slime.currentTarget != null)
        {
            slime.Eat();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Slime otherSlime = other.gameObject.GetComponent<Slime>();
            otherSlime.TakeDamage(slime.Damage, slime);
        }
    }

    private bool playerIsInRange(){
        float distance = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
        if(Aggression == 0 || Aggression == 1){
            return distance < BAL_CHASERANGE;
        }else{
            return distance < AGG_CHASERANGE;
        }
    }
}

public enum NPCState
{
    Idle,
    Hunting,
    GatherBall,
    Escape,
    ChasePlayer
}
