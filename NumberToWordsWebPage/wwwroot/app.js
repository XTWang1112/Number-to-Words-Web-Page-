const form = document.getElementById('converter-form');
const errorMessage = document.getElementById("amount-error");
const inputWrapper = document.querySelector(".input-wrapper");
const amountInput = document.getElementById("amount");
const resultText = document.getElementById("result");
const resultSection = document.getElementById("result-section");
const button = document.getElementById("convert-button");
const copyButton = document.getElementById("copy-button");
const copyStatus = document.getElementById("copy-status");

form.addEventListener("submit", async (event) => {
    event.preventDefault();
    
    resetError();
    resetResult();

    const inputValue = amountInput.value.trim();

    if (!inputValue) {
        displayError("Please enter a number.");
        return;
    }

    setLoading(true);

    try {
        const response = await fetch("/api/number-to-words", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ Value: inputValue })
        });

        const responseBody = await response.json();

        if (!response.ok){
            displayError( responseBody.error ?? "Unable to convert the value");

            return;
        }

        displayWords(responseBody.words);
    }
    catch(error){
        console.error("Failed to convert: ", error);
        displayError("Something went wrong, Please try again.");
    }
    finally {
        setLoading(false);
    }
})

copyButton.addEventListener("click", async () => {
    const result = resultText.textContent.trim();

    if (!result) {
        return;
    }

    try {
        await navigator.clipboard.writeText(result);

        copyButton.textContent = "Copied!";
        copyStatus.textContent = "Result copied to clipboard.";

        setTimeout(() => {
            copyButton.textContent = "Copy";
            copyStatus.textContent = "";
        }, 2000);
    }
    catch (error) {
        console.error("Unable to copy result:", error);

        copyStatus.textContent =
            "Unable to copy the result. Please copy it manually.";
    }
});

function resetError() {
    errorMessage.textContent = '';
    errorMessage.hidden = true;
    inputWrapper.classList.remove("has-error");
    amountInput.removeAttribute("aria-invalid");
}

function resetResult() {
    resultText.textContent = "";
    resultSection.hidden = true;
    copyStatus.textContent = "";
    copyButton.textContent = "Copy";
}

function displayError(message) {
    errorMessage.textContent = message;
    errorMessage.hidden = false;
    inputWrapper.classList.add("has-error");
    amountInput.setAttribute("aria-invalid", "true");
    amountInput.focus();
}

function displayWords(words){
    resultText.textContent = words;
    resultSection.hidden = false;
    copyButton.textContent = "Copy";
    copyStatus.textContent = "";
}

function setLoading(isLoading) {
    button.disabled = isLoading;

    button.textContent = isLoading ? "Converting..." : "Convert";
}