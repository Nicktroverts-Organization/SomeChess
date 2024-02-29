window.BlazorDragAndDrop = {
    DragStart: function (e) {
        e.preventDefault();
        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/plain', e.target.id);
    },
    DragEnd: function (e) {
        e.preventDefault();
    },
    DragOver: function (e) {
        e.preventDefault();
    },
    Drop: function (e) {
        e.preventDefault();
        var data = e.dataTransfer.getData('text/plain');
        var draggedElement = document.getElementById(data);
        e.target.appendChild(draggedElement);
    }
};