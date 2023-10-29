/* ------- Intersection Observer --------- */

const flashcards = document.querySelectorAll('.flashcard-example-content');
const flashcardContentObserver = new IntersectionObserver((entries) => {
    // add an animation when the flashcard example content section is revealed
    entries.forEach((entry) => {
        if (entry.isIntersecting) {
            entry.target.classList.add('flashcard-example-animation')
        }
    });
}, {
    threshold: 0.01
})
flashcards.forEach(flashcard => {
    flashcardContentObserver.observe(flashcard);
});

const shape = document.querySelectorAll('.shape');
const shapeContentObserver = new IntersectionObserver((entries) => {
    // every thime the shapes from the home button is seen, add animation and remove animation if not
    entries.forEach((entry) => {
        if (entry.isIntersecting) {
            entry.target.classList.add('shape-animation')
        } else {
            entry.target.classList.remove('shape-animation')
        }
    });
}, {
    threshold: 0.01
})
shape.forEach(flashcard => {
    shapeContentObserver.observe(flashcard);
});

/* -------------------- Flashcard one --------------------------*/
if (window.location.pathname === '/Flashcard/FlashcardTypeOne') {
    const carousel = document.getElementById('flashcard-default');
    const prevButton = document.getElementById('prev');
    const nextButton = document.getElementById('next');
    const restartButton = document.getElementById('restart');
    const progressBar1 = document.getElementById('progress-bar1');
    const carouselItems = document.querySelectorAll('.carousel-item');


    // for flipping the card
    function rotateCard(card) {
        card.classList.toggle("rotate-180");
        carousel.addEventListener('slid.bs.carousel', function () {
            card.classList.remove("rotate-180");
        });
    }

    // for disabling buttons 
    function updateButtonsState() {
        currentIndex = Array.from(carouselItems).indexOf(document.querySelector('.carousel-item.active'));

        // Disable the "Previous" button if on the first flashcard
        prevButton.disabled = currentIndex === 0;

        // Check if on the last flashcard
        if (currentIndex === carouselItems.length - 1) {
            nextButton.disabled = true;
            isEndReached = true;
            restartButton.style.display = 'block'; // Show the restart button
        } else {
            nextButton.disabled = false;
            isEndReached = false;
            restartButton.style.display = 'none'; // Hide the restart button
        }

        // Update the progress bar
        // Calculate the progress while excluding the first carousel item
        const progress1 = ((currentIndex) / (carouselItems.length - 1)) * 100;
        progressBar1.style.width = progress1 + '%';
        progressBar1.setAttribute('aria-valuenow', progress1);
    }

    function restartCarousel() {
        isEndReached = false;
        document.querySelector('.carousel-item.active').classList.remove('active');
        carouselItems[0].classList.add('active');
        updateButtonsState();
    }

    document.addEventListener('slid.bs.carousel', updateButtonsState);
    // only use the updatebuttonstate function when a flashcard type is loaded
    updateButtonsState();
}
/*----------------------flashcard two ------------------------ */
if (window.location.pathname === '/Flashcard/FlashcardTypeTwo') {
    document.addEventListener('DOMContentLoaded', function () {
        let selectedElement;
        let correctMatches2 = 0;

        // Add event listeners to question elements
        const questions = document.querySelectorAll('.question');
        questions.forEach((question) => {
            question.addEventListener('click', function (event) {
                if (selectedElement) {
                    // Unselect the previously selected element (reset its background)
                    selectedElement.style.background = "lightblue";
                }
                selectedElement = event.target;
                selectedElement.style.background = "green";
            });
        });

        // Add event listeners to answer elements
        const answers = document.querySelectorAll('.answer');
        answers.forEach((answer) => {
            answer.addEventListener('click', function (event) {
                // if a question is selected first
                if (selectedElement) {
                    if (selectedElement.id + "_target" === event.target.id) {
                        // to remove the correct match question and answer
                        event.target.style.background = 'green';
                        event.target.style.display = 'none';
                        selectedElement.style.display = 'none';

                        document.getElementById('ins').innerHTML = "Correct!!";
                        setTimeout(() => {
                            document.getElementById('ins').innerHTML = "Match the correct Questions and Answers <br /> Select a question first then an answer";
                        }, 1000);

                        // for documenting progress for the progress bar
                        correctMatches2++;
                        const progressBar2 = document.getElementById('progress-bar2');
                        const maxMatches2 = answers.length;
                        const progress2 = (correctMatches2 / maxMatches2) * 100;
                        progressBar2.style.width = progress2 + '%';
                        progressBar2.setAttribute('aria-valuenow', progress2);


                        if (correctMatches2 === maxMatches2) {
                            const finishMessage = document.getElementById('game-finish-message');
                            finishMessage.style.display = 'block';
                        }

                    } else {
                        // when the match is wrong
                        document.getElementById('ins').innerHTML = "WRONG MATCH";
                        event.target.style.background = 'red';
                        selectedElement.style.background = "red";
                        setTimeout(() => {
                            event.target.style.background = 'lightseagreen';
                            selectedElement.style.background = "lightblue";
                            selectedElement = "";
                        }, 200);
                        setTimeout(() => {
                            document.getElementById('ins').innerHTML = "Match the correct Questions and Answers <br /> Select a question first then an answer";
                        }, 1000);
                    }
                    // if an answer is selected first
                } else {
                    document.getElementById('ins').innerHTML = "Please Select a QUESTION First";
                }
            });
        });
    });
}
/* ---------------------------- Flashcard Three  ------------------------------ */

