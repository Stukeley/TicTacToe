function CheckEmailConfirmationStatus(email) {
    $.get("/CheckEmailConfirmationStatus?email=" + email, function (data) {
        if (data === "OK") {
            if (interval !== null) {
                clearInterval(interval);
            }
            window.location.href = "/GameInvitation?email=" + email;
        }
    });
}

function CheckGameInvitationConfirmationStatus(id) {
    $.get("/GameInvitationConfirmation?id=" + id, function (data) {
        if (data.result === "OK") {
            if (interval !== null) {
                clearInterval(interval);
            }
            window.location.href = "/GameSession/Index" + id;
        }
    });
}

var openSocket = function (parameter, strAction) {
    if (interval !== null) {
        clearInterval(interval);
    }

    let protocol = location.protocol === "https:" ? "wss:" : "ws:";

    let operation = "";
    let wsUri = "";

    if (strAction == "Email") {
        wsUri = protocol + "//" + window.location.host + "/CheckEmailConfirmationStatus";
        operation = "CheckEmailConfirmationStatus";
    }
    else if (strAction == "GameInvitation") {
        wsUri = protocol + "//" + window.location.host + "/GameInvitationConfirmation";
        operation = "CheckGameInvitationConfirmationStatus";
    }

    let socket = new WebSocket(wsUri);
    socket.onmessage = function (response) {
        console.log(response);
        if (strAction == "Email" && response.data == "OK") {
            window.location.href = "/GameInvitation?email=" + parameter;
        }
        else if (strAction == "GameInvitation") {
            let data = $.parseJSON(response.data);

            if (data.result == "OK") {
                window.location.href = "/GameSession/Index" + data.id;
            }
        }
    };

    socket.onopen = function () {
        let json = JSON.stringify({
            "Operation": operation,
            "Parameters": parameter
        });
        socket.send(json);
    };

    socket.onclose = function (event) {
    };
};