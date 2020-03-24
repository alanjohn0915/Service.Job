namespace System
{
    public static class Extension
    {
        public static int StringToInt32(this string str, int def = -1)
        {
            return int.TryParse(str, out var result) ? result : def;
        }
    }
}