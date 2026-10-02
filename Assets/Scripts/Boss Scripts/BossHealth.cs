using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

    public int health = 3;
    public float hitCooldown = 0.2f;
    public float winDelay = 1.5f;

    private Animator anim;
    private bool isDead = false;
    private bool canDamage = true;

    void Awake () {
		anim = GetComponent<Animator> ();
	}

    void OnTriggerEnter2D(Collider2D target)
    {
        if (isDead || !canDamage) return;

        if (target.CompareTag(MyTags.BULLET_TAG))
        {
            Destroy(target.gameObject);   
            health--;
            canDamage = false;
            StartCoroutine(WaitForDamage());

            if (health <= 0) Die();
        }
    }

    void Die()
    {
        isDead = true;
        GetComponent<BossScript>().DeactivateBossScript();
        anim.Play("BossDead");
        Invoke(nameof(ShowWin), winDelay);
    }
    void ShowWin()
    {
        GameManager.instance.WinGame();
    }

    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(hitCooldown);
        canDamage = true;
    }
}
