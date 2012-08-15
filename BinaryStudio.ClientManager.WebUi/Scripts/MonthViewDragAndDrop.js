$(function () {

    var dragMove = function (dayFromId, dayToId, inquiryId) {
        var date = $('#' + dayToId).attr('date');

        var onSuccess = function () {
            location.reload();
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

            //changeViewModel(dayFromId, dayToId, inquiryId);


        }
    };

    //        var changeViewModel = function (dayFromId, dayToId, inquiryId) {
    //            var indexFrom = parseInt(dayFromId.substring(3)) - 1;
    //            var indexTo = parseInt(dayToId.substring(3)) - 1;
    //            var dayFrom = window.viewModel.Weeks[div(indexFrom,7)].Days[indexFrom % 7];
    //            var dayTo = window.viewModel.Weeks[div(indexTo,7)].Days[indexTo % 7];
    //            for (var inquiryIndex in dayFrom.Inquiries) {
    //                var inquiry = dayFrom.Inquiries[inquiryIndex];
    //                if (inquiry.Id == inquiryId) {
    //                    dayTo.Inquiries.push(inquiry);
    //                    dayFrom.Inquiries.splice(inquiryIndex,1);
    //                }
    //            }
    //        };

    var dayFromId, dayToId, inquiryId;

    var selector = "";
    var dayCount = $('#month').attr('dayCount');
    for (var i = 1; i < dayCount - 1; i++) {
        selector += '#day' + i + ', ';
    }

    selector += '#day' + dayCount;

    $(selector).sortable({
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