# MclScript-Interpreter

The interpreter for my custom programming language "MclScript."
MclScript combines all my favorites features of both C and Python but, unlike either, is completely whitespace agnostic; meaning an entire program can be written without a single space, tab, or newline. It is currently in version 0 and very limited (For example: only the integer datatype is supported.) However, I plan will occasionally return to improve and extend the language.

Here is an example MclScript program:
```
# Execution always begins in the 'main' function.
main()
{       
    x = 10;
    while (x)
    {
        x = x - 1;
        if (x - 5) { print(x); }
    }
    
    print(9999999); # Print output
    
    a = 1; b = 2; c = 3;

    # Long complicated expression
    d = func(0, a, b) + ((5 + (420 - 0) * c 
            * (func((a + b) * c + 3, 1+2-7, 69)))) + 1;
    
    y = input(); # User input
    if (y) { print(y); }
    else (1) { print(-100); }
    
    print(d);
}

func(x, y, z)
{
    if (z) {
        # Set function return value
        return = (x + y + z);
    }
    else (1) {
        return = ((x+y)-z);
    }
}
```
