namespace assignment1.Libs
{
    public static class Utilities
    {
        public static bool ValidString(string _value) => !string.IsNullOrEmpty(_value) && !string.IsNullOrWhiteSpace(_value);
    }
}