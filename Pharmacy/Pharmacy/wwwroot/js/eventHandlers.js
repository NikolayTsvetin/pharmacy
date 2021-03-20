function onDelete(id, isDeleteClicked) {
    const deleteSpan = `deleteSpan_${id}`;
    const confirmDeleteSpan = `confirmDeleteSpan_${id}`;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}