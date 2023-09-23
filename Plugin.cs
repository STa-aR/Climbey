using BepInEx;
using Climbey;
using GorillaLocomotion.Climbing;
using System;
using UnityEngine;
using UnityEngine.UI;
using Utilla;

namespace Climbey
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void Update()
        {
            if (!inRoom)
            {
                foreach (VRRig Rig in GorillaParent.instance.vrrigs)
                {
                    if (!Rig.isMyPlayer)
                    {
                        GameObject.Destroy(Rig.gameObject.GetComponent<GorillaClimbable>());
                    }
                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            foreach (VRRig Rig in GorillaParent.instance.vrrigs)
            {
                if (!Rig.isMyPlayer)
                {
                    if (Rig.gameObject.GetComponent<GorillaClimbable>() == null)
                    {
                        Rig.gameObject.AddComponent<GorillaClimbable>();
                    }
                }
            }
            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            foreach (VRRig Rig in GorillaParent.instance.vrrigs)
            {
                GameObject.Destroy(Rig.gameObject.GetComponent<GorillaClimbable>());
            }

            inRoom = false;
        }
    }
}
