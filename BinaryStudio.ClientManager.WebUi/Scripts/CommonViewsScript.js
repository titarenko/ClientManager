function OnClickAddComment() {
        var comment = $('#commentTextArea').val();
        
        var onSuccess = function() {
            $('#commentTextArea').val('');
            ShowAlert('Your comment has been saved.');
        };

        var onError = function() {
            $('#commentTextArea').val('');
            ShowAlert('An error occured while saving comment.');
        };

        $.ajax({
            url: AddComment.Url,
            type: "POST",
            data: { inquiryId: $('#comment-editor').data("inquiryId"), text: comment }
        }).success(onSuccess).error(onError);
    }

    //when click on menu->add comment
    function ShowModal(inquiryId) {
        $('#comment-editor').data("inquiryId",inquiryId);               
    };

    function ShowAlert(message) {
        $('#saved-comment').text(message)
            .fadeIn('fast')
            .delay(3000)
            .fadeOut('slow');  
    }

    //When click on menu->assign to->some employee
    function OnClickAssign(employee, inquiry) {
        var onSuccess = function() {
            location.reload(true);
        };

        var onError = function() {
            alert("Error while AJAX request");
        };

        $.ajax({
            url: AssignTo.Url,
            type: "POST",
            data: { inquiryId: inquiry, employeeId: employee }
        }).success(onSuccess).error(onError);
    };

    //When click on menu->move to
    function OnClickMoveTo(inquiryId, date) {
        var onSuccess = function() {
            location.reload(true);
        };

        var onError = function() {
            alert("Error while AJAX request");
        };

        $.ajax({
            url: MoveTo.Url,
            type: "POST",
            data: { inquiryId: inquiryId, date: date }
        }).success(onSuccess).error(onError);
    };