using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class MazeBrain : MonoBehaviour
    {
        int dnaLength = 2;

        public float distanceWalked = 0;
        Vector3 startingPosition;

        public MazeDNA dna;
        public GameObject eyes;

        bool seeWall;
        bool alive = true;

        Rigidbody rb;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.gameObject.tag == "dead")
            {
                alive = false;
                distanceWalked = 0;
            }
        }

        public void Init()
        {
            //Inicializa o DNA e outras variaveis que forem necessarias

            dna = new MazeDNA(dnaLength, 360);

            distanceWalked = 0;
            startingPosition = this.transform.position;

            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {

            if (!alive) return;



            //Debug.DrawRay(eyes.transform.position, eyes.transform.forward * .5f, Color.red);
            seeWall = false;

            RaycastHit hit;
            if(Physics.SphereCast(eyes.transform.position,0.1f, eyes.transform.forward * 1, out hit,0.5f))
            {
                if(hit.collider.gameObject.tag == "wall")
                {
                    seeWall = true;
                }
            }

        }

        private void FixedUpdate()
        {

            rb.AddForce(Physics.gravity * 100 * Time.fixedDeltaTime, ForceMode.Acceleration);
            int move = dna.GetGene(0); //define se move ou n
            int turn = 0; //Define o quanto vira

            if (seeWall)
            {
                turn = dna.GetGene(1); //Vira somente se encontrar uma parede
            }

            this.transform.Translate(0, 0, move * 0.001f); //por ser entre 0 e 360, o bot tbm escolhe a velocidade
            this.transform.Rotate(0, turn, 0);

            if(alive)
            distanceWalked = Vector3.Distance(startingPosition, this.transform.position);
        }

    }
}
