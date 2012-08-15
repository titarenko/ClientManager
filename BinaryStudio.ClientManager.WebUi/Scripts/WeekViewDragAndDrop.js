$(function () {

    var dragMove = function (dayFromId, dayToId, inquiryId) {
        var date = $('#' + dayToId).attr('data');

        var onSuccess = function () {

        };

        var onError = function () {
            ShowAlert("Error while AJAX request");
        };

        if (dayFromId != dayToId) {
            $.ajax({
                url: "/Inquiries/MoveTo",
                type: "POST",
                data: { inquiryId: inquiryId, date: date }
            }).success(onSuccess).error(onError);
        }
    };



    var dayFromId, dayToId, inquiryId;
    $("#day0, #day1, #day2, #day3, #day4").sortable({
        connectWith: ".inquiry-list",
        cursor: 'move',
        placeHolder: "highlight", //TODO highlight in a place holder. Change class in shared.less
        receive: function (event, ui) {
            dayToId = $(this).closest(".inquiry-list").attr("id");
            dayFromId = $(ui.sender).attr("id");
            inquiryId = $(ui.item).attr("id");
            dragMove(dayFromId, dayToId, inquiryId);
        }
    }).disableSelection();
});