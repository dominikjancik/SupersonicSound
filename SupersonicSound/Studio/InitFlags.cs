﻿using FMOD.Studio;
using System;

namespace SupersonicSound.Studio
{
    [Flags]
    [EquivalentEnum(typeof(INITFLAGS))]
    public enum InitFlags
        : uint
    {
        /// <summary>
        /// Initialize normally.
        /// </summary>
        Normal = INITFLAGS.NORMAL,

        /// <summary>
        /// Enable live update.
        /// </summary>
        LiveUpdate = INITFLAGS.LIVEUPDATE,

        /// <summary>
        /// Load banks even if they reference plugins that have not been loaded.
        /// </summary>
        AllowMissingPlugins = INITFLAGS.ALLOW_MISSING_PLUGINS,

        /// <summary>
        /// Disable asynchronous processing and perform all processing on the calling thread instead.
        /// </summary>
        SynchronousUpdate = INITFLAGS.SYNCHRONOUS_UPDATE,

        /// <summary>
        /// Defer timeline callbacks until the main update. See Studio::EventInstance::setCallback for more information.
        /// </summary>
        DeferredCallback = INITFLAGS.DEFERRED_CALLBACKS,

        /// <summary>
        /// No additional threads are created for bank and resource loading.  Loading is driven from Studio::System::update.  Mainly used in non-realtime situations.
        /// </summary>
        LoadFromUpdate = INITFLAGS.LOAD_FROM_UPDATE,
    }
}
