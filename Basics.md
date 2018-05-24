## Basics

1. Function signature
    - Values
      - Signature. 
        ```fsharp
        val aName: type = expression.
        ```
      
      - The `let` keyword defines an (immutable) value. No types needed
        ```fsharp
        let anInt = 3
        let aString = "Hola"
        let aList = [1;2;3]
        ```
    - Functions. Parameters are in no need of parenthesis, but they are needed for precedence.
      - Signature
        ```fsharp
        val functionName : domain -> range
        ```
        ```fsharp
        let square x = x * x
        square 3
        ```
        ```fsharp
        let add x y = x + y
        add 1 2
        ```
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

2. Currying
    - A function taking multiple arguments separated by spaces is generated as a function that takes a single argument and returns a function that takes next argument, etc.
      
      ```fsharp
      let add x y =
        x + y
      // Curried version. Note the one parameter per function
      let add x =
        let subFunction y =
          x + y
        subFunction
      ```
        
    - Signature
      ```fsharp
      val add : int -> (int -> int)
      val add : int -> int -> int
      ```
      
      - `int->string->bool->int`      ?
      - `(int->string)->int`          ?
      - `(int->string)->(int->bool)`  ?
        
3. Partial application
    - Fixing the first N parameters of the function, gets a function of the remaining parameters.
    
      ```fsharp
      let printer = printfn "printing param=%i" 
      printer 3
      ```
      ```fsharp
      let add3 = (+) 3
      add3 1
      ```
      
    - Order matters:
      1. First: Parameters more likely to be static
      2. Last: The data structure or collection (or most varying argument)
      3. For well-known operations such as “subtract”, put in the expected order
      
      - List
        - `List-function [function parameter(s)] [list]`
          ```fsharp
          List.filter isOdd list
          ```
          ```fsharp
          List.map (fun i -> i+1) [0;1;2;3]
          ```
          ```fsharp
          let eachAdd1 = List.map (fun i -> i+1) 
          eachAdd1 [0;1;2;3]
          ```
            
4. Pipelining
    - Signature
      ```fsharp
      let (|>) x f = f x
      ```
    
    - It allows to put the function argument in front of the function rather than after.
    - You can pipe the output of one operation to the next using "|>"
    - From Partial Application ordering, having the data structure at the end
      makes it easier to pipe a structure or collection from function to function. 
      ```fsharp
      let result = 
        [1..10]
        |> List.map (fun i -> i+1)
        |> List.filter (fun i -> i>5)
      ```
      ```fsharplet add3Numbers x y z = x+y+z
      add3Numbers 1 2 3
      ```
      ```fsharp
      let add3NumbersPartial = add3Numbers 1 2
      add3NumbersPartial 3
      3 |> add3NumbersPartial
      add3NumbersPartial <| 3
      ```
      
    - Reverse pipe function
      ```fsharp
      let (<|) f x = f x
      ```
      - It reduces the need for parentheses and can make the code cleaner.
      
        ```fsharp
        printf "%i" 1+2          // error
        ```
        ```fsharp
        printf "%i" (1+2)        // using parens
        ```
        ```fsharp
        printf "%i" <| 1+2       // using reverse pipe
        ```
        ```fsharp
        let add x y = x + y
        1+2 |> add <| 3+4
        ```
    
5. Function composition
    - Supose the following functions
      ```fsharp
      let f (x:int) = float x * 3.0
      let g (x:float) = x > 5.0
      ```
    - Then the following function takes the output of "f" and inputs it into "g".  
      ```fsharp
        let h (x:int) =
          let y = f(x)
          g(y)
      ```
    - Or...  
      ```fsharp
      let h (x:int) = g ( f(x) )
      ```
    - Composition can be abstracted to work on any two matching functions.

      ```fsharp
      let compose f g x = g ( f(x) )
      ```
    - And its signature:  
      ```fsharp
      let compose : ('a -> 'b) -> ('b -> 'c) -> 'a -> 'c
      ```
    - The definition of composition operator. 
      ```fsharp
      let (>>) f g x = g ( f(x) )
      ```
      Or using partial application
      ```fsharp
      let (>>) f g = fun x -> g (f x)
      ```
    - Note: This is only possible because every function has one input and one output.
    
    - Example:  
      ```fsharp
      let add3 x = x + 3
      let multiply2 x = x * 2
      let add3Multiply2 = add3 >> multiply2
      add3Multiply2 5
      ```
    - Reverse composition
      ```fsharp
      let reverseCompose : ('b -> 'c) -> ('a -> 'b) -> 'a -> 'c
      let (<<) f g x = g ( f(x) )
      ```
      - Used mainly to make code more like English and more concise
      
      ```fsharp
      let aList = []
      aList |> List.isEmpty |> not
      aList |> (not << List.isEmpty)
      ```
    
6. Pipelining vs Function composition
    - Pipelining -> The input to each function is the output of the previous function. In other words, the expression on the left side of the `|>` operator is passed to the function on the right side.
    - Function composition -> It returns a function instead of immediately invoking the sequence.
    
