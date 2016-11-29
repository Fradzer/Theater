function FillUpdateForm(id) {
    $("#id-update").val(id);

    $("#name-update").val($("#name__" + id).text());
    $("#update-form").removeClass("hide");
}

function FillUpdateFormPlay(id) {
    $("#id-update").val(id);

    $("#name-update").val($("#name__" + id).text());
    $("#authorId-update").val($("#authorId__" + id).text());
    $("#genreId-update").val($("#genreId__" + id).text());
    $("#description-update").val($("#description__" + id).text());
    $("#update-form").removeClass("hide");
}