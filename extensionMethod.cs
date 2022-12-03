namespace extensionMethodNamespace
{
    internal static class MyExtensions
    {
        public static bool validId(this string str)
        {
            int len = str.Length;

            if (len != 11) return false;
            if (str[3] != '-' || str[7] != '-') return false;

            for (int i = 0; i < len; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9') || str[i] == '-') { }
                else return false;
            }

            return true;
        }

        public static bool validYear(this string syr)
        {
            int len = syr.Length;

            if (len != 4) return false;
            
            for (int i = 0; i < len; i++)
            {
                if ((syr[i] >= '0' && syr[i] <= '9')) { }
                else return false;
            }

            return true;
        }
    }
}