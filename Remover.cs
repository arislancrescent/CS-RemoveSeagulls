﻿using System;
using System.Collections.Generic;
using System.Threading;

using ICities;
using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using UnityEngine;

namespace RemoveSeagulls
{
    public class Remover : ThreadingExtensionBase
    {
        private Settings _settings;
        private Helper _helper;

        private bool _initialized;
        private bool _terminated;

        protected bool IsOverwatched()
        {
            ulong skylinesOverwatchID;

            #if DEBUG

            return true;

            #elif PTR

            skylinesOverwatchID = 422560684;

            #else

            skylinesOverwatchID = 421028969;

            #endif

            foreach (var plugin in PluginManager.instance.GetPluginsInfo())
            {
                if (plugin.publishedFileID.AsUInt64 == skylinesOverwatchID)
                    return true;
            }

            return false;
        }

        public override void OnCreated(IThreading threading)
        {
            _settings = Settings.Instance;
            _helper = Helper.Instance;

            _initialized = false;
            _terminated = false;

            base.OnCreated(threading);
        }

        public override void OnBeforeSimulationTick()
        {
            if (_terminated) return;

            if (!_helper.GameLoaded)
            {
                _initialized = false;
                return;
            }

            base.OnBeforeSimulationTick();
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_terminated) return;

            if (!_helper.GameLoaded) return;

            try
            {
                if (!_initialized)
                {
                    if (!IsOverwatched())
                    {
                        _helper.Log(_settings.Flair + " Skylines Overwatch not found. Terminating...");
                        _terminated = true;

                        return;
                    }

                    SkylinesOverwatch.Settings.Instance.Enable.AnimalMonitor  = true;
                    SkylinesOverwatch.Settings.Instance.Enable.Birds          = true;

                    _initialized = true;

                    _helper.Log("Initialized");
                }
                else if (SkylinesOverwatch.Data.Instance.Seagulls.Length > 0)
                {
                    CitizenManager instance = Singleton<CitizenManager>.instance;

                    foreach (ushort i in SkylinesOverwatch.Data.Instance.Seagulls)
                    {
                        CitizenInstance seagull = instance.m_instances.m_buffer[(int)i];

                        if ((seagull.m_flags & CitizenInstance.Flags.Created) == CitizenInstance.Flags.None)
                            continue;

                        instance.ReleaseCitizenInstance(i);
                    }
                }
            }
            catch (Exception e)
            {
                string error = "Failed to initialize\r\n";
                error += String.Format("Error: {0}\r\n", e.Message);
                error += "\r\n";
                error += "==== STACK TRACE ====\r\n";
                error += e.StackTrace;

                _helper.Log(error);

                _terminated = true;
            }

            base.OnUpdate(realTimeDelta, simulationTimeDelta);
        }

        public override void OnReleased ()
        {
            _initialized = false;
            _terminated = false;

            base.OnReleased();
        }
    }
}

