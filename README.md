# MclScript-Interpreter

The interpreter for my custom programming language "MclScript."

MclScript combines all my favorite features of both C and Python, but unlike either, it is completely whitespace agnostic; 
meaning an entire program can be written without a single space, tab, or newline (Aside from comments.) 
It is currently in early development and still very limited. For example: only the integer datatype is currently supported. 
However, I will occasionally return to improve and extend the language over time.

---
## Example Programs

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

Hello, World!
```
main()
{
    print(0);
}
```

Calculate Fibonacci numbers recursively.
```
main()
{
    n = input();
    print(fib());
}

fib(n)
{
    if (n < 2) { return = n; }
    else (1) { return = fib(n - 1) + fib(n - 2); }
}
```

Sum all multiples of 3 or 5 below given number.
```
main()
{
    n = input();
    sum = 0;
    i = 1;
    while (i < n)
    {
        if (((i % 3) : 0) | ((i % 5) : 0)) { sum = sum + i; }
        i = i + 1;
    }
    print(sum);
}
```

---
## Specifications

Operators

    Misc
    ----
    =   (Assignment)


    Arithmatic
    ----------
    +   (Addition)
    -   (Subtraction)
    *   (Multiplication)
    /   (Division)
    %   (Modulus)


    Comparison
    ----------
    :   (Equals)
    !   (Not Equals)
    <   (Less Than)
    >   (Greater Than)


    Boolean
    -------
    &   (And)
    |   (Or)

Keywords

    Flow Control
    ------------
    if
    else
    while


    Input/Output
    ------------------
    print
    input


    Special Variables
    ------------------
    return