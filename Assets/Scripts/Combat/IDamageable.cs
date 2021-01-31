using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	void GotHit(IDamageDealer damageDealer, int damage);
}
