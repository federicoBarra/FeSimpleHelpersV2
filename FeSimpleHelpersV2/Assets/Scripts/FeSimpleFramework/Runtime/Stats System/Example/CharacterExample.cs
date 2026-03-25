using UnityEngine;
using UnityEngine.InputSystem;

namespace FeSimpleHelpers.StatsSystem.Example
{
	public class CharacterExample : MonoBehaviour
	{
		public WeaponExample currentWeapon;
		public WeaponExample gun;
		public WeaponExample shotgun;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			currentWeapon = gun;

			SetWeapon(gun);
		}

		// Update is called once per frame
		void Update()
		{
			if (Keyboard.current.digit1Key.wasPressedThisFrame)
			{
				SetWeapon(gun);
			}

			if (Keyboard.current.digit2Key.wasPressedThisFrame)
			{
				SetWeapon(shotgun);
			}

			if (Keyboard.current.spaceKey.wasPressedThisFrame)
				currentWeapon.TryAttack();
		}

		void SetWeapon(WeaponExample newWeapon)
		{
			currentWeapon = newWeapon;
		}
	}
}