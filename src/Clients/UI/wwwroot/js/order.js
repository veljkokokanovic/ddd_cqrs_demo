"use strict";

var opts = {
    lines: 13, // The number of lines to draw
    length: 38, // The length of each line
    width: 17, // The line thickness
    radius: 45, // The radius of the inner circle
    scale: 1, // Scales overall size of the spinner
    corners: 1, // Corner roundness (0..1)
    color: '#000000', // CSS color or array of colors
    fadeColor: 'transparent', // CSS color or array of colors
    speed: 1, // Rounds per second
    rotate: 0, // The rotation offset
    animation: 'spinner-line-fade-quick', // The CSS animation name for the lines
    direction: 1, // 1: clockwise, -1: counterclockwise
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    className: 'spinner', // The CSS class to assign to the spinner
    top: '50%', // Top position relative to parent
    left: '50%', // Left position relative to parent
    shadow: '0 0 1px transparent', // Box-shadow for the lines
    position: 'absolute' // Element positioning
};
var spinner = new Spinner(opts);
const orderCmdUrl = 'http://localhost:9000/api/orders';
var connection = new signalR.HubConnectionBuilder().withUrl("/commandnotify").build();
var correlationIds = [];



connection.on("CommandMessage", function (message, user) {
    if (message.success) {
        correlationIds.splice($.inArray(message.CorrelationId, correlationIds), 1);
        if (correlationIds.length == 0) {
            spinner.stop();
            location.reload();
        }
    }
    else {
        alert('command failed');
    }
});

connection.start().then(function () {
    console.debug("connected to hub");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("newPizza").addEventListener("change", function (event) {
    var newPizza = document.getElementById("newPizza");
    if (newPizza.value != "") {
        var data = {
            orderId: orderId,
            quantity: 1,
            sku: newPizza.value
        };

        $.ajax({
            url: orderCmdUrl,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json;domain-model=AddProductToOrderViewModel",
            success: function (data, textStatus, request) {
                var correlationId = request.getResponseHeader('X-CorrelationID');
                correlationIds.push(correlationId);
                var target = document.getElementById('spinner');
                spinner.spin(target);
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        });
    }
   
    event.preventDefault();
});


document.getElementById("newDrink").addEventListener("change", function (event) {
    var newDrink = document.getElementById("newDrink");
    if (newDrink.value != "") {
        var data = {
            orderId: orderId,
            quantity: 1,
            sku: newDrink.value
        };

        $.ajax({
            url: orderCmdUrl,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json;domain-model=AddProductToOrderViewModel",
            success: function (data, textStatus, request) {
                var correlationId = request.getResponseHeader('X-CorrelationID');
                correlationIds.push(correlationId);
                var target = document.getElementById('spinner');
                spinner.spin(target);
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.status + ': ' + xhr.statusText
                alert('Error - ' + errorMessage);
            }
        });
    }

    event.preventDefault();
});