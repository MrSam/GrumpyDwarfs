using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]

    public class UndeadAICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public float minPlayerRange = 20f;
		public float minAttackDistance = 4f;
		public float attackDamagepoints = 10f;
		public AudioClip attackSound;
		public AudioClip spottedSound;
		public float maxHeadingChange = 60;
		public float wanderSpeed =0.5f;
		public float chaseSpeed = 3f;

		private Transform target; // target to aim for


		//private Transform wayPoint;
		private Animator m_Animator;
	
		private bool isAttacking = false;
		private bool isChasing = false;
		private bool hasPlayedSpottedSound = false;

		private Vector3 destination;
		private EnemyHealth myHealth;


        // Use this for initialization
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
			m_Animator = GetComponent<Animator>();
			myHealth = GetComponent<EnemyHealth> ();

			target = GameObject.FindWithTag ("Player").GetComponent<Transform>();

			StartCoroutine(NewHeading());
		}


        // Update is called once per frame
        private void Update()
        {
			if (!GameManager.gm.gameRunning)
				return;

			if (myHealth.IsDead ()) {
				agent.speed = 0;
				return;
			}

			// just wander
			if (target == null) {
				WanderAround();
				return;
			}

			float distanceFromPlayer = Vector3.Distance(this.gameObject.transform.position, target.transform.position);		

			if (distanceFromPlayer < minPlayerRange) {
				isChasing = true;

				if(!hasPlayedSpottedSound) {
					hasPlayedSpottedSound = true;
					AudioSource.PlayClipAtPoint(spottedSound, transform.position);
				}

				//should i still go to the player ?
				if(distanceFromPlayer > 2f) {
					agent.speed = chaseSpeed;
					agent.SetDestination (target.position);
					character.Move (agent.desiredVelocity, false, false);
				} else {
					// or just stop
					agent.speed = 0f;
					character.Move (agent.desiredVelocity, false, false);
				}

				//make sure the healthbar is visible
				myHealth.ShowHealthBar();

				//and attack !
				if (distanceFromPlayer < minAttackDistance) {
					if(!isAttacking)
						StartCoroutine ("AttackPlayer");
				}

			} else {
				//wander around .. the destination is chosen by NewHeading()
				isChasing = false;
				WanderAround();
			}
        }

		public void WanderAround() {
			agent.speed = wanderSpeed;
			agent.SetDestination (destination);
			character.Move (agent.desiredVelocity, false, false);
		}

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

		public System.Collections.IEnumerator AttackPlayer () {
			isAttacking = true;

			m_Animator.SetBool("attack", true);
			yield return new WaitForSeconds(0.3f);
			bool goodAttack = target.GetComponent<PlayerHealth>().DecreasePlayerHealth(attackDamagepoints);

			AudioSource.PlayClipAtPoint (attackSound, transform.position);
			yield return new WaitForSeconds(0.5f);
			m_Animator.SetBool("attack", false);

			// if goodAttack was false, means that the target is dead.. stop trying!
			if (!goodAttack) {
				this.target = null;
			}

			//cooldown
			yield return new WaitForSeconds(UnityEngine.Random.Range (1,3));
			isAttacking = false;
		}

		System.Collections.IEnumerator NewHeading ()
		{
			while (true) {
				if(!isChasing)
					destination =  this.transform.position + new Vector3(UnityEngine.Random.Range (-maxHeadingChange, maxHeadingChange),0,UnityEngine.Random.Range (-maxHeadingChange, maxHeadingChange));

				yield return new WaitForSeconds(2);
			}
		}
    }
}
