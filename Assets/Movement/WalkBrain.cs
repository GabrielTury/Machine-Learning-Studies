using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class WalkBrain : MonoBehaviour
{
    public int DNALength = 1; //define o numero de cromossomos
    public float timeAlive;

    public float distance;
    private Vector3 startingPosition;

    public WalkDNA dna;

    private ThirdPersonCharacter character;
    private Vector3 move;
    private bool jump;
    bool alive = true;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "dead")
        {
            alive = false;
        }
    }

    public void Init()
    {
        //Inicializa o DNA com 1 int que pode ter 6 valores representados pelos numeros abaixo
        //0 forward
        //1 back
        //2 left
        //3 right
        //4 jump
        //5 crouch

        dna = new WalkDNA(DNALength, 6);
        character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive=true;
    }


    private void FixedUpdate()
    {

        //Le o DNA
        float h = 0; //movimento horizontal
        float v = 0; //movimento vertical (visto de cima)
        bool crouch = false;

        if (dna.GetGene(0) == 0) v = 1;
        else if (dna.GetGene(0) == 1) v = -1;
        else if (dna.GetGene(0) == 2) h = -1;
        else if (dna.GetGene(0) == 3) h = 1;
        else if (dna.GetGene(0) == 4) jump = true;
        else if (dna.GetGene(0) == 5) crouch = true;

        move = v * Vector3.forward + h * Vector3.right;
        character.Move(move, crouch, jump);//envia os dados lidos do gene para o codigo de movimento do ethan
        jump = false;

        if (alive)
        {
            timeAlive += Time.deltaTime;

            distance = Vector2.Distance(startingPosition , transform.position);
        }

    }
}
