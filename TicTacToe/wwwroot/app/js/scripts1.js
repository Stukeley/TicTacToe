let interval;

function EmailConfirmation(email) {
    if (window.WebSocket) {
        openSocket(email, "Email");
    }
    else {
        interval = setInterval(() => {
            CheckEmailConfirmationStatus(email);
        }, 5000);
    }
}