7. Types
    - The better the type definitions reflect the real-world domain, the better they will statically encode the business rules. And the better they statically encode the business rules, the better the “compile time unit tests” work. In the ideal scenario, if your program compiles, then it really is correct!
    - All types definitions start with a `type` keyword, followed by an identifier for the type, followed by any generic type parameters, followed by the definition.
      ```fsharp
      type A = int
      ```
      ```fsharp
      type B = int * int
      ```
      ```fsharp
      type C = {FirstName:string; LastName:string}
      ```
      ```fsharp
      type D = Square of int | Rectangle of int * int
      ```
      ```fsharp
      type E<'a> = AChoice of 'a | OtherChoice of 'a * 'a 
      ```
    - They can only be declared in namespaces or modules.
    - Can't be declared inside functions.
    
    - Constructing values
      ```fsharp
      let a = 3
      ```
      ```fsharp
      let b = (6, 39)
      ```
      ```fsharp
      let c = {FirstName="Grzegorz"; LastName="Brzęczyszczykiewicz"
      ```
      ```fsharp
      let d = Rectangle (5, 6)
      ```
      ```fsharp
      let e = AChoice "this choice"
      ```
    - Deconstructing values
      ```fsharp
      let (b1, b2) = (6, 39)
      ```
      ```fsharp
      let { FirstName = c1 } = c
      ```
      ```fsharp
      match d with
      | Square d1 -> printf "Square with sides %i" d1
      | Rectangle (d1, d2) -> printf "Rectangle with sides %i %i" d1 d2
      ```
    
    1. Abbreviations or aliases
       ```fsharp
       type [alias] = [existingType]
       ```
       ```fsharp
       type PaymentMethodId = int
       ```
       ```fsharp
       type CustomerId = Guid
       ```
       ```fsharp
       type CustomerPaymentMethod = PaymentMethodId * CustomerId
       ```
       - They provide documentation.
       - Decoupling between the meaning and the definition of a type.
       - Its not really a new type, just an alias.
       - Two different aliases of the same type are compatible and the  compiler does not show an error. 
         For example, the following code type-checks even though a different alias is passed to the printInvoiceId function.
         ```fsharp
         type InvoiceId = Guid
         type CustomerId = Guid
 
         let printInvoiceId (i:InvoiceId) =
             printfn "Invoice id is:%A" i
 
         let customerId:CustomerId = Guid.NewGuid()
         printInvoiceId customerId
         ```
 
    2. Tuples
       - Imagine the Cartesian product of two collections. Each  combination is expressed as (a1, b1), (a1, b2), ..., (a2, b1)
       - Hence the type signature that they do.
         ```fsharp
         let tuple1 = (3, 9)             //Signature: int * int
         ```
         ```fsharp
         let tuple2 = ("Hola!", true)    //Signature: string * bool
         ```
         ```fsharp
         let tuple4 = ("Bob", 42, true)
         ```
         ```fsharp
         let tuple5 = 1, 2, 3            //Note that parenthesis are  not required.
         ```
         ```fsharp
         type PersonalPayment = Person * PaymentMethod
         ```
       - Tuples are single values.
       - Order matters -> `int*bool` not the same as `bool*int`
       - The comma is the most important characteristic of tuples.
         ```fsharp
         let t = 3, 6    //Constructing
         ```
         ```fsharp
         let t1, t2 = t  //Deconstructing
         ```
         ```fsharp
         let t1, _ = t   //_ is a wildcard
         ```
         ```fsharp
         let t1 = fst t  //fst extracts the first element
         ```
         ```fsharp
         let t2 = snd t  //snd extracts the second element.
         ```
       
       - Tuples are equal if they have the same length and values in  each slot.
         - `(1,2) = (1,2)`             ?
         - `(1,2,3) = (1,3,2)`         ?
         - `(1, (2,3), 4) = (1,2,3,4)` ?
         - `(1,(2,3),4) = (1,2,(3,4))` ?
         - `(1,2) = (1,2,3)`           ?
       
       - Printing
         ```fsharp
         printf "%s" t1.ToString()
         printfn "%s" t1.ToString()
         ```
         ```fsharp
         printf "%O" t1
         ```
 
    3. Records
       - Records are tuples where each element is labeled.
         ```fsharp
           type Person = {firstName: string; lastName: string}
           let aPerson = {firstName = "Juan"; lastName: "Perez"}      //What are the differences?
         
           let {firstName = fName; lastName = lName} = aPerson      //What is this?
           let {firstName = _; lastName = lName} = aPerson
         
           let firstName = aPerson.firstName
           let lastName = aPerson.lastName
         ```
       
       - Order doesn't matter
         ```fsharp
           let bPerson = {lastName = "Perez"; firstName = "Juan"}
           aPerson = bPerson
         ```
       
       - Records might have same structure.
         ```fsharp
         let Customer = {firstName: string; lastName: string}
         let aDude = {firstName = "John"; lastName = "Johnson"}      //What type is aDude?
         ```
       - To break ambiguity, add the type name to at least one of the  labels.
         ```fsharp
         let aCustomer = {Customer.firstName="John",  lastName="Johnson"}
         ```
       
       - Note that in F#, unlike some other functional languages, two types with exactly the same structural definition are not the same type. Two types are only equal if they have the same (fully qualified) name.
       
       - `with`
         ```fsharp
         let aCustomerChild = {aCustomer with firstName="Little  Johnny"}
         ```
       
       - Printing
         ```fsharp
         printfn "%A" aCustomer  //Nice representation
         printfn "%O" aCustomer  //Not nice
         ```
      
    4. Discriminated Unions
       - While tuples and records are types of multiplication,  Discriminated Unions are types of addition.
       - Each component is called *union case*, and each one contains  a *case identifier* or *tag*. Each *tag* must start with upper  case.
       - Disciminated Unions are type safe, and data can only be  accessed one way.
         ```fsharp
         type IntOrBool =    //IntOrBool is the sum of all Integer or  Boolean
         | Integer of int
         | Boolean of bool 
         ```
         ```fsharp
         type IntOrBool2 = Integers of int | Booleans of bool   //Note:  vertical bar is only option before the first component.
         ```
         ```fsharp
         type AnyType =
         | Customer of Customer       //Labels can have the same name  of the component type. Common, used as documentation.
         | ATuple of int * string
         | AListOfIntOrBool of IntOrBool list    //Note: Custom types  muust be pre-defined.
         | AnEmptyCase     //No need for a type.
         ```
         
       - Construction
         ```fsharp
         let anInt = Integer 33
         ```
         ```fsharp
         let aBool = Boolean false
         ```
         ```fsharp
         let aCustomer = Customer {firstName = "Bob"; lastName =  "Bobson"}
         ```
         ```fsharp
         let anEmptyCase = AnEmptyCase
         ```
         ```fsharp
         let aFewSquares = 
           [1..5]
           |> List.map Square
         ```
      
       - Naming conflicts
         ```fsharp
         type OtherIntOrBool = Integer of Int | Boolean of bool
         let aValue = Integer 3      //? which type is it?
 
         let otherValue = OtherIntOrBool.Integer 52
         ```
        
       - Deconstruction with *match*
        ```fsharp
        let intOrBool x =
          match x wtih
          | Integer i -> printfn "This is an int! %i" i
          | Boolean b -> printfn "This is a bool! %A" b
          
        let anInt = Integer 46
        intOrBool anInt
        ```
      
       - Single cases
         - Useful practice to enforce type safety.
         
           ```fsharp
           type CustomerId = int   // What is this called?
           let printCustomerId (customerId:CustomerId) =
             printfn ("This is the customerId %i" customerId)
           let paymentMethodId = 123
           printCustomerId paymentMethodId     //What happens?
         
         
           type CustomerId = CustomerId of int
           let printCustomerId (CustomerId customerId) =   // What are  we doing with customerId?
             printfn ("This is the customerId %i" customerId)
           let paymentMethodId = 123
           printCustomerId paymentMethodId     //What happens?
         
           let customerId = CustomerId 321
           printCustomerId customerId
         
           let (CustomerId customerIdInt) = customerId   //Note:  parenthesis must surround the deconstruction.
           type SingleEmptyCase = | EmptyCase      //Note: vertical  bar must be present.
           ```
         
       - Equality
         - Two unions are equal if they have the same type, the same  case and the values for that case are equal.
           ```fsharp
           type PaymentMethod = Cash of decimal | Debit of DebitCard
           let aCashPayment = Cash 438.72
           let otherCashPayment = Cash 438.72
           let areEqual = (aCashPayment = otherCashPayment)
           ```
       
       - Printing
         ```fsharp
         printfn "%A" aCashPayment  //Nice representation
         printfn "%O" otherCashPayment  //Not nice
         ```
 
    5. Object expressions
       - It allows to implement an interface on-the-fly, without  having to create a class.
       
         ```fsharp
         let createResource name =
           { new System.IDisposable
             with member this.Dispose() = printfn "%A disposed" name }
           let useThenDisposeResource =
             use resource = createResource "A resource"
             printfn "Starting to use resource"
             use otherResource = createResource "Another resource"
             printfn "Starting to use another resource"
             printfn "done."
         ```
          
    6. Option.
         ```fsharp
         type Option<'a> =
           | Some of 'a
           | None
         ```
         ```fsharp
         let someInt = Some 2    //Constructor
         let noInt = None
         
         match someInt with
         | Some i -> printfn "Here is the int %d" x    //Deconstructor
         | None -> printfn "No value"
         ```
         
       - Defining option type.
         ```fsharp
         type MiddleName = Option<string>
         ```
         ```fsharp
         type PhoneNumber = string option //recommended for built-in option and list types
         ```
         - `["a","b","c"] |> List.tryFind (fun x -> x = "b")`  // ??
         - `["a","b","c"] |> List.tryFind (fun x -> x = "d")`  // ??
 
       - Printing
         ```fsharp
         let middleName = MiddleName "Dolores"
         printfn "%A" middleName  //Nice representation
         printfn "%O" middleName  //Also nice
         ```
       
       - WARNING:
         - Using `IsSome`, `IsNone` and `Value` should be avoided. Use  pattern matching instead. However, `IsSome` and `IsNone` are  sometimes useful when you don't care about the value.
           Why?
          
       - Option module
         - `map`
             ```fsharp
             let aCost = Some 123.32
             let aTax = 0.15
             let addTaxes =
               match aCost with
               | Some c -> Some(c + c * aTax)
               | None -> None
               
             let addTaxes =
               aCost
               |> Option.map (fun c -> c + c * aTax)
             ```
           
         - `fold`
             ```fsharp
             let amountToPay quantity =
             match addTaxes with
             | Some x -> x * quantity
             | None -> 0
             
             let amountToPay quantity =
               addTaxes
               |> Option.fold (fun x -> x * quantity) 0
               //Or with partial application of (*)
               //|> Option.fold ((*) quantity) 0
             ```
 
    7. Classes
       - They always have parantheses after the class name.
       - Must have functions attached to them as members.
         ```fsharp
         type CustomerName(firstName, middleInitial, lastName) =    //Note that no parameters type is needed
           member this.FirstName = firstName           //`this` could  be any identifier you want. Just needs to be consistent
           member this.MiddleInitial = middleInitial
           member this.LastName = lastName 
         ```
         ```fsharp
         type CustomerName(firstName:string, middleInitial:string  option, lastName:string) =
           member this.FirstName = firstName
           member this.MiddleInitial = middleInitial
           member this.LastName = lastName
         ```
         ```fsharp
         type CustomerName(firstName:string, middleNames:(string *  string) option, lastName:string) =
           member this.FirstName = firstName
           member this.MiddleNames = middleNames
           member this.LastName = lastName
         ```
           
       - Signature
         - Given:
           ```fsharp
           type ASquare(length:int, name:string) = 
             member this.Area = length * length
             member this.Rotate angle times = angle * times
           ```
         - Its signature would be:
           ```fsharp
           type ASquare =
             class
               new : length:int * name:string -> ASquare        //constructor signature. Always called `new`
               member Area : int                                //property signature
               member Rotate : angle:int -> times:int -> int    //method signature
             end
           ```
       - Optional private fields and functions.
           ```fsharp
           type ASquare(length:int, name:string) =
             let mutable mutableColor = "red"                  //private mutable value
             let scaleUp scale = length * scale                //private function
             member this.Area = length * length
             member this.Rotate angle times = angle * times
             
             member this.ScaleUp scale = scaleUp scale          //public function
             member this.SetMutableColor color =                //public wrapper for mutable value
               mutableColor <- color
             
           let aSquareInstance = new ASquare(32, "Squarito")
           printf "My size would be %d when doubled"  (aSquaritoInstance.ScaleUp 2)
           aSquaritoInstance.SetMutableColor "purple"
           ```
           
       - Mutable constructor parameters
           ```fsharp
           type AMutableSquare(length:int, name:string) =
             let mutable mutableLength = length
             
             member this.SetLength length =
               mutableLength <- length
           ```
               
       - Extra constructor behaviour
           ```fsharp
           type ASquare(length:int, name:string) as this =                //Note the `this`, only needed for `PrintMyArea`
             let mutable mutableColor = "red" 
             do printfn "My name is %s and my color is %s" name color     //This is a good way of extra behaviour
             do this.PrintMyArea()                                        //This is not that good
             
             member this.Area = length * length
             
             member this.PrintMyArea() =
               do printfn "My area is %d" this.Area 
           
           new ASquare(65, "Your name")
           ```
           
       - Methods
           ```fsharp
           type CustomerName(firstName, middleInitial, lastName) =
             member this.FirstName = firstName
             member this.MiddleInitial = middleInitial
             member this.LastName = lastName  
             
             // Parameterless method. Notice the parenthesis.
             method this.PrintName() =
               printfn "My name is %s %s %s" this.FirstName  this.MiddleInitial this.LastName
             
             // Parameter method
             method this.GreetPerson nameOfPerson =
               printfn "Hello %s. %s" nameOfPerson this.PrintName
               
           let aCustomer = ACustomer("Bob", "A", "Bobson")    //Note  that `new` is not needed.
           aCustomer.GreetPerson "Joe"
           ```
           
       - Curried vs Tuple
           ```fsharp
           type CurriedVsTuple() =
             member this.CurriedAdd x y =
               x + y
 
             member this.TupleAdd(x,y) =
               x + y
 
           let test = new CurriedVsTuple()
           printfn "%i" <| tc.CurriedAdd 1 2
           printfn "%i" <| tc.TupleAdd(1,2)
           ```
 
         - The advantages of tuple form are:
          * Compatible with other .NET code
          * Supports named parameters and optional parameters
          * Supports method overloads
         - The disadvantages of tuple form are:
          * Doesn’t support partial application
          * Doesn’t work well with higher order functions
          * Doesn’t work well with type inference
           
       - Mutable properties
           ```fsharp
           type ASquare(length) = 
             let mutable length = length
 
             member this.Length 
                 with get() = length 
                 and set(value) =  Length <- value
 
             member val Color = "Red"
             member val Color = "Red" with get, set
           ```
             
       - Secondary constructors
           ```fsharp
           type ASquare(length, name) = 
             new(length) = 
                 ASquare(length,"NoName") 
             new() = 
                 ASquare(length,"NoName") 
           ```
                 
       - Static
         - Members
             ```fsharp
             type ASquare(length, name) = 
               member this.Length = length
               static member NumberOfSides = 4       //Note: There is  no `this`.
               
             let aSquare = new ASquare(83, "Cuadrado")
             printfn "The length is %i" aSquare.Length
             printfn "The number of sides of a square is %i"  ASquare.NumberOfSides
             ```
             
       - Constructors
         - No static constructors in F#. But it can be emulated.
           ```fsharp
           type StaticConstructor() =
             static let rand = new System.Random()
             static do printfn "This can be any other `do`"
             member this.GetRand() = rand.Next()
           ```
               
       - Accesibility
         - All class members are public by default.
         - `public`, `internal`, `private`
 
           ```fsharp
           type CustomerName(firstName, middleInitial, lastName) =
             member private this.FirstName = firstName
             member internal this.MiddleInitial = middleInitial
             member this.LastName = lastName
           ```
             
       - Interop
         - Best to define them in a namespace instead of a module,  since modules are exposed as static classes, and classes  inside of module are then defined as nested classes inside  the static class.
         

    8. Inheritance and abstact classes
       - Syntax
           ```fsharp
           type DerivedClass(param1, param2) =
             inherit BaseClass(param1)           //Note it contains  the name of the class and constructor already
           ```
             
       - Abstract and virtual methods
           ```fsharp
           type BaseClass() =
             abstract member Add: int -> int -> int    // What is the  concrete function?
           ```
           
       - Abstract properties
           ```fsharp
           type BaseClass() =
             abstract member Pi : float
             abstract SideLength : int with get, set
           ```
             
       - However abstract definitions alone won't compile. In order to  fix this:
         - A defaul implementation of the method must be privded; or
         - Mark the class *abstract* as a whole.
           
       - Default implementations
           ```fsharp
           type BaseClass() =
             abstract member Add: int -> int -> int
             abstract member Pi : float 
 
             // defaults
             default this.Add x y = x + y
             default this.Pi = 3.1415
           ```
             
       - Abstract classes
           ```fsharp
           [<AbstractClass>]
           type AbstractBaseClass() =
             abstract member Add: int -> int -> int
             abstract member Pi : float 
             abstract member SideLength : float with get,set
           ```
       
       - Overriding methods
           ```fsharp
           [<AbstractClass>]
           type Currency() =
             abstract member Symbol: unit -> string 
 
           type Dollar() =
             inherit Currency() 
             override this.Symbol () = "$"
           ```
           ```fsharp
           type Phone() =
             default this.Cost() = "10"
   
           type ApplePhone() =
             inherit Phone() 
             override this.Cost() = base.Cost() * 3
           ```
    
    9. Interfaces
       - Syntax
           ```fsharp
           type MyInterface =
             abstract member Add: int -> int -> int
             abstract member Pi : float
             abstract member SideLength : float with get,set
           ```
         - Whats the difference between interfaces and abstract  classes?
 
       - Implementation
           ```fsharp
           type IAddTaxes =
             abstract member AddTaxes: decimalt -> decimal -> decimal
 
           type TaxableItem() =
             interface IAddTaxes with 
                 member this.AddTaxes cost tax = 
                     cost + (cost * tax)
 
             interface System.IDisposable with 
                 member this.Dispose() = 
                     printfn "disposed"
           ```
 
       - Usage
         - The class must be casted to the interface in order to use  its method.
           ```fsharp
           let aPhone = TaxableItem()
           let phoneTaxer = aPhone :> IAddTaxes      //:> means casting
           phoneTaxer.AddTaxes 10.10 0.15
           ```
    
           ```fsharp
           let addTaxesService (taxer:IAddTaxes) =
             printfn "The total cost is %d" <| taxes.AddTaxes 10.10  0.15
           addTaxesServices aPhone
           ```
    
    
