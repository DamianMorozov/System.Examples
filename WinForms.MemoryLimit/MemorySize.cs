// ReSharper disable UnusedMember.Global

namespace WinForms.MemoryLimit
{
    public class Size
    {
        public ulong Bytes { get; set; }

        public ulong GetKiloBytes() => Bytes / 1024;

        public ulong GetMegaBytes() => Bytes / 1048576;
    }

    public class MemorySize
    {
        public Size Limit;
        public Size Physical;
        public Size Virtual;

        public MemorySize(ulong getLimitBytes, ulong getPhysicalBytes = 0, ulong getVirtualBytes = 0)
        {
            Limit = new Size { Bytes = getLimitBytes };
            Physical = new Size { Bytes = getPhysicalBytes };
            Virtual = new Size { Bytes = getVirtualBytes };
        }
    }
}
