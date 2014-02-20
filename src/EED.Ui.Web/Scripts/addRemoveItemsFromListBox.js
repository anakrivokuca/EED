$(function () {
    $('.addAllItems').click(function () {
        $("#DistrictIds option").appendTo("#SelectedDistrictIds");
        $("#SelectedDistrictIds option").prop('selected', false);
    });
});

$(function () {
    $('.addItem').click(function () {
        $("#DistrictIds option:selected").appendTo("#SelectedDistrictIds");
        $("#SelectedDistrictIds option").prop('selected', false);
    });
});

$(function () {
    $('.removeItem').click(function () {
        $("#SelectedDistrictIds option:selected").appendTo("#DistrictIds");
        $("#DistrictIds option").prop('selected', false);
    });
});

$(function () {
    $('.removeAllItems').click(function () {
        $("#SelectedDistrictIds option").appendTo("#DistrictIds");
        $("#DistrictIds option").prop('selected', false);
    });
});

$(function () {
    $("form").submit(function () {
        // Before submitted by the browser modifies the 'selected' options
        $('#SelectedDistrictIds option').prop('selected', true);
    });
});