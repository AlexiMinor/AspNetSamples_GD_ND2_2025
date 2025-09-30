let clickCount = 0; // Initialize click counter

async function testClick() {
    //let testBlock = document.getElementById("test"); // Get the test block element
    //testBlock.style.backgroundColor = "lightblue"; // Change background color
    //clickCount++; // Increment click counter
    //generateSomeText(clickCount); // Call function to generate some text

    //console.log(`generateSomeText started`);
    //generateSomeText(clickCount)
    //    .then((result) => {
    //        console.log(`Text block with ID newParagraph-${result} has been generated`);
    //    });
    //console.log(`testClick finished`);


    //actual equivalent of async/await
    //let result = await generateSomeText(clickCount);
    //console.log(`Text block with ID newParagraph-${result} has been generated`);
    //console.log(`testClick finished`);
    await checkAvailableArticlesOnResource();
}


//async function generateSomeText(clickNumber) {
//    let testBlock = document.getElementById("test"); // Get the test block element
//    let newParagraph = document.createElement("p"); // Create a new paragraph element
//    newParagraph.id = `newParagraph-${clickNumber}`; // Set an ID attribute
//    newParagraph.textContent = "This is a new paragraph."; // Set the text content
//    console.log(`paragraph has been prepared, delay started`);

//    setTimeout(() => {
//        testBlock.appendChild(newParagraph); // Append the new paragraph to the test block

//        if (clickNumber === 5) {
//            //alternative way to add event listener
//            newParagraph.addEventListener("click", function () {
//                alert("Paragraph 5 clicked!");
//            });
//        }
//        console.log(`paragraph has been added to the DOM`);
//    }, 3000);

//    return clickNumber;
//}

async function checkAvailableArticlesOnResource() {
    await fetch("api/articles/count")
        .then((response) => {
            let data = response.json().then((result) => {
                console.log(`Data received from /api/articles:`, result);
                console.log(result.count);
                generateArticleStats(result.count);
            })
        })
        .catch(error => {
            console.error("Error fetching articles:", error);
            return null;
        });
}

function generateArticleStats(count) {
    console.log('genStarted');
    let testBlock = document.getElementById("test");
    let statsParagraph = document.createElement("p");
    statsParagraph.textContent = `Number of articles: ${count}`;
    testBlock.appendChild(statsParagraph);
}1