$(document).ready(function () {
    $("#status").on("click", "button", function () {
        var status = $(this).closest("li");

        status.find(".message").first().toggle();
        status.find(".update").first().toggle();
    });

    $(".comment").on("click", "button", function (event) {
        var comment = $(this).closest("li");

        comment.find(".message").first().toggle();
        comment.find(".update").first().toggle();

        event.stopPropagation();
    });
});