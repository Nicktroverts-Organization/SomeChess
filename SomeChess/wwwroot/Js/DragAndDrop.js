var selectedElementID = "";
var droppedFieldId = "";

window.getClickedElementID = function (e)
{
    selectedElementID = e.target.id;
    return selectedElementID;
}
window.getMouseUpElementID = function (e) {
    droppedFieldId = e.target.id;
    return droppedFieldId;
}
window.getSelected = function  ()
{
    return selectedElementID;
}
window.getDropped = function() {
    return droppedFieldId;
}
window.MovePiece = function (from, to) {
    console.log(from);
    console.log(to);
    var element = document.getElementById(to);
    element.innerHTML = document.getElementById(from).innerHTML;

    document.getElementById(from).innerHTML = "";
}