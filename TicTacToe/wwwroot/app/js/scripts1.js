let interval;

function emailConfirmation(email) {
    interval = setInterval(() => {
        CheckEmailConfirmationStatus(email);
    }, 5000);
}