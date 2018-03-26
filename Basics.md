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
      
    - Discriminated Unions
      -While tuples and records are types of multiplication, Discriminated Unions are types of addition.
      -Each component is called *union case*, and each one contains a *case identifier* or *tag*. Each *tag* must start with upper case.
      -Disciminated Unions are type safe, and data can only be accessed one way.
      - type IntOrBool =    //IntOrBool is the sum of all Integer or Boolean
        | Integer of int
        | Boolean of bool 
      - type IntOrBool = Integers of int | Booleans of bool   //Note: vertical bar is only option before the first component.
      
      - type AnyType =
        | Customer of Customer       //Labels can have the same name of the component type. Common, used as documentation.
        | ATuple of int * string
        | AListOfIntOrBool of IntOrBool list    //Note: Custom types muust be pre-defined.
        | AnEmptyCase     //No need for a type.
        
      -Construction
      - let anInt = Integer 33
      - let aBool = Boolean false
      - let aCustomer = Customer {firstName = "Bob"; lastName = "Bobson"}
      - let anEmptyCase = AnEmptyCase
      - let aFewSquares = 
          [1..5]
          |> List.map Square
      
        -Naming conflicts
        - type OtherIntOrBool = Integer of Int | Boolean of bool
        - let aValue = Integer 3      //? which type is it?

        - let otherValue = OtherIntOrBool.Integer 52
        
      -Deconstruction with *match*
      - let intOrBool x =
          match x wtih
          | Integer i -> printfn "This is an int! %i" i
          | Boolean b -> printfn "This is a bool! %A" b
          
      - let anInt = Integer 46
      - intOrBool anInt
      
      -Single cases
        -Useful practice to enforce type safety.
        
        - type CustomerId = int   // What is this called?
        - let printCustomerId (customerId:CustomerId) =
            printfn ("This is the customerId %i" customerId)
        - let paymentMethodId = 123
        - printCustomerId paymentMethodId     //What happens?
        
        
        - type CustomerId = CustomerId of int
        - let printCustomerId (CustomerId customerId) =   // What are we doing with customerId?
            printfn ("This is the customerId %i" customerId)
        - let paymentMethodId = 123
        - printCustomerId paymentMethodId     //What happens?
        
        - let customerId = CustomerId 321
        - printCustomerId customerId
        
        - let (CustomerId customerIdInt) = customerId   //Note: parenthesis must surround the deconstruction.
        - type SingleEmptyCase = | EmptyCase      //Note: vertical bar must be present.
        
      -Equality
        -Two unions are equal if they have the same type, the same case and the values for that case are equal.
        - type PaymentMethod = Cash of decimal | Debit of DebitCard
        - let aCashPayment = Cash 438.72
        - let otherCashPayment = Cash 438.72
        - let areEqual = (aCashPayment = otherCashPayment)
      
      -Printing
      - printfn "%A" aCashPayment  //Nice representation
      - printfn "%O" otherCashPayment  //Not nice

    - Object expressions
      -It allows to implement an interface on-the-fly, without having to create a class.
      
      - let createResource name =
          { new System.IDisposable
            with member this.Dispose() = printfn "%A disposed" name }
          let useThenDisposeResource =
            use resource = createResource "A resource"
            printfn "Starting to use resource"
            use otherResource = createResource "Another resource"
            printfn "Starting to use another resource"
            printfn "done."
          
    - Option.
      - type Option<'a> =
          | Some of 'a
          | None
      
      - let someInt = Some 2    //Constructor
        let noInt = None
        
        match someInt with
        | Some i -> printfn "Here is the int %d" x    //Deconstructor
        | None -> printfn "No value"
        
      -Defining option type.
        - type MiddleName = Option<string>
        - type PhoneNumber = string option
      
      - ["a","b","c"] |> List.tryFind (fun x -> x = "b")  // ??
      - ["a","b","c"] |> List.tryFind (fun x -> x = "d")  // ??
      
      -Printing
      - let middleName = MiddleName "Dolores"
      - printfn "%A" middleName  //Nice representation
      - printfn "%O" middleName  //Also nice
      
      -WARNING:
        -Using `IsSome`, `IsNone` and `Value` should be avoided. Using pattern matching instead. 
         Why?
         
      -Option module
        -map
          - let aCost = Some 123.32
            let aTax = 0.15
            let addTaxes =
              match aCost with
              | Some c -> Some(c + c * aTax)
              | None -> None
              
          - let addTaxes =
              aCost
              |> Option.map (fun c -> c + c * aTax)
          
        -fold
          - let amountToPay quantity =
            match addTaxes with
            | Some x -> x * quantity
            | None -> 0
          - let amountToPay quantity =
              addTaxes
              |> Option.fold (fun x -> x * quantity) 0

    - Classes
      -They always have parantheses after the class name.
      -Must have functions attached to them as members.
      - type CustomerName(firstName, middleInitial, lastName) =   //Note that no parameters type is needed
          member this.FirstName = firstName           //`this` could be any identifier you want. Just needs to be consistent
          member this.MiddleInitial = middleInitial
          member this.LastName = lastName  
      - type CustomerName(firstName:string, middleInitial:string option, lastName:string) =
          member this.FirstName = firstName
          member this.MiddleInitial = middleInitial
          member this.LastName = lastName
      - type CustomerName(firstName:string, middleNames:(string * string) option, lastName:string) =
          member this.FirstName = firstName
          member this.MiddleNames = middleNames
          member this.LastName = lastName
          
      -Signature
        -Given:
        - type ASquare(length:int, name:string) = 
            member this.Area = length * length
            member this.Rotate angle times = angle * times
        -Its signature would be:
        - type ASquare =
            class
              new : length:int * name:string -> ASquare       //constructor signature. Always called `new`
              member Area : int                               //property signature
              member Rotate : angle:int -> times:int -> int   //method signature
            end
      -Optional private fields and functions.
        - type ASquare(length:int, name:string) =
            let mutable mutableColor = "red"                 //private mutable value
            let scaleUp scale = length * scale               //private function
            member this.Area = length * length
            member this.Rotate angle times = angle * times
            
            member this.ScaleUp scale = scaleUp scale         //public function
            member this.SetMutableColor color =               //public wrapper for mutable value
              mutableColor <- color
            
          let aSquareInstance = new ASquare(32, "Squarito")
          printf "My size would be %d when doubled" (aSquaritoInstance.ScaleUp 2)
          aSquaritoInstance.SetMutableColor "purple"
          
      -Mutable constructor parameters
        - type AMutableSquare(length:int, name:string) =
            let mutable mutableLength = length
            
            member this.SetLength length =
              mutableLength <- length
              
      -Extra constructor behaviour
        - type ASquare(length:int, name:string) as this =               //Note the `this`, only needed for `PrintMyArea`
            let mutable mutableColor = "red" 
            do printfn "My name is %s and my color is %s" name color    //This is a good way of extra behaviour
            do this.PrintMyArea()                                       //This is not that good
            
            member this.Area = length * length
            
            member this.PrintMyArea() =
              do printfn "My area is %d" this.Area 
          
          new ASquare(65, "Your name")
          
      -Methods
        - type CustomerName(firstName, middleInitial, lastName) =
            member this.FirstName = firstName
            member this.MiddleInitial = middleInitial
            member this.LastName = lastName  
            
            // Parameterless method. Notice the parenthesis.
            method this.PrintName() =
              printfn "My name is %s %s %s" this.FirstName this.MiddleInitial this.LastName
            
            // Parameter method
            method this.GreetPerson nameOfPerson =
              printfn "Hello %s. %s" nameOfPerson this.PrintName
              
          let aCustomer = ACustomer("Bob", "A", "Bobson")    //Note that `new` is not needed.
          aCustomer.GreetPerson "Joe"
          
        -Curried vs Tuple
          - type CurriedVsTuple() =
              member this.CurriedAdd x y =
                x + y
              
              member this.TupleAdd(x,y) =
                x + y
            
            let test = new CurriedVsTuple()
            printfn "%i" <| tc.CurriedAdd 1 2
            printfn "%i" <| tc.TupleAdd(1,2)
            
          -The advantages of tuple form are:
           * Compatible with other .NET code
           * Supports named parameters and optional parameters
           * Supports method overloads
          -The disadvantages of tuple form are:
           * Doesn’t support partial application
           * Doesn’t work well with higher order functions
           * Doesn’t work well with type inference
          
      -Mutable properties
        - type ASquare(length) = 
            let mutable length = length

            member this.Length 
                with get() = length 
                and set(value) =  Length <- value

            member val Color = "Red"
            member val Color = "Red" with get, set
            
      -Secondary constructors
        - type ASquare(length, name) = 
            new(length) = 
                ASquare(length,"NoName") 
            new() = 
                ASquare(length,"NoName") 
                
      -Static
        -Members
          - type ASquare(length, name) = 
              member this.Length = length
              static member NumberOfSides = 4       //Note: There is no `this`.
              
            let aSquare = new ASquare(83, "Cuadrado")
            printfn "The length is %i" aSquare.Length
            printfn "The number of sides of a square is %i" ASquare.NumberOfSides
            
      -Constructors
        -No static constructors in F#. But it can be emulated.
        - type StaticConstructor() =
            static let rand = new System.Random()
            static do printfn "This can be any other `do`"
            member this.GetRand() = rand.Next()
              
      -Accesibility
        -All class members are public by default.
        -`public`, `internal`, `private`

        - type CustomerName(firstName, middleInitial, lastName) =
            member private this.FirstName = firstName
            member internal this.MiddleInitial = middleInitial
            member this.LastName = lastName
            
      -Interop
        -Best to define them in a namespace instead of a module, since modules are exposed as static classes, and classes inside of module are then defined as nested classes inside the static class.
        

    - Inheritance and abstact classes
      -Syntax
        - type DerivedClass(param1, param2) =
            inherit BaseClass(param1)           //Note it contains the name of the class and constructor already
            
      -Abstract and virtual methods
        - type BaseClass() =
            abstract member Add: int -> int -> int    // What is the concrete function?
          
      -Abstract properties
        - type BaseClass() =
            abstract member Pi : float
            abstract SideLength : int with get, set
            
      -However abstract definitions alone won't compile. In order to fix this:
        -A defaul implementation of the method must be privded; or
        -Mark the class *abstract* as a whole.
          
      -Default implementations
        - type BaseClass() =
           abstract member Add: int -> int -> int
           abstract member Pi : float 

           // defaults
           default this.Add x y = x + y
           default this.Pi = 3.1415
            
      -Abstract classes
        - [<AbstractClass>]
          type AbstractBaseClass() =
             abstract member Add: int -> int -> int
             abstract member Pi : float 
             abstract member SideLength : float with get,set
      
      -Overriding methods
        - [<AbstractClass>]
          type Currency() =
            abstract member Symbol: unit -> string 

          type Dollar() =
            inherit Currency() 
            override this.Symbol () = "$"
             
        - type Phone() =
            default this.Cost() = "10"
  
          type ApplePhone() =
            inherit Phone() 
            override this.Cost() = base.Cost() * 3
    
    - Interfaces
      -Syntax
        - type MyInterface =
            abstract member Add: int -> int -> int
            abstract member Pi : float
            abstract member SideLength : float with get,set
        - Whats the difference between interfaces and abstract classes?

      -Implementation
        - type IAddTaxes =
            abstract member AddTaxes: decimalt -> decimal -> decimal

          type TaxableItem() =
            interface IAddTaxes with 
                member this.AddTaxes cost tax = 
                    cost + (cost * tax)

            interface System.IDisposable with 
                member this.Dispose() = 
                    printfn "disposed"

      -Usage
        -The class must be casted to the interface in order to use its method.
        - let aPhone = TaxableItem()
          let phoneTaxer = aPhone :> IAddTaxes      //:> means casting
          phoneTaxer.AddTaxes 10.10 0.15
   
        - let addTaxesService (taxer:IAddTaxes) =
            printfn "The total cost is %d" <| taxes.AddTaxes 10.10 0.15
          addTaxesServices aPhone
