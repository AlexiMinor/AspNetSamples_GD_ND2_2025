//cast to string

let num = 1;

console.log(1);
console.log(String(num));
console.log(num.toString());

console.log(typeof (num));
//alert(num);

let a = 1 + "";
let b = 1 + "1";
let c = "2";

console.log(a, typeof(a));
console.log(b, typeof(b));
console.log(c, typeof (c));

let variable = prompt("Enter value for calculation:");
console.log("variable+variable", variable + variable);


//cast to number
let result1 = variable * 1;
console.log(result1, typeof (result1));

let result2 = variable - 1;
console.log(result2, typeof (result2));

let result3 = variable / 2;
console.log(result3, typeof (result3));
console.log("variable+variable", Number(variable) + Number(variable));

let isTrue = true;
console.log("true", Number(isTrue));

let isFalse = false;
console.log("false", Number(isFalse));

let und;
console.log("undefined", Number(und));

let n = null;
console.log("null", Number(n));

let str = "   15 \n";
console.log("str", Number(str));

let emptyStr = "";
console.log("emptyStr", Number(emptyStr));


//cast to boolean
let x = null;// undefined, NaN, 0, "" => false
//otherwise true
