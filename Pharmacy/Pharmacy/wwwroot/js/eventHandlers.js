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

function onSearch() {
    const searchInputValue = $('#searchInput').val();

    if (searchInputValue && searchInputValue.length > 0) {
        $('#searchButton').attr("disabled", false);
    } else {
        $('#searchButton').attr("disabled", true)
    }
}