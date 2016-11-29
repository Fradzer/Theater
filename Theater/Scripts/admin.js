function FillUpdateForm(id) {
    $("#id-update").val(id);
    $("#id-update").attr("value", id);

    $("#name-update").val($("#name__" + id).text());
    $("#update-form").removeClass("hide");
}