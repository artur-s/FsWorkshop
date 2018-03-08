## Basics

  - function signature
    - Values
      - Signature. val aName: type = constant.
      
      - The `let` keyword defines an (immutable) value. No types needed
        ```fsharp
        let anInt = 3
        let aString = "Hola"
        let aList = [1;2;3]
        ```
    - Functions. Parameters are in no need of parenthesis, but it needed for precedence.
      - Signature
      - `val functionName : domain -> range`
      - let square x = x * x
        square 3
      - let add x y = x + y
        add 1 2
      - Multiline functions. Indentation is important.
          ```fsharp
          let odds list =
            let isOdd x = x%2 = 1
            List.filter isOdd list
          odds aList
          ```
          
    - There is very little difference between simple values and function values. 
    - One of the key aspects of thinking functionally is: 
       functions are values that can be passed around as inputs to other functions.
  
  - Currying
    - A function with multiple parameters is rewritten as a series of new tions, each with only one parameter.
    - Done automatically by the compiler
    
    - let add x y =
        x + y
      // Curried version. Note the one parameter per function
      let add x =
        let subFunction y =
          x + y
        subFunction
        
    - Signature
      val add : int -> (int -> int)
      val add : int -> int -> int
      
      int->string->bool->int      ?
      (int->string)->int          ?
      (int->string)->(int->bool)  ?
        
  - Partial application
    - Fixing the first N parameters of the function, gets a function of the ining parameters.
    
    - let printer = printfn "printing param=%i" 
      printer 3
    - let add3 = (+) 3
      add3 1
      
    - Order matters:
      1. First: Parameters more likely to be static
      2. Last: The data structure or collection (or most varying argument)
      3. For well-known operations such as “subtract”, put in the expected order
      
      - List
        - List-function [function parameter(s)] [list]
          - List.filter isOdd list
          - List.map (fun i -> i+1) [0;1;2;3]
          - let eachAdd1 = List.map (fun i -> i+1) 
            eachAdd1 [0;1;2;3]
            
  - Pipelining
    - Signature
      `let (|>) x f = f x`
    
    - It allows to put the function argument in front of the function rather after.
    - You can pipe the output of one operation to the next using "|>"
    - From Partial application ordering, having the data structure at the end
      makes it easier to pipe a structure or collection from function to tion. 
    - let result = 
        [1..10]
        |> List.map (fun i -> i+1)
        |> List.filter (fun i -> i>5) 
    
    - let add3Numbers x y z = x+y+z
      add3Numbers 1 2 3
    - let add3NumbersPartial = add3Numbers 1 2
      add3NumbersPartial 3
      3 |> add3NumbersPartial
      add3NumbersPartial <| 3
      
    - Reverse pipe function
      - let (<|) f x = f x
      - It reduces the need for parentheses and can make the code cleaner.
      
      - printf "%i" 1+2          // error
      - printf "%i" (1+2)        // using parens
      - printf "%i" <| 1+2       // using reverse pipe
      - let add x y = x + y
        1+2 |> add <| 3+4  
    
  - Function composition
    - Supose the following functions
      - let f (x:int) = float x * 3.0
      - let g (x:float) = x > 5.0
    - Then the following function takes the output of "f" and inputs it into "g".  
      - let h (x:int) =
          let y = f(x)
          g(y)
    - Or...  
      - let h (x:int) = g ( f(x) )
    - Additionlly, the two functions can be combined without declaring its signatures.  
      - let compose f g x = g ( f(x) )
    - And its signature:  
      - let compose : ('a -> 'b) -> ('b -> 'c) -> 'a -> 'c    
    - The actual definition of composition. 
      - let (>>) f g x = g ( f(x) )
    - Note: This is only possible because every function has one input and one output.
    
    - Example:  
      - let add3 x = x + 3
      - let multiply2 x = x * 2
      - let add3Multiply2 x = (>>) add3 multiply2 x
    - We can partially apply it
      - let add3Multiply2 = (>>) add3 multiply2
    - And since now its a binary operation
      - let add3Multiply2 = add3 >> multiply2
      - add3Multiply2 5
    - Reverse composition
      - let (<<) f g x = g ( f(x) )
      - Used mainly to make code more like English
      
      - let aList = []
      - aList |> List.isEmpty |> not
      - aList |> (not << List.isEmpty)
    
  - Pipelining vs Function composition
    - Pipelining -> The input to each function is the output of the previous function.
    - Function composition -> It returns a function instead of immediately invoking the sequence.
    
  - Types
    - They are used in two main ways:
      * As compile time unit tests.
      * As domains for functions to act upon. It is a sort of data modeling tool that allows to represent a real world domain in code.
      * The better the type definitions reflect the real-world domain, the better they will statically encode the business rules. And the better they statically encode the business rules, the better the “compile time unit tests” work. In the ideal scenario, if your program compiles, then it really is correct!
    - All tpes definitions start with a `type` keyword, followed by an identifier for the type, followed by any generic type parameters, followed by the definition.
      - type A = int
      - type B = int * int
      - type C = {FirstName:string; LastName:string}
      - type D = Square of int | Rectangle of int * int
      - type E<'a> = AChoice of 'a | OtherChoice of 'a * 'a 
    - They can only be declared in namespaces or modules.
    - Can't be declared inside functions.
    
    - Constructing types
      - let a = 3
      - let b = (6, 39)
      - let c = {FirstName="Grzegorz"; LastName="Brzęczyszczykiewicz"
      - let d = Rectangle (5, 6)
      - let e = AChoice "this choice"
    - Deconstructing types
      - let (b1, b2) = (6, 39)
      - let { FirstName = c1 } = c
      - match d with
        | Square d1 -> printf "Square with sides %i" d1
        | Rectangle (d1, d2) -> printf "Rectangle with sides %i %i" d1 d2
    
    - Abbreviations or aliases
      - type [name] = [existingType]
      - type PaymentMethodId = int
      - type CustomerId = Guid
      - type CustomerPaymentMethod = PaymentMethodId * CustomerId
      -They provide documentation.
      -Decoupling between usage and the implementation of a type.
      -Its not really a new type, just an alias.

    - Tuples
      -Imagine the Cartesian product of two collections. Each combination is expressed as (a1, b1), (a1, b2), ..., (a2, b1)
      -Hence the type signature that they do.
      - let tuple1 = (3, 9)             //Signature: int * int
      - let tuple2 = ("Hola!", true)    //Signature: string * bool
      - let tuple4 = ("Bob", 42, true)
      - let tuple5 = 1, 2, 3            //Note that parenthesis doesn't matter.
      - type PersonalPayment = Person * PaymentMethod
      -Tuples are single objects.
      -Order matters -> `int*bool` not the same as `bool*int`
      -The comma is the most important of tuples.
      - let t = 3, 6    //Constructing
      - let t1, t2 = t  //Deconstructing
      - let t1, _ = t   //Underscore means "whatever"
      
      - let t1 = fst t  //fst extracts the first element
      - let t2 = snd t  //snd extracts the second element.
      
      -Tuples are equal if they have the same length and values in each slot.
      - (1,2) = (1,2)             ?
      - (1,2,3) = (1,3,2)         ?
      - (1, (2,3), 4) = (1,2,3,4) ?
      - (1,(2,3),4) = (1,2,(3,4)) ?
      - (1,2) = (1,2,3)           ?
      
      -Printing
      - printf "%s" t1.ToString()
      - printf "%O" t1

    - Records
      -Records are tuples where each element is labeled.
      - type Person = {firstName: string; lastName: string}
      - let aPerson = {firstName = "Juan"; lastName: "Perez"}     //What are the differences?
      
      - let {firstName = fName; lastName = lName} = aPerson     //What is this?
      - let {firstName = _; lastName = lName} = aPerson
      
      - let firstName = aPerson.firstName
      - let lastName = aPerson.lastName
      
      -Order doesn't matter
      - let bPerson = {lastName = "Perez"; firstName = "Juan"}
      - aPerson = bPerson
      
      -Records might have same structure.
      - let Customer = {firstName: string; lastName: string}
      - let aDude = {firstName = "John"; lastName = "Johnson"}     //What type is aDude?
      -To break ambiguity, add the type name to at least one of the labels.
      - let aCustomer = {Customer.firstName="John", lastName="Johnson"}
      
      -Note that in F#, unlike some other functional languages, two types with exactly the same structural definition are not the same type. Two types are only equal if they have the same name.
      
      -With
      - let aCustomerChild = {aCustomer with firstName="Little Johnny"}
      
      -Printing
      - printfn "%A" aCustomer  //Nice representation
      - printfn "%O" aCustomer  //Not nice
