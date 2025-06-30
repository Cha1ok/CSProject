namespace BD_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Model book1 = new Model() { author = "asd", id = 123, name = "asd" };
                Model book2 = new Model() { author = "sad", id = 123, name = "asdd" };
            }

        }
    }
}
