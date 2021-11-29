using System.IO;

namespace Reporter.Extensions
{
    public static class Extensions
    {
        public static string MergePath(this string self, string endPath) => Path.Combine(self, endPath);
        public static string MergePath(this string self, string middlePath, string endPath) => Path.Combine(self, middlePath, endPath);
    }
}