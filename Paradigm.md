# Why Functional Programming?

## Why it is worth learning FP?

    - Disruptive change in software development. Functional features are invading imperative languages like C# (e.g. LINQ), C++, Java.
        (TODO: Speaker note: Even Java, the bastion of OOP finally let the lambdas in!)
    - Knowing functional paradigm helps writing more robust code in imperative languages.
        (TODO: speaker note: using data transformation, avoiding side-effects, making types immutable, etc.)
    - Multicore revolution. OO paradigm does not help you with concurrency and parallelism.
        (TODO: Speaker note: OO paradigm encourages dangerous and buggy design. The premise of OO, hiding data (not strictly encapsulation) combined with sharing and mutation leads to race conditions. For example, hiding locks in objects can cause deadlocks.)
    - Complexity of software is growing and shows limits of scalability of imperative design.

## What is it?

    - Separation of data and logic vs. object with mutable state
    - Pure functions and immutable _data types_ vs classes
    - Composition of functions
    - Composition of data structures

## Why it matters?

    - Pure functions (and also pure data structures) are easier to compose
    - Pure functions (data transformations) and immutable data is simple to reason about
    - modeling side-effects with data structures (DSL's), transformed by pure functions and interpreted at the edge of the application.
    - It is [simpler and safer](https://fsharpforfunandprofit.com/posts/is-your-language-unreasonable/)
