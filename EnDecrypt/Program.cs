namespace EnDecrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string message = "Hello World";
            string en=ED.Encrypt(message);
            Console.WriteLine("Zashifr "+ en);
            string ed=ED.Decrypt(message);
            Console.WriteLine("Rashfr "+ ed);
        }
    }
}
