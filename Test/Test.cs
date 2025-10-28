namespace Test;

public class Test
{
    public static void Main()
    {
        List<Action> tests = new List<Action>();
        tests.Add(HelloWorld);
        tests.Add(Fibbonochi);
        tests.Add(MultiplesOf3Or5);
        foreach (Action test in tests) test();
        Console.WriteLine("All Tests Pass!");
    }

    private static int Interpret(string src, params int[] args)
    {
        List<string> strArgs = new List<string>();
        for (int i = 0; i < args.Length; i++) strArgs.Add(args[i].ToString());
        return MclScript.MclScript.Interpret(src, strArgs.ToArray());
    }
    
    // Test
    private static void HelloWorld()
    {
        string src = "main(){}";
        if (Interpret(src) != 0) throw new Exception("Test Failed!");
    }

    // Test
    private static void Fibbonochi()
    {
        string src = "main(n){if(n<2){return=n;}else(1){return=main(n-1)+main(n-2);}}";

        int func(int n)
        {
            if (n < 2) return n;
            return func(n - 1) + func(n - 2);
        }
        
        const int x = 20;
        if (Interpret(src, x) != func(x)) throw new Exception("Test Failed!");
    }

    // Test
    private static void MultiplesOf3Or5()
    {
        string src = "main(n){sum=0;i=1;while(i<n){if(((i%3):0)|((i%5):0)){sum=sum+i;}i=i+1;}return=sum;}";
        
        int func(int n)
        {
            int sum = 0;
            for (int i = 1; i < n; i++)
            {
                if (i % 3 == 0 || i % 5 == 0) sum += i;
            }
            return sum;
        }

        const int x = 1000;
        if (Interpret(src, x) != func(x)) throw new Exception("Test Failed!");
    }
}