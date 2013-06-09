$(document).ready(function () {
    $(".comment").on("click", "button", function (event) {
        var comment = $(this).closest("li");

        comment.find(".message").first().toggle();
        comment.find(".update").first().toggle();

        event.stopPropagation();
    });
});

function OnComplete() {
    $("input").filter("#Message[placeholder]").val("");
}