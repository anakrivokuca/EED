$(function () {
    $('.cbSelectRow').change(function () {
        // Detect if the checkbox is checked
        var checked = $(this).prop('checked');
        // Gets the table row as indirect parent
        var trParent = $(this).parents('tr');
        // Add or remove the css class according to the checked state
        if (checked == true)
            trParent.addClass('selected')
        else
            trParent.removeClass('selected');
    })

    // Select all click
    $("#cbSelectAll").change(function () {
        var checked = $(this).prop('checked');
        $('.cbSelectRow').prop('checked', checked).trigger('change');
    });

});