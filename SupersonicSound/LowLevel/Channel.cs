﻿using FMOD;
using SupersonicSound.Wrapper;
using System;
using System.Collections.Generic;

namespace SupersonicSound.LowLevel
{
    public struct Channel
        : IEquatable<Channel>, IChannelControl
    {
        public FMOD.Channel FmodChannel { get; }

        private CallbackHandler _callbackHandler;

        private bool _throwHandle;
        public bool SuppressInvalidHandle
        {
            get { return !_throwHandle; }
            set { _throwHandle = !value; }
        }

        private bool _throwStolen;
        public bool SuppressChannelStolen
        {
            get { return !_throwStolen; }
            set { _throwStolen = !value; }
        }

        private Channel(FMOD.Channel channel)
            : this()
        {
            FmodChannel = channel;
        }

        public static Channel FromFmod(FMOD.Channel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));
            return new Channel(channel);
        }

        private IReadOnlyList<RESULT> Suppressions()
        {
            return ErrorChecking.Suppress(_throwHandle, _throwStolen);
        }

        #region equality
        public bool Equals(Channel other)
        {
            return other.FmodChannel == FmodChannel;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Channel))
                return false;

            return Equals((Channel)obj);
        }

        public override int GetHashCode()
        {
            return (FmodChannel != null ? FmodChannel.GetHashCode() : 0);
        }
        #endregion

        #region Channel specific control functionality
        public float? Frequency
        {
            get
            {
                float freq;
                return FmodChannel.getFrequency(out freq).CheckBox(freq, Suppressions());
            }
            set
            {
                FmodChannel.setFrequency(value.Unbox()).Check(Suppressions());
            }
        }

        public int? Priority
        {
            get
            {
                int priority;
                return FmodChannel.getPriority(out priority).CheckBox(priority, Suppressions());
            }
            set
            {
                FmodChannel.setPriority(value.Unbox()).Check(Suppressions());
            }
        }

        public void SetPosition(uint position, TimeUnit unit)
        {
            FmodChannel.setPosition(position, (TIMEUNIT)unit).Check(Suppressions());
        }

        public uint? GetPosition(TimeUnit unit)
        {
            uint pos;
            return FmodChannel.getPosition(out pos, EquivalentEnum<TimeUnit, TIMEUNIT>.Cast(unit)).CheckBox(pos, Suppressions());
        }

        public ChannelGroup? ChannelGroup
        {
            get
            {
                FMOD.ChannelGroup group;
                FmodChannel.getChannelGroup(out group).Check(Suppressions());
                if (group == null)
                    return null;
                else
                    return LowLevel.ChannelGroup.FromFmod(group);
            }
            set
            {
                var group = value.Unbox();
                FmodChannel.setChannelGroup(group.FmodGroup).Check(Suppressions());
            }
        }

        public int? LoopCount
        {
            get
            {
                int count;
                return FmodChannel.getLoopCount(out count).CheckBox(count, Suppressions());
            }
            set
            {
                FmodChannel.setLoopCount(value.Unbox()).Check(Suppressions());
            }
        }

        public struct LoopPoints
        {
            public readonly uint Start;
            public readonly uint End;

            public LoopPoints(uint start, uint end)
            {
                Start = start;
                End = end;
            }
        }

        public void SetLoopPoints(uint start, TimeUnit startUnit, uint end, TimeUnit endUnit)
        {
            FmodChannel.setLoopPoints(start, EquivalentEnum<TimeUnit, TIMEUNIT>.Cast(startUnit), end, EquivalentEnum<TimeUnit, TIMEUNIT>.Cast(endUnit)).Check(Suppressions());
        }

        public LoopPoints? GetLoopPoints(TimeUnit startUnit, TimeUnit endUnit)
        {
            uint startv;
            uint endv;
            if (!FmodChannel.getLoopPoints(out startv, EquivalentEnum<TimeUnit, TIMEUNIT>.Cast(startUnit), out endv, EquivalentEnum<TimeUnit, TIMEUNIT>.Cast(endUnit)).Check(Suppressions()))
                return null;

            return new LoopPoints(startv, endv);
        }

        public bool Stop()
        {
            return FmodChannel.stop().Check(Suppressions());
        }

        public bool? Pause
        {
            get
            {
                bool value;
                return FmodChannel.getPaused(out value).CheckBox(value, Suppressions());
            }
            set
            {
                FmodChannel.setPaused(value.Unbox()).Check(Suppressions());
            }
        }

        public float? Volume
        {
            get
            {
                float value;
                return FmodChannel.getVolume(out value).CheckBox(value, Suppressions());
            }
            set
            {
                FmodChannel.setVolume(value.Unbox()).Check(Suppressions());
            }
        }

        public bool? VolumeRamp
        {
            get
            {
                bool value;
                return FmodChannel.getVolumeRamp(out value).CheckBox(value, Suppressions());
            }
            set
            {
                FmodChannel.setVolumeRamp(value.Unbox()).Check(Suppressions());
            }
        }

        public float? Audibility
        {
            get
            {
                float value;
                return FmodChannel.getAudibility(out value).CheckBox(value, Suppressions());
            }
        }

        public float? Pitch
        {
            get
            {
                float value;
                return FmodChannel.getPitch(out value).CheckBox(value, Suppressions());
            }
            set
            {
                FmodChannel.setPitch(value.Unbox()).Check(Suppressions());
            }
        }

        public bool? Mute
        {
            get
            {
                bool value;
                return FmodChannel.getMute(out value).CheckBox(value, Suppressions());
            }
            set
            {
                FmodChannel.setMute(value.Unbox()).Check(Suppressions());
            }
        }

        public float Pan
        {
            set
            {
                FmodChannel.setPan(value).Check(Suppressions());
            }
        }

        public bool? IsPlaying
        {
            get
            {
                bool value;
                return FmodChannel.isPlaying(out value).Check(Suppressions());
            }
        }

        public Mode? Mode
        {
            get
            {
                MODE value;
                var nMode = FmodChannel.getMode(out value).CheckBox(value, Suppressions());

                return nMode.HasValue ? EquivalentEnum<MODE, Mode>.Cast(nMode.Value) : (Mode?)null;
            }
            set
            {
                FmodChannel.setMode(EquivalentEnum<Mode, MODE>.Cast(value.Unbox())).Check(Suppressions());
            }
        }

        public float? LowPassGain
        {
            get
            {
                float value;
                return FmodChannel.getLowPassGain(out value).CheckBox(value, Suppressions());
            }
            set
            {
                FmodChannel.setLowPassGain(value.Unbox()).Check(Suppressions());
            }
        }
        #endregion

        #region Clock based functionality
        public DspClock GetDspClock()
        {
            FmodChannel.getDSPClock(out var clock, out var parent).Check(Suppressions());
            return new DspClock(clock, parent);
        }

        public ChannelDelay Delay
        {
            get
            {
                FmodChannel.getDelay(out var start, out var end, out var stop).Check(Suppressions());
                return new ChannelDelay(start, end, stop);
            }
            set
            {
                FmodChannel.setDelay(value.DspClockStart, value.DspClockEnd, value.StopChannels).Check(Suppressions());
            }
        }

        public void AddFadePoint(ulong dspclock, float volume)
        {
            FmodChannel.addFadePoint(dspclock, volume).Check(Suppressions());
        }

        public void SetFadePointRamp(ulong dspclock, float volume)
        {
            FmodChannel.setFadePointRamp(dspclock, volume).Check(Suppressions());
        }

        public void RemoveFadePoints(ulong dspClockStart, ulong dspClockEnd)
        {
            FmodChannel.removeFadePoints(dspClockStart, dspClockEnd).Check(Suppressions());
        }

        public uint GetFadePointsCount()
        {
            uint numpoints = 0;
            FmodChannel.getFadePoints(ref numpoints, null, null).Check(Suppressions());
            return numpoints;
        }

        public uint GetFadePoints(ulong[] pointDspClock, float[] pointVolume)
        {
            uint numpoints = 0;
            FmodChannel.getFadePoints(ref numpoints, pointDspClock, pointVolume).Check(Suppressions());
            return numpoints;
        }
        #endregion

        #region Information only functions
        public bool? IsVirtual
        {
            get
            {
                bool virt;
                return FmodChannel.isVirtual(out virt).CheckBox(virt, Suppressions());
            }
        }

        public Sound? CurrentSound
        {
            get
            {
                FMOD.Sound sound;
                FmodChannel.getCurrentSound(out sound).Check(Suppressions());
                if (sound == null)
                    return null;
                else
                    return Sound.FromFmod(sound);
            }
        }

        public int? Index
        {
            get
            {
                int index;
                return FmodChannel.getIndex(out index).CheckBox(index, Suppressions());
            }
        }
        #endregion

        #region Callback functions
        public void SetCallback(Action<ChannelControlCallbackType, IntPtr, IntPtr> callback)
        {
            if (_callbackHandler == null)
                _callbackHandler = new CallbackHandler(FmodChannel);
            _callbackHandler.SetCallback(callback);
        }

        public void RemoveCallback()
        {
            _callbackHandler.RemoveCallback();
        }
        #endregion
    }
}
