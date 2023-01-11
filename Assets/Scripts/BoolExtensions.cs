namespace SimpleSmeeborg
{
    public static class BoolExtensions
    {
        /// <summary>
        /// A readability extension method for standard bool-to-int conversions.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
