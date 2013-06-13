$(document).ready(function () {
    $(".comment").on("click", "button", function (event) {
        var comment = $(this).closest("li");

        comment.find(".message").first().toggle();
        comment.find(".update").first().toggle();

        event.stopPropagation();
    });
});

function OnFormComplete() {
    $("input").filter("[name=text]").val("");
}