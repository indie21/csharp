using System;

public class HelloWorld
{
    static public void Main()
    {
        var name = "Good";
        Type nameType = name.GetType();
        Console.WriteLine("name type is "+nameType.ToString());
        Console.WriteLine("Hello，World");
    }
}