$(document).ready(function () {
    $(".status").find(".edit").hide();
    $(".status").find(".delete").hide();

    $(".comment").on("click", "button", function () {
        $(this).closest("li").find(".text").first().toggle();
        $(this).closest("li").find(".update").first().toggle();
    });

    $(".status").on("mouseenter", ".comment li", function () {
        $(this).find(".edit").show();
        $(this).find(".delete").show();
    });

    $(".status").on("mouseleave", ".comment li", function () {
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

function onAddStatusComplete() {
    $("input").first().val("");
    $(".status").find(".delete").first().hide();
}

function onRemoveStatusComplete() {
    var status = $(".status");

    status.find(".edit").hide();
    status.find(".delete").hide();
}

function onUpdateCommentComplete(sectionId) {
    $(sectionId).find(".edit").hide();
    $(sectionId).find(".delete").hide();
}

function onRemoveCommentComplete(sectionId) {
    $(sectionId).find(".edit").hide();
    $(sectionId).find(".delete").hide();
}

function onAddCommentComplete(sectionId) {
    $(sectionId).find("input").filter("[type=text]").last().val("");
    $(sectionId).find(".edit").last().hide();
    $(sectionId).find(".delete").last().hide();
}