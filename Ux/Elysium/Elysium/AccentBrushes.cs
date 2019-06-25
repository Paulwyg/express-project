using System.Diagnostics.Contracts;
using System.Windows.Media;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium
{
    [PublicAPI]
    public static class AccentBrushes
    {
        [PublicAPI]
        public static SolidColorBrush Blue
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _blue ?? (_blue = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0x01, 0x7B, 0xCD))));
            }
        }

        private static SolidColorBrush _blue;

        [PublicAPI]
        public static SolidColorBrush Brown
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _brown ?? (_brown = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xA0, 0x50, 0x00))));
            }
        }

        private static SolidColorBrush _brown;

        [PublicAPI]
        public static SolidColorBrush Green
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _green ?? (_green = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0x33))));
            }
        }

        private static SolidColorBrush _green;

        [PublicAPI]
        public static SolidColorBrush Lime
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _lime ?? (_lime = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0x8C, 0xBF, 0x26))));
            }
        }

        private static SolidColorBrush _lime;

        [PublicAPI]
        public static SolidColorBrush Magenta
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _magenta ?? (_magenta = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x97))));
            }
        }

        private static SolidColorBrush _magenta;

        [PublicAPI]
        public static SolidColorBrush Mango
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _mango ?? (_mango = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x96, 0x09))));
            }
        }

        private static SolidColorBrush _mango;

        [PublicAPI]
        public static SolidColorBrush Orange
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _orange ?? (_orange = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xCB, 0x52, 0x01))));
            }
        }

        private static SolidColorBrush _orange;

        [PublicAPI]
        public static SolidColorBrush Pink
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _pink ?? (_pink = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8))));
            }
        }

        private static SolidColorBrush _pink;

        [PublicAPI]
        public static SolidColorBrush Purple
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _purple ?? (_purple = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0x69, 0x22, 0x7B))));
            }
        }

        private static SolidColorBrush _purple;

        [PublicAPI]
        public static SolidColorBrush Red
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _red ?? (_red = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xE5, 0x14, 0x00))));
            }
        }

        private static SolidColorBrush _red;

        [PublicAPI]
        public static SolidColorBrush Rose
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _rose ?? (_rose = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xD8, 0x00, 0x73))));
            }
        }

        private static SolidColorBrush _rose;

        [PublicAPI]
        public static SolidColorBrush Sky
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _sky ?? (_sky = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2))));
            }
        }

        private static SolidColorBrush _sky;

        [PublicAPI]
        public static SolidColorBrush Viridian
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _viridian ?? (_viridian = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xAB, 0xA9))));
            }
        }

        private static SolidColorBrush _viridian;

        [PublicAPI]
        public static SolidColorBrush Violet
        {
            get
            {
                Contract.Ensures(Contract.Result<SolidColorBrush>() != null);
                return _violet ?? (_violet = FreezableExtensions.TryFreeze(new SolidColorBrush(Color.FromArgb(0xFF, 0xA2, 0x00, 0xFF))));
            }
        }

        private static SolidColorBrush _violet;
    }
}
