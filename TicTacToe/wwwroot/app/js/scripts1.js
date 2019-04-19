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

function GameInvitationConfirmation(id) {
    if (window.WebSocket) {
        alert("Gniazda WebSocket są aktywne");
        openSocket(id, "GameInvitation");
    }
    else {
        alert("Gniazda WebSocket nie są aktywne");
        interval = setInterval(() => {
            CheckGameInvitationConfirmationStatus(id);
        }, 5000);
    }
}