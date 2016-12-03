function FillUpdateForm(id) {
    $("#id-update").val(id);

    $("#name-update").val($("#name__" + id).text());
    $("#update-form").removeClass("hide");
}

function FillUpdateFormPlay(id) {
    $("#id-update").val(id);

    $("#name-update").val($("#name__" + id).text());
    $("#author-update").val($("#author__" + id).text());
    $("#genre-update").val($("#genre__" + id).text());
    $("#description-update").val($("#description__" + id).text());
    $("#update-form").removeClass("hide");
}

function FillUpdateFormDatePlay(id) {
    $("#id-update").val(id);
    $("#play-update").val($("#play__" + id).text());
    $("#date-update").val($("#date__" + id).text());
    $("#update-form").removeClass("hide");
}