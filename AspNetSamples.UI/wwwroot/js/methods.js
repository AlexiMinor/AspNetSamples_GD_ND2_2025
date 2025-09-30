
let userName = "John";

let obj = { A: 20, B: "hello" };

greetUser();
let sum = calculateSum(2, 6, 10, 15,);

console.log(sum);
console.log(obj);

function calculateSum(a, b) {
    let sum = a + b;
    return sum;
}

function displayUser() {
    obj.B = obj.B + " world";
}

console.log(obj);


function greetUser() {
    userName = "Alice";
    console.log("Hello, " + userName + "!");
}

console.log(userName);


//let calculateSum2 = function () {
//    let a = 10;
//    let b = 20;
//    let sum = a + b;
//    console.log("Sum:", sum);
//}

//calculateSum();
//calculateSum2();

//console.log(calculateSum);
//console.log(calculateSum2);


//calculateSum = function () {

//    let a = 100;
//    let b = 200;
//    let sum = a + b;
//    console.log("New Sum:", sum);

//}

//calculateSum();


//calculateSum = 15;
//console.log(calculateSum);
//calculateSum();


//arrow function
let calculateSum2 = (a, b) => {
    let sum = a + b;
    return sum;
}

let sumResult = (a, b) => a + b;
console.log(sumResult(5, 10));