8. Expressions vs statements
    
    - _statement_ - a command that performs some action and usually changes the state  or transfers the control flow, e.g. `return` statement _p.328
    - _expression_ - a computation that can be evaluated and yields a result.

    A fundamental building block of F# programs is an _expression_. Entire F# program is a composite expression that returns a value.
    Every function returns a value, including impure functions performing only a side-effect, e.g:

    ```fsharp
    let res =
      if isNewYear then
          printfn "Launching fireworks..."
    ```
    evaluates to:
    ```
    Launching fireworks...
    val res : unit = ()
    ```
    The `()` is (the only possible) value of type `Unit`. It is used for representing lack of information and in practice it indicates that the expression (or function) performs a side-effect.

    The `if` statement is also an expression. Here it returns a value of type `Unit`. As no `else` clause is provided, the expression in `then` has to be of type `Unit`

    Sequencing expressions

    Expressions that evaluate to unit `()` can be chanined together placing them in a new line (preserving the same indentation) or using `;` operator.
    
    The `;` ignores the expression on the left, evaluates the expression on the right and returns its value.

    The below examples are equivalent. The `res` evaluates to `70150`
    ```fsharp
    let res =
      printf "Provisioning a toster ..."
      70150
    ```
    ```fsharp
    let res = 
      printf "Provisioning a toster ..." ; 70150
    ```

    The first example looks like it contains a code block, but internally it is two expressions that are sequentially composed using `;`.

    Every expressions that is not used, passed to or returned from a function, generates a compiler warnings.

    To explicitly ignore use the `ignore: a' -> unit` function.



