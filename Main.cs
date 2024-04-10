using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHPC.AI;
using GHPC.Camera;
using GHPC.Player;
using GHPC.State;
using M6A2Adats;
using MelonLoader;
using NWH.VehiclePhysics;
using UnityEngine;

[assembly: MelonInfo(typeof(AdatsMod), "Z M6A2", "1.1.0", "Cyance and Schweiz")]
[assembly: MelonGame("Radian Simulations LLC", "GHPC")]

namespace M6A2Adats
{
    public class AdatsMod : MelonMod
    {
        public static GameObject[] vic_gos;
        public static GameObject gameManager;
        public static CameraManager camManager;
        public static PlayerInput playerManager;

        public IEnumerator GetVics(GameState _)
        {
            vic_gos = GameObject.FindGameObjectsWithTag("Vehicle");

            yield break;
        }

        public override void OnInitializeMelon()
        {
            MelonPreferences_Category cfg = MelonPreferences.CreateCategory("ADATSConfig");
            M6A2_Adats.Config(cfg);
        }

        public override void OnSceneWasLoaded(int idx, string scene_name)
        {
            if (scene_name == "MainMenu2_Scene" || scene_name == "LOADER_MENU" || scene_name == "LOADER_INITIAL" || scene_name == "t64_menu" || scene_name == "MainMenu2-1_Scene") return;

            gameManager = GameObject.Find("_APP_GHPC_");
            camManager = gameManager.GetComponent<CameraManager>();
            playerManager = gameManager.GetComponent<PlayerInput>();

            StateController.RunOrDefer(GameState.GameReady, new GameStateEventHandler(GetVics), GameStatePriority.Medium);
            M6A2_Adats.Init();
            ProxyFuzeADATS.Init();
        }
    }
}
