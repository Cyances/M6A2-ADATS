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
using NWH.VehiclePhysics;
using GHPC.Equipment;
using GHPC.State;
using GHPC.Utility;
using System.Collections;
using GHPC.AI;
using HarmonyLib;


namespace M6A2Adats
{
    public static class M6A2_Adats
    {
        public static AmmoClipCodexScriptable clip_codex_M919;
        public static AmmoType.AmmoClip clip_M919;
        public static AmmoCodexScriptable ammo_codex_M919;
        public static AmmoType ammo_M919;

        public static AmmoClipCodexScriptable clip_codex_M791m6;
        public static AmmoType.AmmoClip clip_M791m6;
        public static AmmoCodexScriptable ammo_codex_M791m6;
        public static AmmoType ammo_M791m6;

        public static AmmoClipCodexScriptable clip_codex_ADATS;
        public static AmmoType.AmmoClip clip_ADATS;
        public static AmmoCodexScriptable ammo_codex_ADATS;
        public static AmmoType ammo_ADATS;

        public static AmmoClipCodexScriptable clip_codex_APEX;
        public static AmmoType.AmmoClip clip_APEX;
        public static AmmoCodexScriptable ammo_codex_APEX;
        public static AmmoType ammo_APEX;

        public static AmmoClipCodexScriptable clip_codex_M920;
        public static AmmoType.AmmoClip clip_M920;
        public static AmmoCodexScriptable ammo_codex_M920;
        public static AmmoType ammo_M920;

        public static AmmoType ammo_m791;
        public static AmmoType ammo_m792;
        public static AmmoType ammo_I_TOW;

        static MelonPreferences_Entry<bool> useGau, useM919, useM920, adatsTandem, superOptics, betterDynamics, betterAI, compositeTurret, compositeHull, rotateAzimuth;
        static MelonPreferences_Entry<int> apCount, heCount;
        public static MelonPreferences_Entry<float> proxyDistance;

        public static void Config(MelonPreferences_Category cfg)
        {
            useGau = cfg.CreateEntry<bool>("GAU12", true);
            useGau.Description = "Replaces M242 (500 RPM) with GAU-12 (3600 RPM)";
            useM919 = cfg.CreateEntry<bool>("M919", false);
            useM919.Description = "Replaces M792 with M919 APFSDS";
            useM920 = cfg.CreateEntry<bool>("MPAB", false);
            useM920.Description = "Replaces APEX with MPAB (TD airburst)";
            apCount = cfg.CreateEntry<int>("APCount", 300);
            apCount.Description = "Round type count, give at least 1 per type (max of 1500)";
            heCount = cfg.CreateEntry<int>("HECount", 1200);
            adatsTandem = cfg.CreateEntry<bool>("ADATSTandem", false);
            adatsTandem.Description = "Better ERA defeat for ADATS";
            proxyDistance = cfg.CreateEntry<float>("ProxyDistance", 3);
            proxyDistance.Description = "Trigger distance of ADATS proximity fuze (in meters).";
            rotateAzimuth = cfg.CreateEntry<bool>("RotateAzimuth", false);
            rotateAzimuth.Description = "Horizontal reticle stabilization when leading";
            superOptics = cfg.CreateEntry<bool>("SuperOptics", false);
            superOptics.Description = "More zoom levels/clearer image for main and thermal sights";
            betterDynamics = cfg.CreateEntry<bool>("BetterDynamics", false);
            betterDynamics.Description = "Better engine/transmission/suspension/tracks";
            betterAI = cfg.CreateEntry<bool>("BetterAI", false);
            betterAI.Description = "Better AI spotting and gunnery";
            compositeHull = cfg.CreateEntry<bool>("CompositeHull", false);
            compositeHull.Description = "50% better protection with no weight penalty";
            compositeTurret = cfg.CreateEntry<bool>("CompositeTurret", false);
        }

