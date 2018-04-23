// ==============================
// Excercises
// ==============================

(*
Quick cheat-sheet

Symbols
* "=" is used instead of "=="
* "<>" is used instead of "!="
* "not" is used instead of "!"
* In parameters, commas are replaced by whitespace
* In non-parameter usage (eg lists), commas are replaced by semicolons in most places. 

*)

// 1. Create a "Hello world" function that introduces yourself. Where yourself is a parameter
//    And run it



// 2. Re-implement this function using partial application



// 3. Use piping to pass the name into the function.



// 4. Create a composition function made from a greeting and a response.



// 5. Create a type for Customer where each property is also a custom type.
//    Try to use Record, aliases, tuples.
//    Create a function for construction and one for deconstruction.


// 6. Add a discrimanted union to your model. i.e. customer type, contact mode, etc.
//    Update its construction and deconstruction functions.



// 7. Add or modify a property with an option type to it. i.e. Middle name, phone number, etc.
//    Update its construction and deconstruction functions.



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