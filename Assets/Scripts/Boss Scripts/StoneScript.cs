using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour {

	void Start () {
		Invoke ("Deactivate", 4f);
	}

	void Deactivate() {
		gameObject.SetActive (false);
	}

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(MyTags.PLAYER_TAG))
        {
            PlayerCombat combat = target.GetComponent<PlayerCombat>();

            // Shield up: the rock is blocked, no damage
            if (combat != null && combat.isShielding)
            {
                gameObject.SetActive(false);
                return;
            }

            target.GetComponent<PlayerDamage>().DealDamage();
            gameObject.SetActive(false);
        }
    }

} // class