        public static IEnumerator Convert(GameState _)
        {
            ////UniformArmor pieces
            ///Extra armor under testing
            foreach (GameObject armour in GameObject.FindGameObjectsWithTag("Penetrable"))
            {
                if (armour == null) continue;

                UniformArmor m2UA = armour.GetComponent<UniformArmor>();
                if (m2UA == null) continue;
                if (m2UA.Unit == null) continue;
                if (m2UA.Unit.FriendlyName == "M2 Bradley")
                {
                    if (compositeTurret.Value)
                    {
                        if (m2UA.Name == "turret face")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "turret side")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }
                        if (m2UA.Name == "turret rear")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "turret roof")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }
                        if (m2UA.Name == "turret bottom")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "turret inner frame")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }
                        if (m2UA.Name == "turret ring collar")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "gun mantlet")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "trunnion shield")
                        {
                            m2UA.PrimaryHeatRha = 12.7f;
                            m2UA.PrimarySabotRha = 12.7f;
                        }
                    }

                    /*if (m2UA.Name == "machine gun sleeve")
                    {
                        m2UA.PrimaryHeatRha = 38.1f;
                        m2UA.PrimarySabotRha = 38.1f;
                    }

                    if (m2UA.Name == "cannon sleeve")
                    {
                        m2UA.PrimaryHeatRha = 38.1f;
                        m2UA.PrimarySabotRha = 38.1f;
                    }

                    if (m2UA.Name == "ammunition access panel")
                    {
                        m2UA.PrimaryHeatRha = 38.1f;
                        m2UA.PrimarySabotRha = 38.1f;
                    }

                    if (m2UA.Name == "gunsight doghouse")
                    {
                        m2UA.PrimaryHeatRha = 25.4f;
                        m2UA.PrimarySabotRha = 25.4f;
                    }

                    if (m2UA.Name == "turret bustle")
                    {
                        m2UA.PrimaryHeatRha = 12.7f;
                        m2UA.PrimarySabotRha = 12.7f;
                    }

                    if (m2UA.Name == "commander's hatch")
                    {
                        m2UA.PrimaryHeatRha = 50.8f;
                        m2UA.PrimarySabotRha = 50.8f;
                    }

                    if (m2UA.Name == "driver's hatch")
                    {
                        m2UA.PrimaryHeatRha = 50.8f;
                        m2UA.PrimarySabotRha = 50.8f;
                    }

                    if (m2UA.Name == "loading hatch")
                    {
                        m2UA.PrimaryHeatRha = 25.4f;
                        m2UA.PrimarySabotRha = 25.4f;
                    }

                    if (m2UA.Name == "swim vane")
                    {
                        m2UA.PrimaryHeatRha = 12.7f;
                        m2UA.PrimarySabotRha = 12.7f;
                    }*/

                    if (compositeHull.Value)
                    {
                        if (m2UA.Name == "anti-mine plate")
                        {
                            m2UA.PrimaryHeatRha = 12.7f;
                            m2UA.PrimarySabotRha = 12.7f;
                        }

                        if (m2UA.Name == "hull front")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }
                        if (m2UA.Name == "hull side")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "hull side reinforcement")
                        {
                            m2UA.PrimaryHeatRha = 12.7f;
                            m2UA.PrimarySabotRha = 12.7f;
                        }

                        if (m2UA.Name == "hull floor")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "hull rear")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }

                        if (m2UA.Name == "hull roof")
                        {
                            m2UA.PrimaryHeatRha = 38.1f;
                            m2UA.PrimarySabotRha = 38.1f;
                        }
                    }

                    /*if (m2UA.Name == "gearbox cover")
                    {
                        m2UA.PrimaryHeatRha = 50.8f;
                        m2UA.PrimarySabotRha = 50.8f;
                    }
                    if (m2UA.Name == "firing port")
                    {
                        m2UA.PrimaryHeatRha = 38.1f;
                        m2UA.PrimarySabotRha = 38.1f;
                    }

                    if (m2UA.Name == "rear ramp")
                    {
                        m2UA.PrimaryHeatRha = 50.8f;
                        m2UA.PrimarySabotRha = 50.8f;
                    }
                    if (m2UA.Name == "rear ramp door")
                    {
                        m2UA.PrimaryHeatRha = 38.1f;
                        m2UA.PrimarySabotRha = 38.1f;
                    }

                    if (m2UA.Name == "rear ramp armor plate")
                    {
                        m2UA.PrimaryHeatRha = 12.7f;
                        m2UA.PrimarySabotRha = 12.7f;
                    }
                    if (m2UA.Name == "exhaust shield")
                    {
                        m2UA.PrimaryHeatRha = 12.7f;
                        m2UA.PrimarySabotRha = 12.7f;
                    }

                    if (m2UA.Name == "engine vent")
                    {
                        m2UA.PrimaryHeatRha = 12.7f;
                        m2UA.PrimarySabotRha = 12.7f;
                    }

                    if (m2UA.Name == "firewall")
                    {
                        m2UA.PrimaryHeatRha = 12.7f;
                        m2UA.PrimarySabotRha = 12.7f;
                    }*/
                }
            }

            MelonLogger.Msg("Composite armor loaded");

            foreach (GameObject vic_go in AdatsMod.vic_gos)
            {
                Vehicle vic = vic_go.GetComponent<Vehicle>();

                if (vic == null) continue;
                if (vic.FriendlyName != "M2 Bradley") continue;
                if (vic.GetComponent<Util.AlreadyConvertedADATS>() != null) continue;

                vic._friendlyName = useGau.Value ? "M6A2 ADATS" : "M6A1 ADATS";

                vic.gameObject.AddComponent<Util.AlreadyConvertedADATS>();

                vic_go.AddComponent<ProxySwitchADATS>();

                WeaponsManager weaponsManager = vic.GetComponent<WeaponsManager>();
                WeaponSystemInfo mainGunInfo = weaponsManager.Weapons[0];
                WeaponSystem mainGun = mainGunInfo.Weapon;

                WeaponSystemInfo towGunInfo = weaponsManager.Weapons[1];
                WeaponSystem towGun = towGunInfo.Weapon;

                // LRF

                FieldInfo fixParallaxField = typeof(FireControlSystem).GetField("_fixParallaxForVectorMode", BindingFlags.Instance | BindingFlags.NonPublic);
                fixParallaxField.SetValue(mainGun.FCS, true);
                mainGun.FCS.MaxLaserRange = 6000;


                //USSR Vehicles/T80B/T80B_rig/HULL/TURRET/gun/---MAIN GUN SCRIPTS---/2A46-2/1G42 gunner's sight/
                //US Vehicles/M2 Bradley/FCS and sights
                mainGun.FCS.SuperleadWeapon = true;
                mainGun.FCS.SuperelevateWeapon = true;
                mainGun.FCS.RegisteredRangeLimits = new Vector2(200, 4000);
                mainGun.FCS.RecordTraverseRateBuffer = true;
                mainGun.FCS.TraverseBufferSeconds = 0.5f;


                if (useGau.Value) mainGunInfo.Name = "25mm Cannon GAU-12/U Equalizer";
                float gunRPM = useGau.Value ? 0.0166f : 0.12f;
                mainGun.SetCycleTime(gunRPM); //3600 vs 500 RPM
                mainGun.BaseDeviationAngle = 0.045f;
                mainGun.Impulse = 2000;
                mainGun.RecoilBlurMultiplier = 0.5f;

                mainGun.Feed._totalCycleTime = useGau.Value ? 0.0166f : 0.12f;//3600 vs 500 RPM

                towGunInfo.Name = "ADATS Launcher";
                towGun.TriggerHoldTime = 0.5f;
                towGun.MaxSpeedToFire = 999f;
                towGun.MaxSpeedToDeploy = 999f;
                towGun.RecoilBlurMultiplier = 0.2f;
                towGun.FireWhileGuidingMissile = true;
                vic.AimablePlatforms[2].ForcedStowSpeed = 999f;


                GHPC.Weapons.AmmoRack towRack = towGun.Feed.ReadyRack;

                towRack.ClipTypes[0] = clip_ADATS;

                towRack.StoredClips[0] = clip_ADATS;
                towRack.StoredClips[1] = clip_ADATS;
                towRack.StoredClips[2] = clip_ADATS;


                LoadoutManager loadoutManager = vic.GetComponent<LoadoutManager>();

                loadoutManager.LoadedAmmoTypes = new AmmoClipCodexScriptable[] { useM919.Value ? clip_codex_M919 : clip_codex_M791m6, useM920.Value ? clip_codex_M920 : clip_codex_APEX };

                for (int i = 0; i <= 1; i++)
                {
                    GHPC.Weapons.AmmoRack rack = loadoutManager.RackLoadouts[i].Rack;
                    loadoutManager.RackLoadouts[i].OverrideInitialClips = new AmmoClipCodexScriptable[] { useM919.Value ? clip_codex_M919 : clip_codex_M791m6, useM920.Value ? clip_codex_M920 : clip_codex_APEX };
                    rack.ClipTypes = new AmmoType.AmmoClip[] { useM919.Value ? clip_M919 : clip_M791m6, useM920.Value ? clip_M920 : clip_APEX };
                    Util.EmptyRack(rack);
                }

                loadoutManager.SpawnCurrentLoadout();

                PropertyInfo roundInBreech = typeof(AmmoFeed).GetProperty("AmmoTypeInBreech");

                roundInBreech.SetValue(mainGun.Feed, null);
                roundInBreech.SetValue(towGun.Feed, null);

                MethodInfo refreshBreech = typeof(AmmoFeed).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic);
                refreshBreech.Invoke(mainGun.Feed, new object[] { });
                refreshBreech.Invoke(towGun.Feed, new object[] { });

                towRack.AddInvisibleClip(clip_ADATS);

                // update ballistics computer
                MethodInfo registerAllBallistics = typeof(LoadoutManager).GetMethod("RegisterAllBallistics", BindingFlags.Instance | BindingFlags.NonPublic);
                registerAllBallistics.Invoke(loadoutManager, new object[] { });

                // Better Thermals
                var gpsOptic = vic_go.transform.Find("M2BRADLEY_rig/HULL/Turret/GPS Optic/").gameObject.transform;
                var flirOptic = vic_go.transform.Find("M2BRADLEY_rig/HULL/Turret/FLIR/").gameObject.transform;

                UsableOptic horizontalGps = gpsOptic.GetComponent<UsableOptic>();
                UsableOptic horizontalFlir = flirOptic.GetComponent<UsableOptic>();

                CameraSlot daysightPlus = gpsOptic.GetComponent<CameraSlot>();
                CameraSlot flirPlus = flirOptic.GetComponent<CameraSlot>();



                if (rotateAzimuth.Value)
                {
                    horizontalGps.RotateAzimuth = true;
                    horizontalFlir.RotateAzimuth = true;
                }

                if (superOptics.Value)
                {
                    daysightPlus.DefaultFov = 16.5f;//8
                    daysightPlus.OtherFovs = new float[] { 8f, 5.5f, 4f, 2.5f, 1.25f, 0.5f };//2.5
                    daysightPlus.BaseBlur = 0;
                    daysightPlus.VibrationBlurScale = 0;

                    flirPlus.DefaultFov = 16.5f;//8
                    flirPlus.OtherFovs = new float[] { 8f, 5.5f, 4f, 2.5f, 1.25f, 0.5f };//2.5
                    flirPlus.BaseBlur = 0;
                    flirPlus.VibrationBlurScale = 0;
                    GameObject.Destroy(flirOptic.transform.Find("Canvas Scanlines").gameObject);

                    MelonLogger.Msg("Super Optics Loaded");
                }

                //Vehicle dynamics under testing
                VehicleController m6a2Vc = vic_go.GetComponent<VehicleController>();
                NwhChassis m6a2Chassis = vic_go.GetComponent<NwhChassis>();
                UnitAI m6a2Ai = vic.GetComponentInChildren<UnitAI>();
                DriverAIController m6a2dAic = vic.GetComponent<DriverAIController>();

                if (betterDynamics.Value)
                {
                    m6a2dAic.maxSpeed = 32;//20

                    m6a2Vc.engine.maxPower = 1200f;//530;
                    m6a2Vc.engine.maxRPM = 4500f;//4000 ;
                    m6a2Vc.engine.maxRpmChange = 3000f;//2000;

                    m6a2Vc.brakes.maxTorque = 55590;//49590

                    m6a2Chassis._maxForwardSpeed = 32f;//16.4
                    m6a2Chassis._maxReverseSpeed = 16f;//4.47


                    List<float> fwGears = new List<float>();
                    fwGears.Add(6.28f);
                    fwGears.Add(4.81f);
                    fwGears.Add(2.98f);
                    fwGears.Add(1.76f);
                    fwGears.Add(1.36f);
                    fwGears.Add(1.16f);

                    List<float> rvGears = new List<float>();
                    rvGears.Add(-2.76f);
                    //rvGears.Add(-2.98f);
                    rvGears.Add(-8.28f);

                    List<float> Gears = new List<float>();
                    Gears.Add(-2.76f);
                    //Gears.Add(-2.98f);
                    Gears.Add(-8.28f);
                    Gears.Add(0f);
                    Gears.Add(6.28f);
                    Gears.Add(4.81f);
                    Gears.Add(2.98f);
                    Gears.Add(1.76f);
                    Gears.Add(1.36f);
                    Gears.Add(1.16f);

                    m6a2Vc.transmission.forwardGears = fwGears;//5 2.4 1.9 1.6 1.4 1.2
                    m6a2Vc.transmission.gearMultiplier = 9.918f;//9.918
                    m6a2Vc.transmission.gears = Gears;//
                    m6a2Vc.transmission.reverseGears = rvGears;//-8
                    m6a2Vc.transmission.shiftDuration = 0.1f;//.309
                    m6a2Vc.transmission.shiftDurationRandomness = 0f;//.2
                    m6a2Vc.transmission.shiftPointRandomness = 0.05f;//.05


                    for (int i = 0; i < 12; i++)
                    {
                        //m3a2Vc.wheels[i].wheelController.damper.force = 2.3036f;//2.3036
                        m6a2Vc.wheels[i].wheelController.damper.maxForce = 6500;//6500
                        m6a2Vc.wheels[i].wheelController.damper.unitBumpForce = 6500;//6500
                        m6a2Vc.wheels[i].wheelController.damper.unitReboundForce = 9000;//9000

                        //m3a2Vc.wheels[i].wheelController.spring.bottomOutForce = 0f;//0
                        m6a2Vc.wheels[i].wheelController.spring.force = 24079.51f;//24079.51
                        m6a2Vc.wheels[i].wheelController.spring.length = 0.32f;//0.2809
                        m6a2Vc.wheels[i].wheelController.spring.maxForce = 100000;//100000
                        m6a2Vc.wheels[i].wheelController.spring.maxLength = 0.58f;//0.48

                        m6a2Vc.wheels[i].wheelController.fFriction.forceCoefficient = 1.25f;//1.2
                        m6a2Vc.wheels[i].wheelController.fFriction.slipCoefficient = 1f;//1

                        m6a2Vc.wheels[i].wheelController.sFriction.forceCoefficient = 0.85f;//0.8
                        m6a2Vc.wheels[i].wheelController.sFriction.slipCoefficient = 1f;//1 
                    }

                    vic.AimablePlatforms[0].SpeedPowered = 80;//60
                    vic.AimablePlatforms[0].SpeedUnpowered = 20;//5

                    MelonLogger.Msg("Better vehicle dynamics loaded");
                }


                if (betterAI.Value)
                {
                    m6a2Ai.SpotTimeMaxDistance = 3500;
                    m6a2Ai.TargetSensor._spotTimeMax = 3;
                    m6a2Ai.TargetSensor._spotTimeMaxDistance = 500;
                    m6a2Ai.TargetSensor._spotTimeMaxVelocity = 7f;
                    m6a2Ai.TargetSensor._spotTimeMin = 1;
                    m6a2Ai.TargetSensor._spotTimeMinDistance = 50;
                    m6a2Ai.TargetSensor._targetCooldownTime = 1.5f;

                    m6a2Ai.CommanderAI._identifyTargetDurationRange = new Vector2(1.5f, 2.5f);
                    m6a2Ai.CommanderAI._sweepCommsCheckDuration = 4;


                    m6a2Ai.combatSpeedLimit = 25;
                    m6a2Ai.firingSpeedLimit = 20;

                    //m2Ai.AccuracyModifiers.Angle._radius = 2.4f;
                    m6a2Ai.AccuracyModifiers.Angle.MaxDistance = 1500;
                    m6a2Ai.AccuracyModifiers.Angle.MaxRadius = 5f;
                    m6a2Ai.AccuracyModifiers.Angle.MinRadius = 2f;
                    m6a2Ai.AccuracyModifiers.Angle.IncreaseAccuracyPerShot = false;

                    MelonLogger.Msg("Better AI loaded");
                }

                /*
                ////ERA detection for BUSK designation
                foreach (GameObject armor_go in GameObject.FindGameObjectsWithTag("Penetrable"))
                {
                    if (!armor_go.transform.parent.GetComponent<LateFollow>()) continue;

                    string name = armor_go.GetComponent<LateFollow>().ParentUnit.UniqueName;

                    if (name != "M2BRADLEY")
                    {
                        if (armor_go.name == "HULL")
                        {
                            if (armor_go.transform.Find("Hull Front Alu 5083/M2 Hull ERA Array(Clone)"))
                            {
                                vic._friendlyName += " BUSK";
                            }
                        }
                    }
                }*/
            }

            yield break;
        }

        public static void Init()
        {
            if (ammo_M919 == null)
            {
                foreach (AmmoCodexScriptable s in Resources.FindObjectsOfTypeAll(typeof(AmmoCodexScriptable)))
                {
                    if (s.AmmoType.Name == "25mm APDS-T M791") ammo_m791 = s.AmmoType;
                    if (s.AmmoType.Name == "25mm HEI-T M792") ammo_m792 = s.AmmoType;
                    if (s.AmmoType.Name == "BGM-71C I-TOW") ammo_I_TOW = s.AmmoType;
                }

                var era_optimizations_adats = new List<AmmoType.ArmorOptimization>() { };

                string[] era_names = new string[] {
                    "kontakt-1 armour",
                    "kontakt-5 armour",
                    "ARAT-1 Armor Codex",
                    "BRAT-M3 Armor Codex",
                    "BRAT-M5 Armor Codex",
                };

                foreach (ArmorCodexScriptable s in Resources.FindObjectsOfTypeAll<ArmorCodexScriptable>())
                {
                    if (era_names.Contains(s.name))
                    {
                        AmmoType.ArmorOptimization optimization_adats = new AmmoType.ArmorOptimization();
                        optimization_adats.Armor = s;
                        optimization_adats.RhaRatio = 0.2f;
                        era_optimizations_adats.Add(optimization_adats);
                    }

                    if (era_optimizations_adats.Count == era_names.Length) break;
                }

                int apCapacity = apCount.Value;
                int heCapacity = heCount.Value;
                MelonLogger.Msg("Total AP/HE Count: " + (apCapacity + heCapacity));

                if ((apCapacity + heCapacity) > 1500)
                {
                    apCapacity = 300;
                    heCapacity = 1200;
                    MelonLogger.Msg("Invalid total AP/HE amount, defaulting to 300 AP/1200 HE");
                }

                // M791 APFSDS-T

                ammo_M791m6 = new AmmoType();
                Util.ShallowCopy(ammo_M791m6, ammo_m791);
                ammo_M791m6.Name = "M791 APDS-T";

                ammo_codex_M791m6 = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_M791m6.AmmoType = ammo_M791m6;
                ammo_codex_M791m6.name = "ammo_M791m6";

                clip_M791m6 = new AmmoType.AmmoClip();
                clip_M791m6.Capacity = apCapacity;
                clip_M791m6.Name = "M791 APDS-T";
                clip_M791m6.MinimalPattern = new AmmoCodexScriptable[1];
                clip_M791m6.MinimalPattern[0] = ammo_codex_M791m6;

                clip_codex_M791m6 = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_M791m6.name = "clip_M791m6";
                clip_codex_M791m6.ClipType = clip_M791m6;

                // M919 APFSDS-T

                ammo_M919 = new AmmoType();
                Util.ShallowCopy(ammo_M919, ammo_m791);
                ammo_M919.Name = "M919 APFSDS-T";
                ammo_M919.Caliber = 25;
                ammo_M919.RhaPenetration = 102f;
                ammo_M919.MuzzleVelocity = 1390f;
                ammo_M919.Mass = 0.134f;
                ammo_M919.CertainRicochetAngle = 5;
                /*ammo_M919.SpallMultiplier = 1.5f;
                ammo_M919.MaxSpallRha = 16;
                ammo_M919.MinSpallRha = 4;*/

                ammo_codex_M919 = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_M919.AmmoType = ammo_M919;
                ammo_codex_M919.name = "ammo_M919";

                clip_M919 = new AmmoType.AmmoClip();
                clip_M919.Capacity = apCapacity;
                clip_M919.Name = "M919 APFSDS-T";
                clip_M919.MinimalPattern = new AmmoCodexScriptable[1];
                clip_M919.MinimalPattern[0] = ammo_codex_M919;

                clip_codex_M919 = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_M919.name = "clip_M919";
                clip_codex_M919.ClipType = clip_M919;

                // ADATS
                ammo_ADATS = new AmmoType();
                Util.ShallowCopy(ammo_ADATS, ammo_I_TOW);
                ammo_ADATS.Name = "MIM-146 ADATS";
                ammo_ADATS.Caliber = 152;
                ammo_ADATS.RhaPenetration = 1000f;
                ammo_ADATS.MuzzleVelocity = 510f;
                ammo_ADATS.Mass = 51f;
                ammo_ADATS.TntEquivalentKg = 12.5f;
                ammo_ADATS.Tandem = true;
                ammo_ADATS.SpallMultiplier = 1.5f;
                ammo_ADATS.TurnSpeed = 2.5f;
                ammo_ADATS.DetonateSpallCount = 300;
                ammo_ADATS.MaxSpallRha = 50;
                ammo_ADATS.MinSpallRha = 3;
                ammo_ADATS.MaximumRange = 10000;
                ammo_ADATS.ImpactFuseTime = 20; //max flight time is 20 secs
                ammo_ADATS.CertainRicochetAngle = 5;
                if (adatsTandem.Value) ammo_ADATS.ArmorOptimizations = era_optimizations_adats.ToArray<AmmoType.ArmorOptimization>();
                ProxyFuzeADATS.AddFuzeADATS(ammo_ADATS);

                ammo_codex_ADATS = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_ADATS.AmmoType = ammo_ADATS;
                ammo_codex_ADATS.name = "ammo_ADATS";

                clip_ADATS = new AmmoType.AmmoClip();
                clip_ADATS.Capacity = 4;
                clip_ADATS.Name = "MIM-146 ADATS";
                clip_ADATS.MinimalPattern = new AmmoCodexScriptable[1];
                clip_ADATS.MinimalPattern[0] = ammo_codex_ADATS;

                clip_codex_ADATS = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_ADATS.name = "clip_ADAT";
                clip_codex_ADATS.ClipType = clip_ADATS;

                // APEX APHE-T
                ammo_APEX = new AmmoType();
                Util.ShallowCopy(ammo_APEX, ammo_m792);
                ammo_APEX.Name = "APEX APHE-T";
                ammo_APEX.Coeff = 0.1f;
                ammo_APEX.Caliber = 25;
                ammo_APEX.RhaPenetration = 35f;
                ammo_APEX.MuzzleVelocity = 1270f;
                ammo_APEX.Mass = 0.222f;
                ammo_APEX.TntEquivalentKg = 0.050f;
                ammo_APEX.SpallMultiplier = 1.25f;
                ammo_APEX.DetonateSpallCount = 30;
                ammo_APEX.MaxSpallRha = 16f;
                ammo_APEX.MinSpallRha = 2f;
                ammo_APEX.CertainRicochetAngle = 5;

                ammo_codex_APEX = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_APEX.AmmoType = ammo_APEX;
                ammo_codex_APEX.name = "ammo_APEX";

                clip_APEX = new AmmoType.AmmoClip();
                clip_APEX.Capacity = heCapacity;
                clip_APEX.Name = "APEX APHE-T";
                clip_APEX.MinimalPattern = new AmmoCodexScriptable[1];
                clip_APEX.MinimalPattern[0] = ammo_codex_APEX;

                clip_codex_APEX = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_APEX.name = "clip_APEX";
                clip_codex_APEX.ClipType = clip_APEX;

                // M920 MPAB-T
                ammo_M920 = new AmmoType();
                Util.ShallowCopy(ammo_M920, ammo_m792);
                ammo_M920.Name = "M920 MPAB-T";
                ammo_M920.Coeff = 0.1f;
                ammo_M920.Caliber = 25;
                ammo_M920.RhaPenetration = 15f;
                ammo_M920.MuzzleVelocity = 1270f;
                ammo_M920.Mass = 0.222f;
                ammo_M920.TntEquivalentKg = 0.050f;
                ammo_M920.SpallMultiplier = 2f;
                ammo_M920.DetonateSpallCount = 60;
                ammo_M920.MaxSpallRha = 32f;
                ammo_M920.MinSpallRha = 2f;
                ammo_M920.CertainRicochetAngle = 5;

                ammo_codex_M920 = ScriptableObject.CreateInstance<AmmoCodexScriptable>();
                ammo_codex_M920.AmmoType = ammo_M920;
                ammo_codex_M920.name = "ammo_M920";

                clip_M920 = new AmmoType.AmmoClip();
                clip_M920.Capacity = heCapacity;
                clip_M920.Name = "M920 MPAB-T";
                clip_M920.MinimalPattern = new AmmoCodexScriptable[1];
                clip_M920.MinimalPattern[0] = ammo_codex_M920;

                clip_codex_M920 = ScriptableObject.CreateInstance<AmmoClipCodexScriptable>();
                clip_codex_M920.name = "clip_M920";
                clip_codex_M920.ClipType = clip_M920;
            }
            StateController.RunOrDefer(GameState.GameReady, new GameStateEventHandler(Convert), GameStatePriority.Lowest);
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