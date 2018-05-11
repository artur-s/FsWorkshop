// ==============================
// Excercises
// ==============================

(*
Quick cheat-sheet

Operators
* "=" is used instead of "=="
* "<>" is used instead of "!="
* "not" is used instead of "!"
* In parameters, commas are replaced by whitespace
* In non-parameter usage (eg lists), commas are replaced by semicolons in most places. 
*)


(* 0. Reading a type signature of a function.
 When using pure functions, the type signature of a function tells a lot about what the function does.
 Try to think of what each of the below functions does and rename it
 f1: ('a -> bool) -> 'a list -> a' list
 f2: ('a -> bool) -> 'a list -> int
 f3: ('a -> 'b) -> 'a list -> 'b list
 f4: ('a -> 'b -> 'c) -> 'a list -> 'b list -> 'c list
*)


// 1. Create a "Hello world" function that introduces yourself. Where yourself is a parameter
//    And run it
let helloWorld name = printfn "Hello world! My name is %s" name
helloWorld "juanito"

// 2. Re-implement this function partialy applying `printfn`
let helloWorld2 = printfn "Hello world! My name is %s"
helloWorld2 "anita"


// 3. Use piping to pass the name into the function.
"anita" |> helloWorld2


// 4. Create a composition function made from a greeting and a response.
let greeting name = printfn "Hello world! my name is %s" name
                    name
let response name = printfn "Hello %s, welcome to the world" name
let result = greeting >> response
result "jonas"


// 5. Create a type for Customer where each property is also a custom type.
//    Try to use Record, aliases, tuples.
//    Create a function for construction and one for deconstruction.

open System
type CustomerId = CustomerId of Guid
type Age = int
type String50 = private String50 of string
    module String50 =
        let create (s:string) = 
            if String.IsNullOrEmpty(s) then 
                None               
            else if (s.Length <= 50) then
                Some (String50 s)
            else None
        let value (String50 s) = s

type Name = {
  FirstName: String50
  LastName: String50
}    
type Customer = {
  Id : CustomerId
  Name: Name
  Age: Age
}

let aCustomer =
  let name = {
    FirstName = String50 "Anna"
    LastName = String50 "Collin"
  }
  {
    Id = CustomerId (Guid.NewGuid())
    Name = name
    Age = 32
  }
aCustomer
let deconstruct =
  let {FirstName = fName; LastName = lName } = aCustomer.Name
  let {Id = id; Age = age} = aCustomer
  printfn "The person name %s %s" (string fName) (string lName) 
  printfn "Customer %A, is %d years old" id age
deconstruct

// 6. Add a discrimanted union to your model. i.e. customer type, contact mode, etc.
//    Update its construction and deconstruction functions.

type EmailAddress = private EmailAddress of string
module EmailAddress =
    let create (s:string) = 
        if String.IsNullOrEmpty(s) then 
            None               
        else if s.Contains("@") then
            Some (EmailAddress s)
        else None
    let value (EmailAddress s) = s

type AddressInfo = {
    Address1: String50
    Address2: String50
    PostalCode: String50
    Country: String50 
    }  

type ContactInfo = 
    | EmailOnly of EmailAddress
    | AddrOnly of AddressInfo
    | EmailAndAddr of EmailAddress * AddressInfo

type EnhanchedCustomer = {
  Id : CustomerId
  Name: Name
  Age: Age
  Contact: ContactInfo
}

let aCustomer2 =
  let name = {
    FirstName = String50 "Anna"
    LastName = String50 "Collin"
  }
  {
    Id = CustomerId (Guid.NewGuid())
    Name = name
    Age = 32
    Contact = EmailOnly (EmailAddress "aja@aja.com")
  }
aCustomer2
let deconstruct2 =
  let {FirstName = fName; LastName = lName } = aCustomer2.Name
  let {Id = id; Age = age; Contact = contact} = aCustomer2
  printfn "The person name %s %s" (string fName)(string lName) 
  printfn "Customer %A, is %d years old" id age
  printfn "Contact info is %A" contact
deconstruct2

// 7. Add or modify a property with an option type to it. i.e. Middle name, phone number, etc.
//    Update its construction and deconstruction functions.
type FullName = {
  FirstName: String50
  MiddleName: String50 option
  LastName: String50
}   
type EnhanchedCustomer2 = {
  Id : CustomerId
  Name: FullName
  Age: Age
  Contact: ContactInfo
}
let aCustomer3 =
  let name = {
    FirstName = String50 "Anna"
    MiddleName = Some (String50 "Marie")
    LastName = String50 "Collin"
  }
  {
    Id = CustomerId (Guid.NewGuid())
    Name = name
    Age = 32
    Contact = EmailOnly (EmailAddress "aja@aja.com")
  }
aCustomer3
let deconstruct3 =
  let {FirstName = fName; MiddleName = mName; LastName = lName } = aCustomer3.Name
  let {EnhanchedCustomer2.Id = id; Age = age; Contact = contact} = aCustomer3
  printfn "The person name %s %s %s" (string fName) (string mName) (string lName) 
  printfn "Customer %A, is %d years old" id age
  printfn "Contact info is %A" contact
deconstruct3

// 8. Create an interface for calculating age for the customer. Try creating it both as object expression or as independent interface.
//    Implement it.


// 9. Create a type for payment methods, and one type for payment.
//    Create a "pay" function that takes a payment and decides how to process it.
(* The payment taking system should accept the followinhg payment methods
* Cash
* Credit cards
* Cheques
* Paypal
* Bitcoin

For cash, no extra info is required
For cheques, a check number (int) is required
For cards, a card number and card type is required
  * A card type is one of Visa, Mastercard, Amex.
For paypal, an email address is required
for bitcoin, a bitcoin address is required

A payment consists of:
* a payment method
* a payment amount

A payment amount is a non-negative decimal

After designing the types, define the types of functions that will:

* print a payment method
* print a payment, including the amount
* create a new payment from an decimal amount and a payment method
*)
//    Hint: use pattern matching.


// 10. Expand this pattern matching function using guards. i.e. when cheque number finishes in odd or even, print different message.


// 11. Create an active pattern for odds and even
//     Create a function isDivisibleByTwo function that uses pattern matching to decide.