if (window.location.pathname === '/Flashcard/FlashcardTypeThree') {
    let numberRating = null;
    let qualitativeRating = null;
    let explanation = null;

    const numberRatingDict = {
        0: "#cb3a36",
        1: "#e58f2a",
        2: "#f7c045",
        3: "#96bc4b",
        4: "#53c7e0"
    };

    function getAnswer(flashcardid) {
        const submit = document.getElementById("submit_" + flashcardid);
        const status = document.getElementById("status_" + flashcardid);
        const loading = document.getElementById("loading_" + flashcardid);
        const check = document.getElementById("check_" + flashcardid);
        // Get the answer input within the flashcard
        const userInput = document.getElementById(flashcardid).value;
        const question = document.getElementById("question_" + flashcardid).innerHTML;

        if (userInput == " " || userInput.length == 0) {
            document.getElementById("error_" + flashcardid).innerHTML = "write something";
        } else {
            check.style.display = "none";
            loading.style.display = "flex";
            status.style.display = "inline";
            submit.disabled = true;
            document.getElementById("error_" + flashcardid).innerHTML = "";
            fetchData(question, userInput, flashcardid);
        }
    }

    // an asynchronous request to the OpenAI's API 
    async function fetchData(question, input, flashcardid) {
        const submit = document.getElementById("submit_" + flashcardid);
        const status = document.getElementById("status_" + flashcardid);
        const loading = document.getElementById("loading_" + flashcardid);
        const check = document.getElementById("check_" + flashcardid);
        // Hard coding the API key for easier access
        const API_KEY = "";

        const response1 = await fetch("https://api.openai.com/v1/chat/completions", {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${API_KEY}`,
                "Content-Type": 'application/json',
            },
            body: JSON.stringify({
                model: "gpt-3.5-turbo",
                messages: [
                    {
                        role: "system",
                        content: "You are an evaluator assessing the user's response to a flashcard question they have created. Provide a strict evaluation. The number rating should be 0-4, 0 being the worst. The format should be: '(number rating) ||| Rating: (qualitative rating) ||| Reasoning: (explanation).'",
                    },
                    {
                        role: "user",
                        content: `Flashcard Question: ${question} | User's Response: ${input}`,
                    },
                ],
                temperature: 0.7,
                max_tokens: 150,
                top_p: 1,
                frequency_penalty: 0,
                presence_penalty: 0,
            }),
        });

        console.log("getting data...");
        const data = await response1.json();
        console.log(data);

        console.log("formatting data...");

        const content = data.choices[0].message.content;
        const responseString = content.split("|||");
        const rating = responseString[0].trim();
        const evaluation = responseString[1].trim();
        const response = responseString[2].trim();
        console.log("setting vars...");

        // You can return the extracted values or use them as needed in your code
        numberRating = rating;
        qualitativeRating = evaluation;
        explanation = response;

        const instruction3 = document.getElementById("ins3_" + flashcardid);
        const answer3 = document.getElementById("ans3_" + flashcardid);

        if (openaiResponded(numberRating, qualitativeRating, explanation)) {
            submit.disabled = false;
            check.style.display = "inline"
            status.style.display = "none";
            loading.style.display = "none";
            instruction3.style.display = "none";
            answer3.style.display = "flex";
            displayResponse(flashcardid);
        } else {
            check.style.display = "inline"
            submit.disabled = false;
            status.style.display = "none";
            loading.style.display = "none";
        }

    }

    // checks if the global variables  numberRating, qualitativeRating, explanation are null
    // that means that we haven't gotten a response from openai's api
    function openaiResponded(numberRating, qualitativeRating, explanation) {
        // we have not gotten a proper response 
        if ((numberRating == null) || (qualitativeRating == null) || (explanation == null)) {
            return false;
        } else {
            return true;
        }
    }
    function displayResponse(flashcardid) {
        // quality display
        const rating = document.getElementById("rating_" + flashcardid);
        rating.style.background = numberRatingDict[numberRating];

        // Color display
        const quality = document.getElementById("quality_" + flashcardid);
        quality.innerHTML = qualitativeRating;

        // Display OpenAI's response
        const evaluationDiv = document.getElementById("explanation_" + flashcardid); // Use the correct ID here
        evaluationDiv.innerHTML = explanation;

        // after displaying the response from openai's api, "reset" the variables so they are ready for the "openaiResponded"-check
        numberRating = null;
        qualitativeRating = null;
        explanation = null;
    }

    document.addEventListener("DOMContentLoaded", function () {
        const carousel3 = document.getElementById("flashcard3-carousel");
        const progressBar3 = document.getElementById("progress-bar3");
        let currentSlide3 = 0;
        function updateProgressBar() {
            const progress = (currentSlide3 / (carousel3.querySelectorAll(".carousel-item").length - 1)) * 100;
            progressBar3.style.width = progress + "%";
        }
        carousel3.addEventListener("slid.bs.carousel", function () {
            currentSlide3 = Array.from(carousel3.querySelectorAll(".carousel-item")).indexOf(carousel3.querySelector(".carousel-item.active"));
            updateProgressBar();
            updateButtonsState();
        });
        function handleButtonClick(buttonId, increment) {
            document.getElementById(buttonId).addEventListener("click", function () {
                if (currentSlide3 + increment >= 0 && currentSlide3 + increment < carousel3.querySelectorAll(".carousel-item").length) {
                    currentSlide3 += increment;
                    updateProgressBar();
                    updateButtonsState();
                }
            });
        }
        function updateButtonsState() {
            const carouselItems3 = carousel3.querySelectorAll(".carousel-item");
            const prevButton3 = document.getElementById("prev");
            const nextButton3 = document.getElementById("next");

            if (currentSlide3 === 0) {
                prevButton3.disabled = true;
            } else {
                prevButton3.disabled = false;
            }

            if (currentSlide3 === carouselItems3.length - 1) {
                nextButton3.disabled = true;
            } else {
                nextButton3.disabled = false;
            }
        }
        document.addEventListener('slid.bs.carousel', updateButtonsState);
        // only use the updatebuttonstate function when a flashcard type is loaded
        updateButtonsState();

        handleButtonClick("next", 1);
        handleButtonClick("prev", -1);
    });
}
// Function to restart the match game
function restartMatch() {
    location.reload();
}


