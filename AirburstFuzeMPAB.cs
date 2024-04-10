using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GHPC.Player;
using GHPC.Weapons;
using UnityEngine;
using HarmonyLib;
using GHPC;

//Attempt to add toggleable TD AB fuze instead of being always active but I get brain ache instead

namespace M6A2Adats
{
    public class AirburstSwitchMPAB : MonoBehaviour
    {
        public bool activated = false;
        private float cd = 0f;
        private WeaponSystem weapon = null;
        private PlayerInput player_manager;

        void Awake()
        {
            weapon = GetComponent<WeaponsManager>().Weapons[0].Weapon;
            player_manager = GameObject.Find("_APP_GHPC_").GetComponent<PlayerInput>();
        }

        void Update()
        {
            cd -= Time.deltaTime;

            if (player_manager.CurrentPlayerUnit.gameObject.GetInstanceID() != gameObject.GetInstanceID()) return;

            M6A2_Adats.clip_M920.Name = activated ? "M920 MPAB-T [TD Airburst]" : "M920 MPAB-T";

            if (Input.GetKey(KeyCode.Mouse2) && cd <= 0f && weapon.CurrentAmmoType == M6A2_Adats.ammo_M920)
            {
                cd = 0.2f;

                activated = !activated;
            }
        }
    }


    public class AirburstFuzeMPAB : MonoBehaviour
    {
        private GHPC.Weapons.LiveRound live_round;
        private static GameObject prox_fuse;
        private static HashSet<string> prox_ammos = new HashSet<string>();
        private bool detonated = false;


        public static void AddFuzeMPAB(AmmoType ammo_type)
        {
            if (prox_ammos.Contains(ammo_type.Name)) return;
            prox_ammos.Add(ammo_type.Name);
        }
        

        [HarmonyPatch(typeof(GHPC.Weapons.LiveRound), "Start")]
        public static class Airburst
        {
            private static void Postfix(GHPC.Weapons.LiveRound __instance)
            {
                {
                    if (__instance.Info.Name != "M920 MPAB-T") return;

                    FieldInfo rangedFuseTimeField = typeof(GHPC.Weapons.LiveRound).GetField("_rangedFuseCountdown", BindingFlags.Instance | BindingFlags.NonPublic);
                    FieldInfo rangedFuseTimeActiveField = typeof(GHPC.Weapons.LiveRound).GetField("_rangedFuseActive", BindingFlags.Instance | BindingFlags.NonPublic);
                    FieldInfo ballisticsComputerField = typeof(FireControlSystem).GetField("_bc", BindingFlags.Instance | BindingFlags.NonPublic);

                    FireControlSystem FCS = __instance.Shooter.WeaponsManager.Weapons[0].FCS;
                    BallisticComputerRepository bc = ballisticsComputerField.GetValue(FCS) as BallisticComputerRepository;

                    float range = FCS.CurrentRange;
                    float fallOff = bc.GetFallOfShot(M6A2_Adats.ammo_M920, range);
                    float extra_distance = range > 2000 ? 19f + 3.5f : 17f;

                    //funky math 
                    rangedFuseTimeField.SetValue(__instance, bc.GetFlightTime(M6A2_Adats.ammo_M920, range + range / M6A2_Adats.ammo_M920.MuzzleVelocity * 2 + (range + fallOff) / 2000f + extra_distance));
                    rangedFuseTimeActiveField.SetValue(__instance, true);
                }
            }
        }
    }
}
