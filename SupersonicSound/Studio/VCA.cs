﻿using SupersonicSound.LowLevel;
using SupersonicSound.Wrapper;
using System;

namespace SupersonicSound.Studio
{
    public struct VCA
        : IEquatable<VCA>, IHandle
    {
        public FMOD.Studio.VCA FmodVca { get; }

        private VCA(FMOD.Studio.VCA vca)
            : this()
        {
            FmodVca = vca;
        }

        public static VCA FromFmod(FMOD.Studio.VCA vca)
        {
            if (vca == null)
                throw new ArgumentNullException(nameof(vca));
            return new VCA(vca);
        }

        public bool IsValid()
        {
            return FmodVca.isValid();
        }

        #region equality
        public bool Equals(VCA other)
        {
            return other.FmodVca == FmodVca;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VCA))
                return false;

            return Equals((VCA)obj);
        }

        public override int GetHashCode()
        {
            return (FmodVca != null ? FmodVca.GetHashCode() : 0);
        }
        #endregion

        public Guid Id
        {
            get
            {
                Guid id;
                FmodVca.getID(out id).Check();
                return id;
            }
        }

        public string Path
        {
            get
            {
                string path;
                FmodVca.getPath(out path).Check();
                return path;
            }
        }

        public float Volume
        {
            get
            {
                float volume;
                float _;
                FmodVca.getVolume(out volume, out _).Check();
                return volume;
            }
            set
            {
                FmodVca.setVolume(value).Check();
            }
        }

        public float FinalVolume
        {
            get
            {
                float volume;
                float _;
                FmodVca.getVolume(out _, out volume).Check();
                return volume;
            }
        }
    }
}