9. Type Inference
    - Based on an algorithm called "Hindley-Milner".
      - Rules
        * Look at the literals
        * Look at the functions and other values something interacts with
        * Look at any explicit type constraints
        * If there are no constraints anywhere, automatically generalize to generic types

        - Look at the literals
            ```fsharp
            let inferredAsInt x = x + 3
            ```
            ```fsharp
            let inferredAsString x = x + "3"
            ```
            ```fsharp
            let inferredAsDecimal x = x + 3m
            ```
        - Look at the functions and other values something interacts with
            ```fsharp
            let indirectlyInferredAsInt x = inferredAsInt x
            ```
            ```fsharp
            let aString = "this is a string"
            let meToo = aString
            ```
            ```fsharp
            let inferredAsBool x = if x then 0 else 1
            ```
            ```fsharp
            let inferredAsSequenceOfInt x = for i in x do printfn "%i" x
            ```
        - Look at any explicit type constraints
            ```fsharp
            let inferredAlsoAsInt (x:int) = x
            ```
            ```fsharp
            let inferredIndirectlyAlsoAsInt x = inferredAlsoAsInt x
            ```
            ```fsharp
            let inferredAsIntDueToPrint x = printf "%i" x 
            ```
        - Automatically generalize to generic types
            ```fsharp
            let inferredAsGeneric x = x
            ```
            ```fsharp
            let inferredInderectlyAsGeneric x = inferredAsGeneric x
            ```
          
      - Type inference works top-down, bottom-up, front-to-back, back-to-front, etc.
      
      - Sometimes it fails to know:
        * Declarations out of order
        * Not enough information
        * Overloaded methods
        * Quirks of generic numeric functions
        
        - Declarations out of order
          ```fsharp
          let addTwoNumber x y = addThreeNumbers + y
          ```
          ```fsharp
          let addThreeNumbers x y z = x + y +z
          ```
          
          - Recursive
            ```fsharp
            let times2 x =
              if x = 0 then 1
              else x * 2
            ```
            ```fsharp
            let rec times2 x =      //rec needs to be added to indicate recursiveness
              if x = 0 then 1
              else x * 2
            ```
          
          - Simultaneus type
            ```fsharp
            type A = None | AUsesB of B
            type B = None | BUsesA of A
            ```
            ```fsharp
            type A = None | AUsesB of B       //*and* used for simultaneus declarations.
            and B = None | BUsesA of A
            ```
        
        - Not enough information and can't be generic
          ```fsharp
          let stringLength x = x.Length
          ```
          ```fsharp
          let stringLength (x:string) = x.Length
          ```
          
        - Overloaded methods
          ```fsharp
          let concat x = System.String.Concat(x)
          ```
          ```fsharp
          let concat (x:string) = System.String.Concat(x)
          ```
          
        - Quirks of generic numeric functions
          ```fsharp
          let times2 x = x * 2
          times2 3                //Assumes times2 is int
          times2 3.3              //Error
          ```
            
        - Solutions:
          - Define things before they are used.
          - Declare first known types.
          - Annotate all and then remove one by one until the minimum is achieved.
    
