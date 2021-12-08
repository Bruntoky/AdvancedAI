using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{


	public class Attack : ActionTask{
		private bool attack;//attack aniamtion bool
		public BoxCollider Sword;//reference to swords collider
		public Animator anim;//reference to the animator

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
			
			Sword.enabled = true;
			
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){

			attack = true;
			Sword.enabled = true;
			StartCoroutine(attackDelay());
			anim.SetBool("attack", attack);

		}

		//Called when the task is disabled.
		protected override void OnStop(){
			Sword.enabled = false;

		}

		//Called when the task is paused.
		protected override void OnPause(){
			
		}
		private IEnumerator attackDelay()//turns off the sword collider after the animation runs
		{

			yield return new WaitForSeconds(2.5f);
			Sword.enabled = false;
			attack = false;//stop the attack animation
			anim.SetBool("attack", attack);
			EndAction(true);
		}
	}
}