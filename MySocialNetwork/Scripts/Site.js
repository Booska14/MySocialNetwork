$(document).ready(function () {
    var status = $(".status");
    status.find(".update").hide();
    status.find(".edit").hide();
    status.find(".delete").hide();

    $(".comment").on("click", "button", function () {
        var comment = $(this).closest("li");

        comment.find(".text").first().toggle();
        comment.find(".update").first().toggle();
    });

    $(".comment").on("mouseenter", "li", function () {
        $(this).find(".edit").show();
        $(this).find(".delete").show();
    });

    $(".comment").on("mouseleave", "li", function () {
        $(this).find(".edit").hide();
        $(this).find(".delete").hide();
    });

    $(".status").on("mouseenter", "li", function (event) {
        $(this).find(".delete").first().show();
    });

    $(".status").on("mouseleave", "li", function (event) {
        $(this).find(".delete").first().hide();
    });
});

function OnFormComplete() {
    $("input").filter("[name=text]").val("");
}