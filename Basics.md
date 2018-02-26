- Basics: 
      - function signature
        - Values. 
          - Signature. val aName: type = constant.
          
          - The "let" keyword defines an (immutable) value. No types needed
          - let anInt = 3
          - let aString = "Hola"
          - let aList = [1;2;3]
        - Functions. Parameters are in no need of parenthesis, but it needed for precedence.
          - Signature
          - val functionName : domain -> range
          
          - let square x = x * x
            square 3
          - let add x y = x + y
            add 1 2
          - Multiline functions. Indentation is important.
            - let odds list =
                let isOdd x = x%2 = 1
                List.filter isOdd list
              odds aList
              
        - There is very little difference between simple values and function values. 
        - One of the key aspects of thinking functionally is: 
          functions are values that can be passed around as inputs to other functions.
        
        
      - Currying
        - A function with multiple parameters is rewritten as a series of new functions, each with only one parameter.
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
        - Fixing the first N parameters of the function, gets a function of the remaining parameters.
        
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
            - let (|>) x f = f x
          
          - It allows to put the function argument in front of the function rather than after.
          - You can pipe the output of one operation to the next using "|>"
          - From Partial application ordering, having the data structure at the end
            makes it easier to pipe a structure or collection from function to function. 
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

        
