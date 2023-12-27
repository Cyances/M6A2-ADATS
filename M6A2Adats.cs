using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using M6A2Adats;
using GHPC.Camera;
using GHPC.Equipment.Optics;
using GHPC.Player;
using GHPC.Vehicle;
using GHPC.Weapons;
using MelonLoader;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using GHPC;

[assembly: MelonInfo(typeof(M6A2_Adats), "M6A2 ADATS", "1.0.1", "Schweiz and Cyance")]
[assembly: MelonGame("Radian Simulations LLC", "GHPC")]


namespace M6A2Adats
{
    public class M6A2_Adats : MelonMod
    {

        GameObject[] vic_gos;
        GameObject gameManager;
        CameraManager cameraManager;
        PlayerInput playerManager;

        AmmoClipCodexScriptable clip_codex_M919;
        AmmoType.AmmoClip clip_M919;
        AmmoCodexScriptable ammo_codex_M919;
        AmmoType ammo_M919;

        AmmoClipCodexScriptable clip_codex_APEX;
        AmmoType.AmmoClip clip_APEX;
        AmmoCodexScriptable ammo_codex_APEX;
        AmmoType ammo_APEX;

        AmmoClipCodexScriptable clip_codex_ADATS;
        AmmoType.AmmoClip clip_ADATS;
        AmmoCodexScriptable ammo_codex_ADATS;
        AmmoType ammo_ADATS;

        AmmoType ammo_m791;
        AmmoType ammo_I_TOW;

        AmmoType ammo_m792;

        // https://snipplr.com/view/75285/clone-from-one-object-to-another-using-reflection
        public static void ShallowCopy(System.Object dest, System.Object src)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo[] destFields = dest.GetType().GetFields(flags);
            FieldInfo[] srcFields = src.GetType().GetFields(flags);

            foreach (FieldInfo srcField in srcFields)
            {
                FieldInfo destField = destFields.FirstOrDefault(field => field.Name == srcField.Name);

                if (destField != null && !destField.IsLiteral)
                {
                    if (srcField.FieldType == destField.FieldType)
                        destField.SetValue(dest, srcField.GetValue(src));
                }
            }
        }

        public static void EmptyRack(GHPC.Weapons.AmmoRack rack)
        {
            MethodInfo removeVis = typeof(GHPC.Weapons.AmmoRack).GetMethod("RemoveAmmoVisualFromSlot", BindingFlags.Instance | BindingFlags.NonPublic);

            PropertyInfo stored_clips = typeof(GHPC.Weapons.AmmoRack).GetProperty("StoredClips");
            stored_clips.SetValue(rack, new List<AmmoType.AmmoClip>());

            rack.SlotIndicesByAmmoType = new Dictionary<AmmoType, List<byte>>();

            foreach (Transform transform in rack.VisualSlots)
            {
                AmmoStoredVisual vis = transform.GetComponentInChildren<AmmoStoredVisual>();

                if (vis != null && vis.AmmoType != null)
                {
                    removeVis.Invoke(rack, new object[] { transform });
                }
            }
        }

        public override void OnInitializeMelon()
        {
        }

