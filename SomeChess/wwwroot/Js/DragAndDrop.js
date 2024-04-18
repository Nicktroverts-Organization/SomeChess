var selectedElementID = "";
var droppedFieldId = "";
var elementUnderMouse = "";
var dotNet = "";
var gameOver = false;

window.setDotNetReference = function (dotNetReference) {
    dotNet = dotNetReference;
}

window.Over = function (_gameOver) {
    console.log("GameOver: ", _gameOver);
    gameOver = _gameOver;
}

window.getMouseDownID = function (e) {
    console.log("mousedown");
    console.log(e);
    if (!gameOver) {
        var draggedElement = document.getElementById(e.id);
        selectedElementID = draggedElement.id;
        dotNet.invokeMethodAsync('IdHandler', selectedElementID, "mousedown");
        return selectedElementID.id;
    }
    return null;
}
window.getMouseUpElementID = function (e) {
    console.log("mouseup");
    console.log(e);

    if (!gameOver) {
        var position = document.elementsFromPoint(e.pageX, e.pageY);
        position.forEach((elt) => {
            if (elt.hasAttribute('id')) {
                droppedFieldId = elt.id;
                console.log(elt);

            }
        });
        dotNet.invokeMethodAsync('IdHandler', droppedFieldId, "mouseup");
        return droppedFieldId;
    }
    return null;
}
window.getSelected = function () {
    return selectedElementID;
}
window.getDropped = function () {
    return droppedFieldId;
}
window.MovePiece = function (from, to) {
    var fromElement = document.getElementById(from);
    var toElement = document.getElementById(to);
    toElement.innerHTML = fromElement.innerHTML;
    fromElement.innerHTML = "";
}

window.addEventListener("dragover", function (event) {
    event.preventDefault();
});

window.addEventListener('mousemove', function (event) {
    if (window.draggingElement) {
        var offsetX = event.clientX - 37;
        var offsetY = event.clientY - 37;
        window.draggingElement.style.left = offsetX + 'px';
        window.draggingElement.style.top = offsetY + 'px';
    }
});

window.addEventListener('mouseup', function (event) {
    window.draggingElement = null;
});

window.addEventListener('dragstart', function (event) {
    window.draggingElement = event.target;
    event.target.offsetX = event.clientX - 37;
    event.target.offsetY = event.clientY - 37;
    event.target.style.position = 'absolute';
});


function SetMoveHistoryScroll() {
    var objDiv = document.getElementById("move-history");
    if (objDiv != null) {
        objDiv.scrollTop = objDiv.scrollHeight;
    }
}