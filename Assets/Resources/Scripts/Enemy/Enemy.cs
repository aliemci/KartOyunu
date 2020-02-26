using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public class Move
    {
        public void Attack()
        {
            Debug.Log("Attacked!");
        }

        public void Health()
        {
            Debug.Log("health increased!");
        }

        public void Mana()
        {
            Debug.Log("Mana Poison consumed!");
        }
    }
    
    public enum Moves { Attack, Health, Mana };

    public string Id, Description;
    public int Health, Mana, Shield;
    public Sprite CharacterSprite;
    public Moves[] moves;

    public Enemy()
    {
        Debug.Log("Enemey has created!");
    }

    public void moveFunction(Moves[] moves)
    {
        Move Enemy_Movement = new Move();

        foreach(Moves movement in moves)
        {
            if (movement == Moves.Attack)
                Enemy_Movement.Attack();
            else if (movement == Moves.Health)
                Enemy_Movement.Health();
            else if (movement == Moves.Mana)
                Enemy_Movement.Mana();
        }
        
    }
}
