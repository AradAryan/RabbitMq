using System.Threading;

internal class Program
{
    public  string Text { get; set; } = "";
    private static async Task Main(string[] args)
    {
        ////Test();
        ////Test();
        //List<Task> tasks = new();
        //tasks.Add(Test(1));
        //tasks.Add(Test(2));

        //Task.WhenAll(tasks);
        //Console.WriteLine("done");
        //Console.ReadLine();
        try
        {
            await Task.WhenAll(Task.Delay(1000), Task.Delay(1000), Task.Delay(1000), Task.Delay(1000), Task.Delay(1000), Task.Delay(1000), Task.Delay(1000), Task.Delay(1000), Task.Delay(1000));
            //await Task.Delay(10000);
            throw new ArgumentException("Oh noes!");
        }
        catch (ArgumentException aex)
        {
            Console.WriteLine($"Caught ArgumentException: {aex.Message}");
        }
    }

    public  async Task<int> Test(int a)
    {
        Thread.Sleep(5000);
        Text += $"{a},";
        return a;
    }

    //public static async Task Func(int a)
    //{
        
    //}
}


//public class test
//{

//    public string Text { get; set; } = "";
//    private void Action(string[] args)
//    {
//        //Test();
//        //Test();
//        List<Task> tasks = new();
//        tasks.Add(Test(1));
//        tasks.Add(Test(2));

//        Task.WhenAll(tasks);
//        Console.WriteLine("done");
//        Console.ReadLine();
//    }

//    public async Task<int> Test(int a)
//    {
//        await Thread.Sleep(5000);
//        Text += $"{a},";
//        return a;
//    }



//}