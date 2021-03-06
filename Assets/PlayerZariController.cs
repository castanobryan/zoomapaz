using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerZariController : MonoBehaviour {
	public GameObject game;
	public AudioClip Pointsound;
	public AudioClip Pointfail;
	public float maxSpeed = 10f;
	public float speed = 2f;
	public bool grounded;
	public float jumpPower = 6.5f;
	private Rigidbody2D rb2d;
	private AudioSource audioPlayer;
	private Animator anim;
	private bool jump;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		audioPlayer = GetComponent<AudioSource> ();
		game = GameObject.Find ("Points");
	}
		
	void Update () {
		anim.SetFloat ("Speed", Mathf.Abs(rb2d.velocity.x));
		anim.SetBool ("Grounded", grounded);

		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			jump = true;
		}
	}

	void FixedUpdate(){
		Vector3 fixedVelocity = rb2d.velocity;
		fixedVelocity.x *= 0.75f;

		if (grounded) {
			rb2d.velocity = fixedVelocity;
		}
		float h = Input.GetAxis("Horizontal");
		rb2d.AddForce (Vector2.right * speed * h);
		float limitedSpeed = Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed);
		rb2d.velocity = new Vector2 (limitedSpeed, rb2d.velocity.y);
		if (h > 0.1f) {
			transform.localScale = new Vector3 (1f, 1f, 1f);
		} 
		if (h < -0.1f) {
			transform.localScale = new Vector3 (-1f, 1f, 1f);
		}
		if (jump) {
			rb2d.AddForce (Vector2.up * jumpPower, ForceMode2D.Impulse);
			jump = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Point"){
			Destroy (other.gameObject);
			game.SendMessage ("IncreasePoints");
			audioPlayer.clip = Pointsound;
			audioPlayer.Play ();
		} 

		if (other.gameObject.tag == "Caida"){
			game.SendMessage("DecreasePoints");
			audioPlayer.clip = Pointfail;
			audioPlayer.Play ();
		}
	}

}
