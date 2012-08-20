$(function () {
    $(".employees").autocomplete({
        source: window.Employees,
        select: function (event, ui) {
            $(this).data('Id', ui.item.Id);
        }
    });
});

function CreateTeam() {
    var teamName = $('#new-team-name').val();
        
    var onSuccess = function () {
        $.get( '/Teams/Index', function(data) {
            $('#teams-tab').html(data);
        }); 
         $.get( '/Teams/CurrentTeamAndUser', function(data) {
            $('#current-team-and-user').html(data);
        }); 
    };

    var onError = function () {
        ShowAlert("Error while AJAX request");  
    };

    $.ajax({
        url: "/Teams/CreateTeam",
        type: "POST",
        data: { name: teamName }
    }).success(onSuccess).error(onError);
}

function RemoveUser(userId, teamId, teamName) {
    var onSuccess = function () {
        $.get( '/Teams/Index', function(data) {
            $('#teams-tab').html(data);
        }); 
        $.get( '/Teams/CurrentTeamAndUser', function(data) {
            $('#current-team-and-user').html(data);
        });
    };

    var onError = function () {
        ShowAlert("Error while AJAX request");
    };

    $.ajax({
        url: "/Teams/RemoveUser",
        type: "POST",
        data: { userId: userId, teamId: teamId }
    }).success(onSuccess).error(onError);
}
    
function AddUser(teamId,teamName) {
    var personId = $('#'+teamName+"-employees").data('Id');
        
    var onSuccess = function () {
        $.get( '/Teams/Index', function(data) {
            $('#teams-tab').html(data);
        }); 
         $.get( '/Teams/CurrentTeamAndUser', function(data) {
            $('#current-team-and-user').html(data);
        }); 
    };

    var onError = function () {
        ShowAlert("Error while AJAX request");  
    };

    $.ajax({
        url: '/Teams/AddUser',
        type: 'POST',
        data: { personId: personId, teamId: teamId }
    }).success(onSuccess).error(onError);
}

function MakeTeamCurrent(teamId) {
    var onSuccess = function () {
        $.get( '/Teams/Index', function(data) {
            $('#teams-tab').html(data);
        }); 
         $.get( '/Teams/CurrentTeamAndUser', function(data) {
            $('#current-team-and-user').html(data);
        }); 
    };

    var onError = function () {
        ShowAlert("Error while AJAX request");
    };

    $.ajax({
        url: "/Teams/MakeTeamCurrent",
        type: "POST",
        data: {teamId: teamId }
    }).success(onSuccess).error(onError);
}