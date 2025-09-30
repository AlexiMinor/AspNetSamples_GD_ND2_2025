//math operations

//let a = 2;
//let b = 10000000;

//let power = a ** b; //exponentiation
//console.log("Exponentiation:", power, typeof (power));



// comparison operations
let x1 = 10;
let x2 = -10;

console.log("x1 > x2:", x1 > x2);
console.log("x1 < x2:", x1 < x2);
console.log("x1 >= x2:", x1 >= x2);
console.log("x1 <= x2:", x1 <= x2);
console.log("x1 == x2:", x1 == x2);
console.log("x1 != x2:", x1 != x2);


let a = "A";
let z = "Z";
let word1 = "Word";
let word2 = "World";

console.log(a > z);
console.log(word1 > word2);
console.log("Hello" > "hello");
console.log("son" > "sun");

//how string are comparing:
// 1. take first symbol
// 2. if 1st symbol of stringA > 1st symbol of stringB
// 3. If equal => take 2nd symbol
// repeat until difference
// 4. If stringA is finished => longer is bigger

console.log("comparing different types:"); 

let num = 1;
let number = "15";

console.log(number > num); // "15" => Number(15) > 1
console.log('01' == 1); // "01" => 1 == 1


console.log("true == 1", true == 1);
console.log("false == 0", false == 0);

console.log("'1' == 1", '1' == 1);


console.log("undefined == 0", undefined == 0);
console.log("null == 0", null == 0);
console.log("null == undefined", null == undefined);

console.log("null > 0", null > 0);
console.log("null < 0", null < 0);
console.log("null >= 0", null >= 0);


console.log("undefined > 0", undefined > 0);
console.log("undefined < 0", undefined < 0);
console.log("undefined >= 0", undefined >= 0);

//strict equal

console.log(true === 1); // will be true only if types are same & value are same
