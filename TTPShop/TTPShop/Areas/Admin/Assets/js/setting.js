$(document).ready(function () {
    $("button").click(function () {
        var value = $(this).attr('id');
        var name = $(this).val();
        $("#name-to-delete").html(name);
        $("#MaTK").val(value);
    });
});