        public override async void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "LOADER_INITIAL" || sceneName == "MainMenu2_Scene" || sceneName == "t64_menu") return;

            vic_gos = GameObject.FindGameObjectsWithTag("Vehicle");

            while (vic_gos.Length == 0)
            {
                vic_gos = GameObject.FindGameObjectsWithTag("Vehicle");
                await Task.Delay(3000);
            }

            if (ammo_M919 == null)
            {
                foreach (AmmoCodexScriptable s in Resources.FindObjectsOfTypeAll(typeof(AmmoCodexScriptable)))
                {
                    if (s.AmmoType.Name == "25mm APDS-T M791") ammo_m791 = s.AmmoType;
                    if (s.AmmoType.Name == "25mm HEI-T M792") ammo_m792 = s.AmmoType;
                    if (s.AmmoType.Name == "BGM-71C I-TOW") ammo_I_TOW = s.AmmoType;
                }

                // M919  APFSDS-T

                ammo_M919 = new AmmoType();
                ShallowCopy(ammo_M919, ammo_m791);
                ammo_M919.Name = "M919 APFSDS-T";
                ammo_M919.Caliber = 25;
                ammo_M919.RhaPenetration = 112f;
                ammo_M919.MuzzleVelocity = 1390f;
                ammo_M919.Mass = 0.134f;

                ammo_codex_M919 = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_M919.AmmoType = ammo_M919;
                ammo_codex_M919.name = "ammo_M919";

                clip_M919 = new AmmoType.AmmoClip();
                clip_M919.Capacity = 300;
                clip_M919.Name = "M919 APFSDS-T";
                clip_M919.MinimalPattern = new AmmoCodexScriptable[1];
                clip_M919.MinimalPattern[0] = ammo_codex_M919;

                clip_codex_M919 = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_M919.name = "clip_M919";
                clip_codex_M919.ClipType = clip_M919;

                // M210  APFSDS-T

                ammo_APEX = new AmmoType();
                ShallowCopy(ammo_APEX, ammo_m792);
                ammo_APEX.Name = "APEX APHE-T";
                ammo_APEX.Caliber = 25;
                ammo_APEX.RhaPenetration = 15;
                ammo_APEX.MuzzleVelocity = 970f;
                ammo_APEX.Mass = 0.222f;
                ammo_APEX.TntEquivalentKg = 0.050f;
                ammo_APEX.SpallMultiplier = 1.25f;
                ammo_APEX.DetonateSpallCount = 30;
                ammo_APEX.MaxSpallRha = 10f;
                ammo_APEX.MinSpallRha = 2f;
                ammo_APEX.ImpactFuseTime = 0.4f;

                ammo_codex_APEX = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_APEX.AmmoType = ammo_APEX;
                ammo_codex_APEX.name = "ammo_APEX";

                clip_APEX = new AmmoType.AmmoClip();
                clip_APEX.Capacity = 1200;
                clip_APEX.Name = "APEX APHE-T";
                clip_APEX.MinimalPattern = new AmmoCodexScriptable[1];
                clip_APEX.MinimalPattern[0] = ammo_codex_APEX;

                clip_codex_APEX = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_APEX.name = "clip_APEX";
                clip_codex_APEX.ClipType = clip_APEX;

                // TOW-2A

                ammo_ADATS = new AmmoType();
                ShallowCopy(ammo_ADATS, ammo_I_TOW);
                ammo_ADATS.Name = "MIM-146 ADATS";
                ammo_ADATS.Caliber = 152;
                ammo_ADATS.RhaPenetration = 1000f;
                ammo_ADATS.MuzzleVelocity = 1020f;
                ammo_ADATS.Mass = 51f;
                ammo_ADATS.TntEquivalentKg = 12.5f;
                ammo_ADATS.Tandem = true;
                ammo_ADATS.SpallMultiplier = 1.5f;
                ammo_ADATS.TurnSpeed = 1.5f;
                ammo_ADATS.DetonateSpallCount = 100;
                ammo_ADATS.MaxSpallRha = 24;
                ammo_ADATS.MinSpallRha = 12;
                ammo_ADATS.MaximumRange = 10000;
                ammo_ADATS.ImpactFuseTime = 10; //max flight time is 10 secs

                ammo_codex_ADATS = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_ADATS.AmmoType = ammo_ADATS;
                ammo_codex_ADATS.name = "ammo_ADATS";

                clip_ADATS = new AmmoType.AmmoClip();
                clip_ADATS.Capacity = 4;
                clip_ADATS.Name = "MIM-146 ADATS";
                clip_ADATS.MinimalPattern = new AmmoCodexScriptable[1];
                clip_ADATS.MinimalPattern[0] = ammo_codex_ADATS;

                clip_codex_ADATS = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_ADATS.name = "clip_ADATS";
                clip_codex_ADATS.ClipType = clip_ADATS;
            }

            foreach (GameObject vic_go in vic_gos)
            {
                Vehicle vic = vic_go.GetComponent<Vehicle>();

                if (vic == null) continue;
                if (vic.FriendlyName != "M2 Bradley") continue;

                // Rename to M6A2 ADATS  

                string name = "M6A2 ADATS";

                FieldInfo friendlyName = typeof(GHPC.Unit).GetField("_friendlyName", BindingFlags.NonPublic | BindingFlags.Instance);
                friendlyName.SetValue(vic, name);

                FieldInfo uniqueName = typeof(GHPC.Unit).GetField("_uniqueName", BindingFlags.NonPublic | BindingFlags.Instance);
                uniqueName.SetValue(vic, name);

                WeaponsManager weaponsManager = vic.GetComponent<WeaponsManager>();
                WeaponSystemInfo mainGunInfo = weaponsManager.Weapons[0];
                WeaponSystem mainGun = mainGunInfo.Weapon;

                WeaponSystemInfo towGunInfo = weaponsManager.Weapons[1];
                WeaponSystem towGun = towGunInfo.Weapon;

                // LRF

                FieldInfo fixParallaxField = typeof(FireControlSystem).GetField("_fixParallaxForVectorMode", BindingFlags.Instance | BindingFlags.NonPublic);
                fixParallaxField.SetValue(mainGun.FCS, true);
                mainGun.FCS.MaxLaserRange = 10000;

                //M242 stats
                mainGun.SetCycleTime(0.0166f); //3600 RPM

                PropertyInfo feedRPM = typeof(AmmoFeed).GetProperty("TotalCycleTime");
                feedRPM.SetValue(mainGun.Feed, 0.0166f);

                //TOW stat
                towGun.TriggerHoldTime = 0.5f;
                towGun.MaxSpeedToFire = 999f;
                towGun.MaxSpeedToDeploy = 999f;
                vic.AimablePlatforms[2].ForcedStowSpeed = 999f;

                LoadoutManager loadoutManager = vic.GetComponent<LoadoutManager>();
                loadoutManager.LoadedAmmoTypes = new AmmoClipCodexScriptable[] { clip_codex_M919, clip_codex_APEX };

                GHPC.Weapons.AmmoRack towRack = towGun.Feed.ReadyRack;

                towRack.ClipTypes[0] = clip_ADATS;

                towRack.StoredClips[0] = clip_ADATS;
                towRack.StoredClips[1] = clip_ADATS;
                towRack.StoredClips[2] = clip_ADATS;
                towRack.StoredClips[3] = clip_ADATS;

                for (int i = 0; i <= 1; i++)
                {
                    GHPC.Weapons.AmmoRack rack = loadoutManager.RackLoadouts[i].Rack;
                    loadoutManager.RackLoadouts[i].OverrideInitialClips = new AmmoClipCodexScriptable[] { clip_codex_M919, clip_codex_APEX };
                    rack.ClipTypes = new AmmoType.AmmoClip[] { clip_M919, clip_APEX };
                }

                loadoutManager.SpawnCurrentLoadout();


     
                PropertyInfo roundInBreech = typeof(AmmoFeed).GetProperty("AmmoTypeInBreech");

                roundInBreech.SetValue(mainGun.Feed, null);
                roundInBreech.SetValue(towGun.Feed, null);

                MethodInfo refreshBreech = typeof(AmmoFeed).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic);
                refreshBreech.Invoke(mainGun.Feed, new object[] {});
                refreshBreech.Invoke(towGun.Feed, new object[] { });

                towRack.AddInvisibleClip(clip_ADATS);

                // update ballistics computer
                MethodInfo registerAllBallistics = typeof(LoadoutManager).GetMethod("RegisterAllBallistics", BindingFlags.Instance | BindingFlags.NonPublic);
                registerAllBallistics.Invoke(loadoutManager, new object[] {});

                // Better Thermals

                FireControlSystem FCS = mainGun.FCS;
                UsableOptic optic = null;
                if (FCS.MainOptic.slot.VisionType == NightVisionType.Thermal)
                {
                    optic = FCS.MainOptic;
                }
                else if (FCS.MainOptic.slot.LinkedNightSight != null && FCS.MainOptic.slot.LinkedNightSight.VisionType == NightVisionType.Thermal)
                {
                    optic = FCS.MainOptic.slot.LinkedNightSight.PairedOptic;
                }

                optic.slot.BaseBlur = 0;
            }
        }
    }
}