using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Senses
{
    public class SensesBrain : MonoBehaviour
    {
        int DNALength = 2;

        public float timeAlive;
        public float timeWalking;

        public SensesDNA dna;
        public GameObject eyes;

        bool alive = true;
        bool seeGround = true;

        public GameObject ethanPrefab;
        GameObject ethan;

        private void OnDestroy()
        {
            Destroy(ethan);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "dead")
            {
                alive = false;
                timeAlive = 0;
                timeWalking = 0;
            }
        }

        public void Init()
        {
            //Inicializa o DNA
            //0 forward
            //1 turn left
            //2 turn right

            dna = new SensesDNA(DNALength, 3); //por ter o 2 valores no DNALength, tera 2 opcoes de acao (cada uma com 3 valores dentro) cada gene tera um acao
                                               //caso enxerge o chao e uma caso nao

            timeAlive = 0;
            alive = true;

            ethan = Instantiate(ethanPrefab, this.transform.position, this.transform.rotation);
            ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
        }

        private void Update()
        {
            if (!alive) return;

            //Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
            seeGround = false;

            RaycastHit hit;
            if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
            {
                if (hit.collider.gameObject.tag == "platform")
                {
                    seeGround = true;
                }
            }

            timeAlive = SensesPopulationManager.elapsed;

            //leitura do DNA
            float turn = 0;
            float move = 0;

            if (seeGround) //dependendo do sentido que ele esta detectando (visao no caso) vai selecionar um dos genes
            {
                //v eh relativo ao personagem ent sempre vai para frente
                if (dna.GetGene(0) == 0) { move = 1; timeWalking += Time.deltaTime; }
                else if (dna.GetGene(0) == 1) turn = -90;
                else if (dna.GetGene(0) == 2) turn = 90;
            }
            else
            {
                if (dna.GetGene(1) == 0) { move = 1; timeWalking += Time.deltaTime; }
                else if (dna.GetGene(1) == 1) turn = -90;
                else if (dna.GetGene(1) == 2) turn = 90;
            }

            this.transform.Translate(0, 0, move * 0.1f);
            this.transform.Rotate(0, turn, 0);

        }
    }
}