10. Pattern Matching
    - It is ubiquitous in F#:
      - Used for binding expressions with `let`
      - Function parameters
      - `match`..`with`
        
    1. Match.
        ```fsharp
        match [something] with 
        | pattern1 -> expression1
        | pattern2 -> expression2
        | pattern3 -> expression3
        ```
       - Note: It looks like a series of lamba expressions where each one has exactly one parameter.
        So, it can be seen as a choice between a set of lambda expressions.
        Each choice is defined by the first pattern that matches the expression.
        _Order is important_ (unlike `switch`)
                    
         ```fsharp
         let x =
           match "a" with
           | "a" -> 1
           | "b" -> 2
           | _ -> 999
         ```
         ```fsharp
         let x =
           match "a" with
           | _ -> 999
           | "a" -> 1
           | "b" -> 2
         ```
       - `match`..`with` is an expression; thus, all branches must evaluate to the same type
         ```fsharp
         let x y =
           match y with
           | "a" -> 1
           | "b" -> "letre"
           | _ -> false
         ```
       - Since it is an expression it can nested, embedded in a lambda, etc.
      
       - Formatting suggestions
         - Alignment of `| expression` should be directly under `match`
         - `match`..`with` should be on a new line
         - The expression after `->` should be on a new line when expression is long.
         
       - Exhaustive matching
         - There must always be a branch that matches.
           - Compiler will warn about it. (Sometimes unnecessarily, in  which a `_` can be used. Be sure to document why its being  used)
           - If ignored and unmatched, a `MatchFailureException` will  be thrown.
         - Avoid using wildcard, specially in union types. This will  help catching errors when a new case is added to the union.
             ```fsharp
             type PaymentMethods = Cash | Debit | Credit
             let pay paymentMethod =
               match paymentMethod with
               | Cash -> printf "Cash was used"
               | Debit -> printf "Debit was used"
               | Credit -> printf "Credit was used"
               | _ -> "Error" //What happens if we add `GiftCard` to  the `PaymentMethods` union type?
             ```
              
       1. Binding to value
          ```fsharp
          let x =
            match ("a", "b") with
            | (y, "b") -> printfn "y=%0" y
            | ("a", z) -> printfn "z=%0" z
          ```
       2. Logical
          ```fsharp
          let validatePaymentMethodForCashBack (paymentMethod, overpaid, overpaidEnabled) =
            match (paymentMethod, overpaid, overpaidEnabled) with
            | (_ , false, _ ) -> true
            | (Cash, true, _ ) | (Debit, true, true) -> true
            | (x, true, false) & ((Credit, _ , _ ) | (Debitit, _ , _ ))       //Note a single `&` is used
                -> failwith (sprintfn "%A is not configured for overpayments x)
          ```
       3. On lists
          - Lists can be matched explicitly in the form [x;y;z] or in  the “cons” form head::tail.
          ```fsharp
          let y =
            match [1; 2; 3] with
            | [1;x;y] -> printfn "x=%i y=%i" x y      //Square brackets needed
            | 1::tail -> printfn "tail=%A" tail       //No square brackets
            | [] -> printfn "empty"
          ```
          ```fsharp
          let rec loopAndSum aList sumSoFar = 
            match aList with
            | [] -> 
                sumSoFar
            | x::xs -> 
                let newSumSoFar = sumSoFar + x
                loopAndSum xs newSumSoFar
          ```
       4. On Tuples
          ```fsharp
          let aTuple = (32, false)
          match aTuple with
          | (32, _ ) -> printfn "Matched in 32"
          | (_ , true) -> printfn "Matched in true"
          | (_ , _ ) -> printfn "Something else"
          ```
          
       5. On Record
          ```fsharp
          type Customer = {FirstName:string; LastName:string}
          let aCustomer = {FirstName:"Clinton"; LastName:"Adams"}
          match aCustomer with
          | {LastName="Adams"} -> printfn "It is an Adams"
          | {FirstName="Adam"} -> printfn "It's name is Adam"
          | _ -> printfn "No Adam in either name"
          ```
          
       6. On Union
          ```fsharp
          type PaymentType = Cash of decimal | Debit of (string, string, decimal)
          let aPayment = Debit ("Name On Card", "1234-4321")
          match aPayment with
          | Cash amount -> sprintfn "Payment of %d was cash" amount
          | Debit (name, number, amount) ->
              sprintfn "Payment of %d was debit by %s with %s" amount name number
          ```
              
       7. `as`
          ```fsharp
          let x =
            match ("john doe", "1234-4321", 30.32) with
            | (x, y, z) as debitPayment ->
              sprintfn "Payment of %d was debit by %s with %s" z x y
              sprintfn "Using the whole as %A" debitPayment
          ```
              
       8. On Subtypes (Code smell)
          ```fsharp
          let x = new Object()
          let y =
            match x with
            | :? System.Int32 ->
                printfn "its an int"
            | :? System.String ->
                printfn "its a string"
            | _ -> printfn "its something else"
          ```
                
       9. On multiple values
          - Is not allowed. But the values can be inserted into a tuple
          ```fsharp
          let matchOnCustomerAndPaymentType customer paymentType =
            match (customer, paymentType) with
            | ({LastName="Adams"}, pT) ->
              printfn "Adams paid with %A" pT
            | (c, Cash _ ) ->
              printfn "Cash was paid by %A" c
            | _ -> printfn "Something else happened"
          ```
              
       10. Guards
           - `when` is used for Guards           
           - Guards can be used for all sorts of things that pure  patterns can’t be used for, such as:
             * Comparing the bound values
             * Testing object properties
             * Different kinds of matching, such as regular expressions
             * Conditionals derived from functions
           * Comparing the bound values
           ```fsharp
           let elementsAreEqual aTuple = 
             match aTuple with 
             | (x,y) -> 
                 if (x=y) then printfn "They are the same" 
                 else printfn "They are different" 
                 
           let elementsAreEqual aTuple = 
             match aTuple with 
             | (x,y) when x=y -> 
                 printfn "They are the same" 
             | _ ->
                 printfn "They are different"
           ```
           * Testing object properties
           ```fsharp
            type Payment = {Cost:decimal; Payment:decimal}
           ```
           ```fsharp
           let isOverpayment payment =
             match payment:Payment with
             | x when x.Payment > x.Cost -> trye
             | _ -> false
           ```
            
           * Different kinds of matching, such as regular expressions
           ```fsharp
           let classifyString aString = 
             match aString with 
             | x when Regex.Match(x,@".+@.+").Success-> 
                 printfn "%s is an email" aString
             | _ -> 
                 printfn "%s is something else" aString
           ```
                
           * Conditionals derived from functions
           ```fsharp
           let costInString x =
             match x with
             | x when x = 0 -> printfn "its free"
             | x when x > 100 -> printfn "its expensive!"
             | x when x < 0 -> printfn "something is fishy"
             | _ -> "I guess its ok"
           ```
       11. `function`
           ```fsharp
           let f aValue =
             match aValue with
             | pattern1 -> expression1
             | pattern2 -> expression2
           let f =
             function
             | pattern1 -> expression1
             | pattern2 -> expression2
           ```
           ```fsharp
           // using match..with
           [1..10] |> List.map (fun i ->
                   match i with 
                   | 1 | 2 | 3 | 5 | 7 -> sprintf "%i is prime" i
                   | _ -> sprintf "%i is not prime" i
                   )
 
           // using function keyword
           [1..10] |> List.map (function 
                   | 1 | 2 | 3 | 5 | 7 -> sprintf "prime"
                   | _ -> sprintf "not prime"
                   )
           ```
 
       12. recursive
           ```fsharp
           type Contact = 
               | EmailAddress of string
               | PhoneNumber of string
   
           type Member = {
               FirstName:string
               LastName:string
               Age:byte
               Contact:Contact }
   
           let (|Adult|Minor|) age =
               if age >= 18uy then Adult else Minor
   
           let adultsPhone member' =
               match member' with
               | {Age = Adult; Contact = PhoneNumber phone } -> Some    phone
               | _ -> None
           ```
 
    2. let
       - There are two uses of `let`:
         * Top level: a named expression at the top lovel of a module;  analog to methods.
         * Local: used in the context of some expression.
         
       - It can use patterns directly
           ```fsharp
           let anInt = 1
           ```
           ```fsharp
           let aPerson = {FirstName:"Ann"; LastName:"Robinson"}
           ```
       - In functions with parameters
           ```fsharp
           let multiply (a,b) = a * b
           ```
        
    3. Active Patterns
       - Dynamically parse or detect a pattern.
         ```fsharp
         let (|Int|_|) str =
           match System.Int32.TryParse(str) with
           | (true,int) -> Some(int)
           | _ -> None
         let isInt str = 
           match str with
           | Int i -> true
           | _ -> false
         isInt "3"
         isInt "Three" 
         ```
    
