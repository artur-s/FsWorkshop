- title : F# Workshop
- description : F# Workshop for iQmetrix developers
- author : Gabe, Artur
- theme : White
- transition : none

***

### F# Workshop

<br />
<br />
Gabriel Martinez
<br />
Artur Siwic

***

### Purpose

Help iQ developers switch to F#

_The best language available for .NET_

***

### Plan

* FP paradigm
* F# Basics
    * Functions
    * Currying and partial application
    * Pipelining
    * Function composition
    * Type system
    * Expressions vs statements
    * Type inference
    * Pattern matching

***

### Plan (continued)

* DDD/ Domain modeling

* Asynchronous programming
* Recursion and beyond
    * Recursive types and functions and immutability
    * Tail recursion
    * Abstracting recursion
    

***

### Plan (continued)

 * Composability
    * Algebraic Data Types (algebra on types)
    * Functional composition building blocks: `map`, `bind`, `fold`, ...
    * Effects: Monadic vs applicative
    * Effectful computations with computation expressions: `result`, `async`, `query` , `cloud`
    * Dependency rejection
 * Type providers
 
***


<!--Paradigm-->
### Why it is worth learning FP?

 * Disruptive change in software development. Functional features are invading imperative languages like C#, C++, Java
 * Knowing functional paradigm helps writing more robust code in imperative languages
 * Multicore revolution. OO paradigm does not help you with concurrency and parallelism
 * Complexity of software is growing and shows limits of scalability of imperative design

' * C# has LINQ (declarative style of programming, that implements list monad). C++ has lambdas. Even Java, the bastion of OOP finally let the lambdas in!    
' * Using data transformation, avoiding side-effects, making types immutable, etc.
' * OO paradigm encourages dangerous and buggy design. The premise of OO, hiding data (not strictly encapsulation) combined with sharing and mutation leads to race conditions. For example, hiding locks in objects can cause deadlocks
' * Concise syntax and simple design are more important now

***

### What is it?

* Separation of data and logic vs. object with mutable state
* Pure functions and immutable _data types_ vs classes
* Composition of functions
* Composition of data structures

*** 

## Why it matters?

* Pure functions (and also pure data structures) are easier to compose
* Pure functions (data transformations) and immutable data is simple to reason about
* modeling side-effects with data structures (DSL's), transformed by pure functions and interpreted at the edge of the application.
* It is [simpler and safer](https://fsharpforfunandprofit.com/posts/is-your-language-unreasonable/)

*** 

<!--Basics-->








### Exercises


*** 

### Thank you!

* Learn all the FP you can!
* Ask questions and discuss on Slack [#funprog]()

