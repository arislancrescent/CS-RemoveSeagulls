using System;
using System.Collections.Generic;
using System.Threading;

using ICities;
using ColossalFramework;
using ColossalFramework.Math;
using ColossalFramework.UI;
using UnityEngine;

namespace RemoveSeagulls
{
    public class Remover : ThreadingExtensionBase
    {
        private Helper _helper;

        private SkylinesOverwatch.Data _data;

        private bool _initialized;
        private bool _terminated;

        public override void OnCreated(IThreading threading)
        {
            _helper = Helper.Instance;

            _initialized = false;
            _terminated = false;

            base.OnCreated(threading);
        }

        public override void OnBeforeSimulationTick()
        {
            try
            {
                if (!SkylinesOverwatch.Helper.Instance.GameLoaded)
                {
                    _initialized = false;
                    return;
                }
            }
            catch (Exception e)
            {
                _helper.Log("[ARIS] Skylines Overwatch not found. Unloading...");
                _terminated = true;
            }

            base.OnBeforeSimulationTick();
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (_terminated) return;

            try
            {
                if (!SkylinesOverwatch.Helper.Instance.GameLoaded) return;

                if (!_initialized)
                {
                    try
                    {
                        SkylinesOverwatch.Settings.Instance.Enable.AnimalMonitor  = true;
                        SkylinesOverwatch.Settings.Instance.Enable.Birds          = true;

                        _data = SkylinesOverwatch.Data.Instance;
                    }
                    catch (Exception e)
                    {
                        _helper.Log("[ARIS] Skylines Overwatch not found. Unloading...");
                        _terminated = true;
                    }

                    _initialized = true;

                    _helper.Log("Initialized");
                }
                else if (_data.Seagulls.Length > 0)
                {
                    CitizenManager instance = Singleton<CitizenManager>.instance;

                    foreach (ushort i in _data.Seagulls)
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

