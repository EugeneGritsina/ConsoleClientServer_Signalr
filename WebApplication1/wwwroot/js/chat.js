"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});



//connection.on("sayWhoIsTyping", function (name, message) {
//    $('#isTyping').html('<em>' + name + ' is typing ' + message + '</em >');
//});


//const messageInput = document.getElementById('messageInput');

//messageInput.addEventListener('input', updateValue);

//function updateValue(text) {
//    if (text.which == 13) {
//        $('#sendButton').trigger('click');
//    } else {
//        var message = text.target.value;
//        connection.invoke("isTyping", $('#userInput').val(), message);
//    }
//}

messageInput.onkeydown = function (letter) {
    if (letter.code == "Backspace")
        connection.invoke("DeleteLetter", document.getElementById("userInput").value);
    else {
        connection.invoke("AddLetter", letter.key);
    }
}

connection.on("AddLetter", function (letter) {
    $('#isTyping').append('<em>' + letter + '</em >');
});


connection.on("DeleteLetter", function (user) {
    document.getElementById("isTyping").lastChild.remove();
})