11. Asynchronous programming or asynchronous workflows
    - They are objects that encapsulate a background task providing several operations
      ```fsharp
      let timerAsync =
        let timer = new Timer(1000m)
        let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore
        
        timer.Start()
        
        Async.RunSynchronously timerEvent
      ```
        
      * `Async.AwaitEvent` -> Creates an `async` object directly from the event.
      * `Async.Ignore` -> Ignores the result
      * `Async.RunSynchronously` -> blocks on the async object until it has completed
    - Note: async workflows can be used with `IAsyncResult`, begin/end, and other .NET methods
    
    - Manually creating async workflows.
      - Use `async` keyword and curly braces and inside a set of expressions to be executed in the background.
      
        ```fsharp
        let sleepWorkflow = async {
          do! Async.Sleep 1000
        }
        Async.RunSynchronously sleepWorkflow
        ```
        
    - Nested workflows
      - Within the braces, nested worflows can be blocked on by using `let!`, `do!`, `use`
        ```fsharp
        let calculatePrices
          (calculateTax: (CompanyId * InvoiceId) -> Async<TaxCalculation>)
          (getProductPricing: CompanyId * CatalogItemId list -> Async<ProductsPricing>)
          (companyId, invoiceId, catalogIds)
          : Async<Sale>
          = async{
            let! calculateTaxWorkflow =
              calculateTax (companyId, invoiceId)
              |> Async.StartChild
            let! getProductPricingWorkflow =
              getProductPricing (companyId, catalogIds)
              |> Async.StartChild
            let calculation = calculateTaxWorkflow
            let productPricing = getProductPricingWorkflow
            
            return Sale {Taxes = calculation
                         Pricing = pricing}
          }
          Async.RunSynchronously calculatePrices 
        ```
          
      - Cancelling workflows
        - Use the `CancellationToken` class. Any nested async call will check the cancellation token automatically.
          ```fsharp
          let cancellationSource = new CancellationtokenSource()
          Async.Start (calculationPrices, cancellationSource.Token)
          cancellationSource.Cancel()
          ```
          
      - Serial workflows
          ```fsharp
          let calculatePricesInSeries = async {
            let! calculatePricesFirst =
              calculatePrices calculateTax getProductPricing (1234, "invoice1", [1, 2, 3])
            let! calculatePricesAfter =
              calculatePrices calculateTax getProductPricing (1234, "invoice2", [11, 12, 13])   
          }
          Async.RunSynchronously calculatePricesInSeries
          ```
          
      - Parallel workflows
          ```fsharp
          let calculatePrices1 =
              calculatePrices calculateTax getProductPricing (1234, "invoice1", [1, 2, 3])
          let calculatePrices2 =
            calculatePrices calculateTax getProductPricing (1234, "invoice2", [11, 12, 13])
            
          [calculatePrices1; calculatePrices2]
          |> Async.Parallel
          |> Async.RunSynchronously
          ```
