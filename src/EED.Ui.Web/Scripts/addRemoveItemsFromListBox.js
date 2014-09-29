$(function () {
    $('.addAllItems').click(function () {
        $("#ItemIds option").appendTo("#SelectedItemIds");
        $("#SelectedItemIds option").prop('selected', false);
    });
});

$(function () {
    $('.addItem').click(function () {
        $("#ItemIds option:selected").appendTo("#SelectedItemIds");
        $("#SelectedItemIds option").prop('selected', false);
    });
});

$(function () {
    $('.removeItem').click(function () {
        $("#SelectedItemIds option:selected").appendTo("#ItemIds");
        $("#ItemIds option").prop('selected', false);
    });
});

$(function () {
    $('.removeAllItems').click(function () {
        $("#SelectedItemIds option").appendTo("#ItemIds");
        $("#ItemIds option").prop('selected', false);
    });
});

$(function () {
    $("form").submit(function () {
        // Before submitted by the browser modifies the 'selected' options
        $('#SelectedItemIds option').prop('selected', true);
    });
});