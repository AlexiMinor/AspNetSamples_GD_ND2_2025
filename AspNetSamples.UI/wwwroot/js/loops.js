for (var i = 0; i < 10; i++) {
    console.log(i);
}

let arr = [1, 2, 3, 4, 5];
for (let i = 0; i < arr.length; i++) {
    console.log(arr[i]);
}

//stack
while(arr.length > 0) {
    let val = arr.pop();
    console.log(val);
}

do {
    console.log("This will run once");
} while (false);



arr = [10, 20, 30, 40, 50];

//equivalent to foreach in C#
for (let val of arr) {
    console.log(val);
}

let obj = { A: 1, B: 2, C: 3 };
for (let key in obj) {
    console.log(key, obj[key]);
}